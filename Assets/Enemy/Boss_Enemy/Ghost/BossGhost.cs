using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGhost : Boss
{
    enum BossStatus //공격 <= 움직임 무시 
    {
        POINTCHECKING,// 정찰 지점 체크
        PATROLMOVING, // 지정 구간으로 움직임
        FOLLOW, //엄청 근접하게 다가오면 따라가기 <-> 벗어나면 POINTCHECKING
    }

    [Space(10f)]
    [SerializeField] BossStatus m_bossStatus;

    [SerializeField] SpriteRenderer followAreaSprite;

    Color[] setGhostAreaColor = { new Color(1, 1, 1, 0.15f), new Color(1, 0, 0, 0.15f) };

    Vector3 spawnPivot;

    [Header("Order of Attack")]
    int[] theOrderOfAttack = { 0, 0, 0, 1, 0, 0, 0, 0,1, 0, 0, 0,1};
    int theOrderOfAttackIndex = 0;

    [Header("Boss ProlMove_Move Info")]
    Vector3 moveArea; //울타리에 따라 움직일 수 있는 범위
    Vector3 moveToward; //상태에 따른 이동할 vect 
    float followDistance = 1.3f; //수정시 sprite Scale도 수정해야함.
    float checking_Timer_pathnode = 0; 
    float pointCheckingTime = 0.5f; //정찰 시간
    float checkingTimer_Animation = 0; //애니메이션 시간 체크

    [Header("Boss Attack_Spawn Bomb")]
    bool isShoot = false;

    public override void BossSpawnSet(bool _spawn)
    {
        if (_spawn)
        {
            attackInternalTime = 1.5f;

            spawnPivot = transform.position;

            m_bossStatus = BossStatus.POINTCHECKING;

            followAreaSprite.color = setGhostAreaColor[0];

            checkingTimer_Animation = 0;
        }
        else
        {
            StopAllCoroutines();

            animator.SetBool("IsAppear", true);
        }
    }

    void FixedUpdate()
    {
        if (!isDeath)
        {
            GhostAnimation();

            float currentDistance = (target.position - transform.position).magnitude;

            if (currentDistance > followDistance)
            {
                followAreaSprite.color = setGhostAreaColor[0];

                if (m_bossStatus != BossStatus.POINTCHECKING)
                    m_bossStatus = BossStatus.PATROLMOVING;

            }
            else if (currentDistance <= followDistance)
            {
                m_bossStatus = BossStatus.FOLLOW;
            }

            switch (m_bossStatus)
            {
                case BossStatus.POINTCHECKING:
                    checking_Timer_pathnode += Time.deltaTime;

                    if (checking_Timer_pathnode < pointCheckingTime)
                        return;

                    checking_Timer_pathnode = 0;
                    
                    float randomX = Random.Range(spawnPivot.x - 3, spawnPivot.x + 3);
                    float randomY = Random.Range(spawnPivot.y - 4, spawnPivot.y + 4);
                    moveArea = new Vector3(randomX, randomY, 0);

                    m_bossStatus = BossStatus.PATROLMOVING;
                    break;

                case BossStatus.PATROLMOVING: //정찰지역 따라가기

                    float distanceToTargetPoint = (moveArea - transform.position).magnitude;

                    moveToward = moveArea;

                    if (distanceToTargetPoint < 0.1f)//transform.position == moveArea
                        m_bossStatus = BossStatus.POINTCHECKING;
                    break;

                case BossStatus.FOLLOW: //플레이어 따라가기

                    moveToward = target.position;

                    followAreaSprite.color = setGhostAreaColor[1];
                    break;
            }

            if(m_bossStatus != BossStatus.POINTCHECKING)
                transform.position = Vector3.MoveTowards(transform.position, moveToward, Time.fixedDeltaTime * bossSpeed);

            //Sprite Direction
            Vector3 direction = (moveToward - transform.position).normalized;
            m_bossSpriteRenderer.flipX = direction.x > 0 ? true : false;
        }
    }

    public override void BossAttack() 
    {
        if (!isDeath)
        {
            if (m_bossStatus != BossStatus.FOLLOW)
            {
                checkingTime_attack += Time.deltaTime;

                if (checkingTime_attack > attackInternalTime)
                {
                    checkingTime_attack = 0;

                    if (!isShoot)
                        StartCoroutine(SpawnMineBomb());
                }
            }
        }
    }

    IEnumerator SpawnMineBomb()
    {
        isShoot = true;

        int SpawnRandom = theOrderOfAttack[theOrderOfAttackIndex];

        if (SpawnRandom == 0)
        {
            GameObject _pinkBomb = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_GHOST_PINK_MINE);
            if (_pinkBomb)
            {
                _pinkBomb.gameObject.SetActive(true);
                _pinkBomb.GetComponent<MineBomb>().BombSpawnTransform(transform);
            }
        }
        else
        {
            GameObject _blueBomb = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_GHOST_BLLUE_MINE);
            if (_blueBomb)
            {
                _blueBomb.gameObject.SetActive(true);
                _blueBomb.GetComponent<MineBomb>().BombSpawnTransform(transform);
            }
        }

        theOrderOfAttackIndex++;

        if (theOrderOfAttackIndex >= theOrderOfAttack.Length)
            theOrderOfAttackIndex = 0;


        yield return new WaitForSeconds(0.1f);

        isShoot = false;
    }

    void GhostAnimation()
    {
        float internalAnimation = 5f;

        checkingTimer_Animation += Time.fixedDeltaTime;

        if (checkingTimer_Animation >= 0 && checkingTimer_Animation < internalAnimation )
        {
            animator.SetBool("IsAppear", true); //idle 상태
        }
        else if (checkingTimer_Animation >= internalAnimation && checkingTimer_Animation < internalAnimation * 1.5)
        {
            animator.SetBool("IsAppear", false); //사라지는 상태
        }
        else if (checkingTimer_Animation >= internalAnimation * 1.5f)
        {
            checkingTimer_Animation = 0;
        }
    }
}
