using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WhistlingArrowPlayState
{
    Idle,//대기 상태
    Targeting,
    Follow,//NormalEnemy Attack
    Attack,//BossAttack
}

public class OneWhistlingArrow : MonoBehaviour , IPlayerBullet
{
    AudioSource skillAudio;
    [SerializeField] AudioClip skillClip;

    CameraMove m_cameraMove;
    Drillshot m_drillshot;

    WhistlingArrowPlayState m_whistlingArrowPlayState = WhistlingArrowPlayState.Idle;

    int skillLevel = 5;

    Transform bullerPoolerTransform;
    Transform targetTransform;

    [Header("Boss Attack")]
    Transform angularTransform;
    float angularSpeed;

    float moveSpeed = 15f;
    float rotationLerpValue = 1f;

    bool isTimeToReady = false;

    public void InitializedData(Drillshot _drillshot, CameraMove _cameraMove, AudioSource _skillAudio, Transform _bullerPoolerTransform)
    {
        m_drillshot = _drillshot;
        m_cameraMove = _cameraMove;
        skillAudio = _skillAudio;
        bullerPoolerTransform = _bullerPoolerTransform;

        if (StageManager.playState == PlayState.BossCombat)
        {
            // 초기 위치 설정
            targetTransform = GetWhistlingArrowTargetPoint();
            angularTransform = targetTransform.gameObject.transform.GetChild(0).transform;
            transform.SetParent(angularTransform);
            transform.localPosition = Vector3.zero + new Vector3(angularTransform.localPosition.x, 0, 0);
            transform.localRotation = Quaternion.identity;
        }
    }

    public void SetToReady(bool _isTimeToReady) //*******************************보스 전인데  5렙 될경우
    {
        isTimeToReady = _isTimeToReady;

        if (isTimeToReady) //BossRedy
        {
            m_whistlingArrowPlayState = WhistlingArrowPlayState.Idle;

        }
        else if (!isTimeToReady)
        {
            skillAudio.PlayOneShot(skillClip);

            if (StageManager.playState == PlayState.BossCombat)
            {
                // 초기 위치 설정
                targetTransform = GetWhistlingArrowTargetPoint();
                angularTransform = targetTransform.gameObject.transform.GetChild(0).transform;
                transform.SetParent(angularTransform);
                transform.localPosition = Vector3.zero + new Vector3(angularTransform.localPosition.x, 0, 0);
                transform.localRotation = Quaternion.identity;
            }
            else if (StageManager.playState != PlayState.BossCombat)
            {
                transform.SetParent(bullerPoolerTransform.gameObject.transform);
            }

            m_whistlingArrowPlayState = WhistlingArrowPlayState.Targeting;
        }
    }
    void SetAttackTransform()
    {

    }

    private void Update()
    {
        switch (m_whistlingArrowPlayState)
        {
            case WhistlingArrowPlayState.Idle://휴식

                transform.position = Vector3.Lerp(transform.position, transform.position, 0.05f);

                transform.localRotation = Quaternion.identity;
                break;
            case WhistlingArrowPlayState.Targeting: //대기 + 타겟팅            

                targetTransform = GetWhistlingArrowTargetPoint();

                if ((targetTransform == m_drillshot.transform))
                    return;
                
                bool isAliveEnemy = targetTransform != null && targetTransform.gameObject.activeInHierarchy;
                if (isAliveEnemy && StageManager.playState == PlayState.NormalBattle)
                    m_whistlingArrowPlayState = WhistlingArrowPlayState.Follow;
                else if (isAliveEnemy && StageManager.playState == PlayState.BossCombat)
                    m_whistlingArrowPlayState = WhistlingArrowPlayState.Attack;
                break;
            case WhistlingArrowPlayState.Follow: //추적(Enemy공격)
                if (targetTransform == null || !targetTransform.gameObject.activeInHierarchy)
                {
                    targetTransform = GetWhistlingArrowTargetPoint();
                    m_whistlingArrowPlayState = WhistlingArrowPlayState.Targeting;
                }
                else
                    Move();
                break;
            case WhistlingArrowPlayState.Attack: //공격
                Attack();
                break;

        }
    }

    private void Move()
    {
        if (isTimeToReady)
            return;

        float distanceToTargetPoint = (targetTransform.position - transform.position).magnitude;

        Vector2 forwardNormal = (targetTransform.position - transform.position).normalized;
        float radian = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
        float angle = radian * (180 / Mathf.PI);

        Quaternion lookRotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(Vector2.one * Time.deltaTime * moveSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, lookRotation, rotationLerpValue);

        bool isContactEnemy = GetComponent<CapsuleCollider2D>().IsTouching(targetTransform.gameObject.GetComponent<Collider2D>());

        bool isTargetUpdateNeeded = !targetTransform.gameObject.activeInHierarchy || targetTransform.gameObject == null;

        if (isTargetUpdateNeeded || (distanceToTargetPoint < GetComponent<CapsuleCollider2D>().size.x || isContactEnemy))
        {
            targetTransform = GetWhistlingArrowTargetPoint();
            m_whistlingArrowPlayState = WhistlingArrowPlayState.Targeting;
        }
    }

