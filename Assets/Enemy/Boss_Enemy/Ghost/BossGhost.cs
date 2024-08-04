using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGhost : Boss
{
    enum BossStatus //���� <= ������ ���� 
    {
        POINTCHECKING,// ���� ���� üũ
        PATROLMOVING, // ���� �������� ������
        FOLLOW, //��û �����ϰ� �ٰ����� ���󰡱� <-> ����� POINTCHECKING
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
    Vector3 moveArea; //��Ÿ���� ���� ������ �� �ִ� ����
    Vector3 moveToward; //���¿� ���� �̵��� vect 
    float followDistance = 1.3f; //������ sprite Scale�� �����ؾ���.
    float checking_Timer_pathnode = 0; 
    float pointCheckingTime = 0.5f; //���� �ð�
    float checkingTimer_Animation = 0; //�ִϸ��̼� �ð� üũ

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

                case BossStatus.PATROLMOVING: //�������� ���󰡱�

                    float distanceToTargetPoint = (moveArea - transform.position).magnitude;

                    moveToward = moveArea;

                    if (distanceToTargetPoint < 0.1f)//transform.position == moveArea
                        m_bossStatus = BossStatus.POINTCHECKING;
                    break;

                case BossStatus.FOLLOW: //�÷��̾� ���󰡱�

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
            animator.SetBool("IsAppear", true); //idle ����
        }
        else if (checkingTimer_Animation >= internalAnimation && checkingTimer_Animation < internalAnimation * 1.5)
        {
            animator.SetBool("IsAppear", false); //������� ����
        }
        else if (checkingTimer_Animation >= internalAnimation * 1.5f)
        {
            checkingTimer_Animation = 0;
        }
    }
}
