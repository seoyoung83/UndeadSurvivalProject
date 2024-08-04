using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNinjaFrog : Boss
{
    enum BossAttackType
    {
        ATTACK_FIRE_AT_TARGET, //허수아비 6개 랜덤 생성 후, 생성 순서대로 허수아비 방향으로 공격  + Move (Ptrol Map)
        ATTACK_STARS_BIGTHREE,  //다가가서 총알 발사 3방향+ Move( 플레이어 마지막 Pos)
        ATTACK_STARS_HALFCIRCLE,     //다가가서 총알 발사 7방향 + Move(플레이어를 향해 돌진 후 벽에 닿으면 방대방향으로 총알 발사)
        READY, //// 기본 Move : Player Ptrol
    }

    [SerializeField] BossAttackType m_bossAttackType;

    [SerializeField] Transform bulletMuzzleCenter;

    Vector3 spawnPivot;

    float attackDistanceToPlayer = 3f;

    [Header("Order of Attack")]
    int[] theOrderOfAttack = { 0, 1, 2, 0, 0, 1, 2, 0, 1, 1, 2, 2, 1, 0, 2 };
    int theOrderOfAttackIndex = 0;

    [Header("[Attack] Fire at Target _ Scarecrow")]
    int scarecrowSpawnCount = 8; //허수아비 &불렛 갯수 동일
    float moveToX_targetAttack; //공격 전 이동할 x축
    float moveToY_targetAttack; //공격 전 이동할 x축
    List<Vector2> scarecrowSpawnVect = new List<Vector2>();

    [Header("[Attack] Star_Big Three")]
    int bigThreeStarShootCount = 3; 
    float moveToX_bigThree; //공격 전 이동할 x축
    float bigThreeWidth = 0.4f;

    [Header("[Attack] Star_Half Circle")]
    int refletiveStarShootDrectCount= 7; 
    int refletiveStarShootCountByOneDrect= 3; 
    float moveToX_halfCircle; //공격 전 이동할 x축
    float halfCircleWidth = 0.6f;

    private void Awake()
    {
        m_bossAttackType = BossAttackType.READY;
    }

    public override void BossSpawnSet(bool _spawn)
    {
        if (_spawn)
        {
            attackInternalTime = 2f;

            spawnPivot = transform.position;

            m_bossAttackType = BossAttackType.READY;

            animator.SetBool("IsIdle", false);
            animator.SetBool("IsRun", true);
        }
        else
        {
            StopAllCoroutines();

            animator.SetBool("IsIdle", true);
            animator.SetBool("IsRun", false);
        }
    }

    void FixedUpdate()
    {
        if (!isDeath && m_bossAttackType == BossAttackType.READY) //Patrol Player
            Move();
    }

    public override void BossAttack()
    {
        if (!isDeath)
        {
            if (m_bossAttackType == BossAttackType.READY)
            {
                checkingTime_attack += Time.deltaTime;

                if (checkingTime_attack > attackInternalTime)
                {
                    checkingTime_attack = 0;

                    GetBossMoveArea();

                    int attackNumber = theOrderOfAttack[theOrderOfAttackIndex];

                    switch (attackNumber)
                    {
                        case (int)BossAttackType.ATTACK_FIRE_AT_TARGET:
                            StartCoroutine(FireAtTarget());
                            theOrderOfAttackIndex++;

                            break;
                        case (int)BossAttackType.ATTACK_STARS_BIGTHREE:
                            StartCoroutine(BigThreeStar());
                            theOrderOfAttackIndex++;
                            break;

                        case (int)BossAttackType.ATTACK_STARS_HALFCIRCLE:
                            StartCoroutine(ShootHalfCircleStars());
                            theOrderOfAttackIndex++;
                            break;                        
                    }
                    
                    if (theOrderOfAttackIndex >= theOrderOfAttack.Length)
                        theOrderOfAttackIndex = 0;

                }
            } 
        }
    }

    void GetBossMoveArea()
    {
        float moveTo;

        if (spawnPivot.x > target.position.x) //타일맵 중심에서 왼쪽
        {
            moveTo = target.position.x + attackDistanceToPlayer;

            moveToX_bigThree = moveTo * 0.7f;
            moveToX_halfCircle = moveTo;
        }
        else if (spawnPivot.x < target.position.x) //타일맵 중심에서 오른쪽
        {
            moveTo = target.position.x - attackDistanceToPlayer;

            moveToX_bigThree = moveTo * 0.7f;
            moveToX_halfCircle = moveTo;
        }

        moveToX_targetAttack = Random.Range(spawnPivot.x - 0.5f, spawnPivot.x + 0.5f);
        moveToY_targetAttack = Random.Range(spawnPivot.y - 0.5f, spawnPivot.y + 0.5f);
    }


    //=================[Fire At Target]===========
    IEnumerator FireAtTarget()
    {
        m_bossAttackType = BossAttackType.ATTACK_FIRE_AT_TARGET;

        //Move
        animator.SetBool("IsJump", true);

        Vector3 _moveTo = new Vector3(moveToX_targetAttack, moveToY_targetAttack, 0f);

        while (transform.position != _moveTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTo, Time.fixedDeltaTime * bossSpeed);
            yield return null;
        }

        animator.SetBool("IsJump", false);
        //Get Spawn Vector
        int count = 0;
        
        while (count < scarecrowSpawnCount)
        {
            //각도 다르게 스폰
            Quaternion _rotation = Quaternion.Euler(0f, 0f, count * (360 / scarecrowSpawnCount));

            float width = Random.Range(2f, 5f);

            Vector3 _direction = _rotation * Vector3.up * width;

            Vector3 _position = transform.position + _direction;

            Vector2 scarecrowObjectSiz = new Vector2(1.1f, 1.5f);

          //  int targetLayerMask = 1 << LayerMask.NameToLayer("BossRaidFence") | 1 << LayerMask.NameToLayer("Player");

            Collider2D colliderDetecter = Physics2D.OverlapBox(_position, scarecrowObjectSiz, 0, LayerMask.NameToLayer("BossRaidFence"));

            if (colliderDetecter == null)
            {
                scarecrowSpawnVect.Add(_position);
                count++;
            }
            yield return null;
        }

        //Spawn_Fx
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsRun", false);

        int spawnFx = 0;
        while (spawnFx < scarecrowSpawnCount)
        {
            GameObject _pressedCircleFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_PRESSEDCIRCLE);
            if (_pressedCircleFx != null)
            {
                _pressedCircleFx.transform.position = scarecrowSpawnVect[spawnFx];
                _pressedCircleFx.SetActive(true);
                spawnFx++;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        //Spawn_Scarecrow Object 
        int spawnScarecrow = 0;
        while (spawnScarecrow < scarecrowSpawnCount)
        {
            SpawnScarecrowTarget(scarecrowSpawnVect[spawnScarecrow]);
            spawnScarecrow++;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        
        //Shoot Bullet AT Scarecrow Object 
        int fireBullet = 0;
        while (fireBullet < scarecrowSpawnCount)
        {
            Vector2 shootVect = new Vector3(scarecrowSpawnVect[fireBullet].x, scarecrowSpawnVect[fireBullet].y + 0.6f, 0);
            ShootFireBullet(shootVect);//허수아비 중심
            fireBullet++;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        scarecrowSpawnVect.Clear();

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRun", true);

        m_bossAttackType = BossAttackType.READY;
    }

    void SpawnScarecrowTarget(Vector3 _landingVect)
    {
        GameObject _target = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET);
        if (_target)
        {
            _target.gameObject.SetActive(true);
            _target.GetComponent<ScarecrowTarget>().GetSpawnPositionInfo(_landingVect);
        }

    }

    void ShootFireBullet(Vector3 _moveVect)
    {
        GameObject _fireBullet = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_FIREBULLET);
        if (_fireBullet)
        {
            _fireBullet.gameObject.SetActive(true);
            _fireBullet.GetComponent<FireBulletAtTarget>().SetMuzzleTransform(transform);
            _fireBullet.GetComponent<FireBulletAtTarget>().Shooting(_moveVect);
        }
    }

    //===============[Big Three Throwing Star]========================
    IEnumerator BigThreeStar()
    {
        m_bossAttackType = BossAttackType.ATTACK_STARS_BIGTHREE;

        //Move
        animator.SetBool("IsJump", true);

        while (transform.position.x != moveToX_bigThree)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveToX_bigThree, target.position.y), Time.fixedDeltaTime * (bossSpeed * 1.5f));
            m_bossSpriteRenderer.flipX = target.position.x < transform.position.x;
            yield return null;
        }
        animator.SetBool("IsJump", false);

        yield return new WaitForSeconds(0.35f);

        //Shoot
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsRun", false);

        int count = 0;
        while (count < bigThreeStarShootCount)
        {
            Quaternion _rotation = Quaternion.Euler(0f, 0f, (count + 1) * 45f);

            float width = transform.position.x > target.position.x ? bigThreeWidth : -bigThreeWidth;

            Vector3 _direction = _rotation * Vector3.up * width;

            Vector3 _position = bulletMuzzleCenter.position + _direction;

            ShootBigStar(_position, _direction);

            count++;
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRun", true);

        m_bossAttackType = BossAttackType.READY;
    }

    void ShootBigStar(Vector3 _spawnVect, Vector3 _direction)
    {
        GameObject _bigStar = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTAR);
        if (_bigStar)
        {
            _bigStar.gameObject.SetActive(true);
            _bigStar.GetComponent<BigThrowingStar>().SetMuzzleTransform(_spawnVect);
            _bigStar.GetComponent<BigThrowingStar>().Shoot(_direction);
        }
    }

    //===============[Half Circle Throwing Star]========================
    IEnumerator ShootHalfCircleStars()
    {
        m_bossAttackType = BossAttackType.ATTACK_STARS_HALFCIRCLE;

        //Move
        animator.SetBool("IsJump", true);

        while (transform.position.x != moveToX_halfCircle)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveToX_halfCircle, target.position.y), Time.fixedDeltaTime * (bossSpeed * 1.5f));
            m_bossSpriteRenderer.flipX = target.position.x < transform.position.x;
            yield return null;
        }
        animator.SetBool("IsJump", false);

        yield return new WaitForSeconds(0.35f);

        //Shoot
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsRun", false);

        int count = 0;
        while (count < refletiveStarShootDrectCount)
        {
            StartCoroutine(HalfCircleOneStar(count));
            count++;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRun", true);

        m_bossAttackType = BossAttackType.READY;
    }

    IEnumerator HalfCircleOneStar(int _count)
    {
        int groupCount = 0;

        while (groupCount < refletiveStarShootCountByOneDrect)
        {
            Quaternion _rotation = Quaternion.Euler(0f, 0f, _count * 30f);

            float width = transform.position.x > target.position.x ? halfCircleWidth : -halfCircleWidth;

            Vector3 _direction = _rotation * Vector3.up * width;

            Vector3 _position = bulletMuzzleCenter.position + _direction;

            ShootRefletiveStar(_position, _direction);

            groupCount++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ShootRefletiveStar(Vector3 _spawnVect, Vector3 _direction)
    {
        GameObject _reflectiveStar = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_REFLECTIVE_THROWINGSTAR);
        if (_reflectiveStar)
        {
            _reflectiveStar.gameObject.SetActive(true);
            _reflectiveStar.GetComponent<ReflectiveThrowingStar>().SetMuzzleTransform(_spawnVect);
            _reflectiveStar.GetComponent<ReflectiveThrowingStar>().GetInitialVelocity(_direction);
        }

    }
} 