    void Attack()
    {
        angularTransform.Rotate(0,0, angularSpeed * Time.deltaTime);
        //angularTransform.rotation = Quaternion.Euler(0, 0, angularSpeed * Time.deltaTime);

        if (targetTransform.gameObject.GetComponent<BossStat>().isDeath)
        {
            transform.SetParent(bullerPoolerTransform);
            m_whistlingArrowPlayState = WhistlingArrowPlayState.Idle;
        }
    }

    public void UpdateSkillLevel(int _level )
    {
        skillLevel = _level;

        skillAudio.PlayOneShot(skillClip);
    }

    public void UpdateSkillInfo(float _speed,float _angularSpeed)
    {
        moveSpeed = _speed;

        angularSpeed = _angularSpeed;
    }

    public void SetPlayState(WhistlingArrowPlayState _stat)
    {
        m_whistlingArrowPlayState = _stat;
    }

    public void Fire(Vector3 shootVect) { }
    public void SetMuzzleTransform(Transform muzzleTransform) { }

    Transform GetWhistlingArrowTargetPoint()
    {
        if (StageManager.playState == PlayState.NormalBattle)
        {
            float cameraSize = m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize;
            Vector3 colliderBox = new Vector3(cameraSize, cameraSize * 1.8f, 0);
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            Collider2D[] detectEnemyArea = Physics2D.OverlapBoxAll(m_drillshot.transform.position, colliderBox, 0f, enemyLayer);

            if (detectEnemyArea.Length > 0)
            {
                float shortDistance = Mathf.Infinity;

                foreach (Collider2D enemyCollider in detectEnemyArea)
                {
                    if (enemyCollider.CompareTag("Enemy") && enemyCollider != null)
                    {
                        bool biggerDrillX_thenPlayerX = transform.position.x > m_drillshot.transform.position.x;
                        bool biggerDrillY_thenPlayerY = transform.position.y > m_drillshot.transform.position.y;

                        bool biggerEnemyX_thenPlayerX = enemyCollider.transform.position.x > m_drillshot.transform.position.x;
                        bool biggerEnemyY_thenPlayerY = enemyCollider.transform.position.y > m_drillshot.transform.position.y;

                        float distanceFromEnemy = (enemyCollider.gameObject.transform.position - m_drillshot.transform.position).magnitude;

                        if (biggerDrillX_thenPlayerX && biggerDrillY_thenPlayerY) //WhistlingArrow 1사분면에 위치
                        {
                            if (!(biggerEnemyX_thenPlayerX && biggerEnemyY_thenPlayerY))//1사분면 제외한 Enemy
                            {
                                if (shortDistance > distanceFromEnemy)
                                {
                                    shortDistance = distanceFromEnemy;
                                    if (enemyCollider.gameObject != null && enemyCollider.gameObject.activeInHierarchy)
                                        return enemyCollider.transform;
                                }
                            }
                        }
                        else if (!biggerDrillX_thenPlayerX && biggerDrillY_thenPlayerY)  //WhistlingArrow 2사분면에 위치
                        {
                            if (!(!biggerEnemyX_thenPlayerX && biggerEnemyY_thenPlayerY))  //2사분면 제외한 Enemy
                            {
                                if (shortDistance > distanceFromEnemy)
                                {
                                    shortDistance = distanceFromEnemy;
                                    if (enemyCollider.gameObject != null && enemyCollider.gameObject.activeInHierarchy)
                                        return enemyCollider.transform;
                                }
                            }
                        }
                        else if (!biggerDrillX_thenPlayerX && !biggerDrillY_thenPlayerY)  //WhistlingArrow 3사분면에 위치 
                        {
                            if (!(!biggerEnemyX_thenPlayerX && !biggerEnemyY_thenPlayerY))  //3사분면 제외한 Enemy
                            {
                                if (shortDistance > distanceFromEnemy)
                                {
                                    shortDistance = distanceFromEnemy;
                                    if (enemyCollider.gameObject != null && enemyCollider.gameObject.activeInHierarchy)
                                        return enemyCollider.transform;
                                }
                            }
                        }
                        else if (biggerDrillX_thenPlayerX && !biggerDrillY_thenPlayerY) //WhistlingArrow 4사분면에 위치
                        {
                            if (!(biggerEnemyX_thenPlayerX && !biggerEnemyY_thenPlayerY)) //4사분면제외한 Enemy
                            {
                                if (shortDistance > distanceFromEnemy)
                                {
                                    shortDistance = distanceFromEnemy;
                                    if (enemyCollider.gameObject != null && enemyCollider.gameObject.activeInHierarchy)
                                        return enemyCollider.transform;
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (StageManager.playState == PlayState.BossCombat)
        {
            GameObject _currentBossObject = EnemyPooler.Instance.GetCurrentBosstData();

            if (_currentBossObject != null && _currentBossObject.activeInHierarchy)
                return _currentBossObject.transform;
            else
                return null;
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DRILLSHOT);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDrillshot>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DRILLSHOT);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDrillshot>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DRILLSHOT);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDrillshot>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("PickupBox"))
        {
            PickupBox _box = collision.gameObject.GetComponent<PickupBox>();
            if (_box)
            {
                _box.GetComponent<PickupBox>().SetBoxState(false);
            }
        }
    }
}