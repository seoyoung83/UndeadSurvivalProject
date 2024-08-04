using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBunny : Boss
{
    enum BossAttackType
    {
        ATTACK_JUMP,
        ATTACK_RUST,
        ATTACK_CARROT,
        READY,
    }

    [Space(10f)]
    [SerializeField] BossAttackType m_bossAttackType;

    bool isBossAttacking;//공격중 들어갔을때

    [Header("Order of Attack")]
    int[] theOrderOfAttack = { 0, 1, 2, 0, 0, 2, 1, 1, 0, 0, 2, 1, 1, 0, 2 }; 
    int theOrderOfAttackIndex = 0;

    [Header("[Attack] Jump Attack")]
    [SerializeField] Transform[] round_ShootPositoin;
    Transform jumpAttackTargetPoint; // GroundFx 활성화된 Transform
    AnimationCurve jumpCurve = new AnimationCurve(); //Y축 점프 커브
    float jumpDuration; //지속 시간  

    [Header("[Attack] Rush Attack")]
    [SerializeField] GameObject rushAttackArrowObject;
    SpriteRenderer arrowSprite;

    [Header("[Attack] Carrot Attack")]
    [SerializeField] int loadedCarrot = 4;

    private void Awake()
    {
        arrowSprite = rushAttackArrowObject.GetComponentInChildren<SpriteRenderer>();
    }

    public override void BossSpawnSet(bool _spawn)
    {
        if (_spawn)
        {
            attackInternalTime = 1.5f;

            m_bossAttackType = BossAttackType.READY;

            isBossAttacking = false;
            jumpCurve = new AnimationCurve();

            animator.SetBool("IsRun", true);
            animator.SetBool("IsIdle", false);

            arrowSprite.color = new Color(1, 0, 0, 0);

            rushAttackArrowObject.gameObject.SetActive(false);
        }
        else
        {
            StopAllCoroutines();

            rushAttackArrowObject.gameObject.SetActive(false);

            animator.SetBool("IsIdle", true);
            animator.SetBool("IsRun", false);
        }
    }

    void FixedUpdate() 
    {
        if (!isDeath && !isBossAttacking)
            Move();
    }

    public override void BossAttack()
    {
        if (!isDeath)
        {
            if (m_bossAttackType==BossAttackType.READY)
            {
                checkingTime_attack += Time.deltaTime;

                if (checkingTime_attack > attackInternalTime)
                {
                    checkingTime_attack = 0;
                    
                    int attackNumber = theOrderOfAttack[theOrderOfAttackIndex];

                    switch (attackNumber)
                    {
                        case (int)BossAttackType.ATTACK_JUMP:
                            StartCoroutine(JumpAttack(2f));
                            theOrderOfAttackIndex++;
                            break;
                        case (int)BossAttackType.ATTACK_RUST:
                            StartCoroutine(RushAttack());
                            theOrderOfAttackIndex++;
                            break;
                        case (int)BossAttackType.ATTACK_CARROT:
                            if (loadedCarrot > 0)
                            {
                                StartCoroutine(CarrotAttack());
                                theOrderOfAttackIndex++;
                            }
                            else
                                theOrderOfAttackIndex++;
                            break;
                    }
                    if (theOrderOfAttackIndex >= theOrderOfAttack.Length)
                        theOrderOfAttackIndex = 0;
                }
            }

            if (m_bossAttackType == BossAttackType.ATTACK_RUST)
            {
                //Rush Attack Arrow
                if (!isBossAttacking) //스위치로 공격 타입 걸기
                {
                    Vector3 targetVect = (target.position - transform.position).normalized;
                    float radValue = Mathf.Atan2(targetVect.y, targetVect.x);
                    float shootAngle = radValue * (180 / Mathf.PI);
                    rushAttackArrowObject.transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
                }
            }
            
        }
    }

    IEnumerator JumpAttack(float _time)
    {
        m_bossAttackType = BossAttackType.ATTACK_JUMP;

        //JUMP ATACK _Big Ground Dust 전조
        GameObject _dangerGroundFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DANGEROUSGROUND);

        if (_dangerGroundFx != null)
        {
            _dangerGroundFx.transform.position = target.position; 
            _dangerGroundFx.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            _dangerGroundFx.GetComponent<DangerousPointFx>().activeTime = _time;

            jumpDuration = _time / 2;//jump 지속시간 저장_dangerGroundFx가 반틈 지속된 시간 후에 점프!
            jumpAttackTargetPoint = _dangerGroundFx.transform; //jump 목표 지점 저장

            _dangerGroundFx.SetActive(true);
        }

        yield return new WaitForSeconds(jumpDuration / 2);

        //JUMP
        isBossAttacking = true;

        animator.SetBool("IsRun", false);
        animator.SetBool("IsJump", true);
        GetComponent<CapsuleCollider2D>().enabled = false;

        jumpCurve = new AnimationCurve();//초기화
        jumpCurve.AddKey(0f, transform.position.y + 2);
        jumpCurve.AddKey(jumpDuration / 2, jumpAttackTargetPoint.position.y + 2);
        jumpCurve.AddKey(jumpDuration, jumpAttackTargetPoint.position.y);

        float jumpTimer = 0f;

        while (jumpTimer < jumpDuration) 
        {
            jumpTimer += Time.deltaTime;

            float jumpValue = jumpCurve.Evaluate(jumpTimer);

            transform.position = Vector3.Lerp(transform.position, new Vector3(jumpAttackTargetPoint.position.x, jumpValue, 0), jumpTimer / jumpDuration);
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(jumpAttackTargetPoint.position.x, jumpValue, 0), jumpTimer / jumpDuration);
            yield return null;
        }

        animator.SetBool("IsIdle", true);
        animator.SetBool("IsJump", false);

        //JUMP ATACK _Big Ground Dust
        GameObject _bigDust = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_BUNNY_BIGDUST);
        if (_bigDust != null)
        {
            _bigDust.transform.position = jumpAttackTargetPoint.position;
            _bigDust.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        GetComponent<CapsuleCollider2D>().enabled = true;

        //JUMP ATACK _Small Ground Dust 전조

        float miniDangerousGround_lifeTime = 0.5f;
        
        int groundCount = 0;
        while (groundCount < round_ShootPositoin.Length)
        {
            GameObject _miniDangerGroundFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DANGEROUSGROUND);
            if (_miniDangerGroundFx != null)
            {
                _miniDangerGroundFx.transform.position = round_ShootPositoin[groundCount].position;
                _miniDangerGroundFx.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                _miniDangerGroundFx.GetComponent<DangerousPointFx>().activeTime = miniDangerousGround_lifeTime;
                _miniDangerGroundFx.SetActive(true);
            }
            groundCount++;
            yield return null;
        }

        yield return new WaitForSeconds(miniDangerousGround_lifeTime - 0.1f);

        //JUMP ATACK _Small Ground Dust
        int dustCount = 0;
        while (dustCount < round_ShootPositoin.Length)
        {
            if (_dangerGroundFx != null) 
            {
                GameObject _smallDust = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_BUNNY_SMALLDUST);
                if (_bigDust != null)
                {
                    _smallDust.transform.position = round_ShootPositoin[dustCount].position;
                    _smallDust.SetActive(true);
                }
            }
            dustCount++;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRun", true);

        yield return new WaitForSeconds(0.5f);
        
        isBossAttacking = false;
        m_bossAttackType = BossAttackType.READY;
    }

    IEnumerator RushAttack()
    {
        m_bossAttackType = BossAttackType.ATTACK_RUST;

        //Rush Attack Arrow 조준

        rushAttackArrowObject.gameObject.SetActive(true);

        float alpha_min = 0;
        while(alpha_min < 0.8f)
        {
            alpha_min += Time.deltaTime * 1.5f;
            arrowSprite.color = new Color(1, 0, 0, alpha_min);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        //Rush Attack 전 잠시 멈추기

        isBossAttacking = true; //Move Stop
        
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsRun", false);

        yield return new WaitForSeconds(0.2f);

        //Arrow 사라지기
        float alpha_max = 0.7f;
        while (alpha_max < 0)
        {
            alpha_max -= Time.deltaTime * 1.5f;
            arrowSprite.color = new Color(1, 0, 0, alpha_max);
            yield return null;
        }

        rushAttackArrowObject.gameObject.SetActive(false);
        arrowSprite.color = new Color(1, 0, 0, 0); //리셋

        //Rush Attack_돌진
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRun", true);
        animator.SetFloat("RunSpeed", 2f);

        Vector3 targetVect = (target.position - transform.position).normalized;

        bool isContact = false;
        while (!isContact)
        {
            transform.position += targetVect * Time.deltaTime * 8;

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, targetVect, 0.6f, LayerMask.GetMask("BossRaidFence"));

            if (hitInfo.collider)
                isContact = true;

            yield return null;
        }        
        yield return new WaitForSeconds(0.2f);
        
        //Reset
        animator.SetFloat("RunSpeed", 1f);
        isBossAttacking = false;

        m_bossAttackType = BossAttackType.READY;
    }

    IEnumerator CarrotAttack()
    {
        int maxCount = loadedCarrot;
        m_bossAttackType = BossAttackType.ATTACK_CARROT;

        isBossAttacking = true; //Move Stop

        int count = 0;
        while (count < maxCount)
        {
            Vector3 _direction = (target.position - transform.position).normalized;
            ShootCarrot(_direction);
            count++;
            yield return new WaitForSeconds(0.5f);
        }
        
        yield return new WaitForSeconds(1.5f);

        isBossAttacking = false;

        m_bossAttackType = BossAttackType.READY;
    }

    void ShootCarrot(Vector3 _direction)
    {
        GameObject _carrot = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_BUNNY_CARROT);
        if (_carrot)
        {
            float distanceToBoss = 0.8f;
            Vector3 _position = transform.position + (_direction * distanceToBoss);
            _carrot.gameObject.SetActive(true);
            _carrot.GetComponent<OneCarrot>().SetMuzzleTransform(_position);
            _carrot.GetComponent<OneCarrot>().GetInitialVelocity(_direction);
            --loadedCarrot;
        }
    }
    
    public void CarrotBulletExpired(OneCarrot _bullet)
    {
        _bullet.gameObject.SetActive(false);
        _bullet.transform.position = transform.position;
        ++loadedCarrot;
    }
}
