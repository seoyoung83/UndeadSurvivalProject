using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drone : ActiveComboSkill
{
    protected PlayerSkillCategory thisSkillType;
    protected SkillType activeSkillType;
    protected PlayerBulletType bulletType;

    [Header("About Drone Body")]
    [SerializeField] Transform[] droneTrans; //Drone Transform [0]: 렙0-4 / [1]: 렙 5
    Vector3 initialDroneVect; //초기 세팅 위치
    bool moveUp = true;
    float checkingTimer_droneMove = 0f;

    [Header("About Reload")]
    float checkingTimer_reload = 0f;
    bool isCalculating = false; //isCalculation

    [Header("About Calculation Rocket Launch Vect")]
    const float circleRadius = 2.45f; // 공격 써클 반지름
    const int angleOfAttack = 24; // 25도 당 한번 슛
    float checkingTimer_launch = 0;
    float checkingTimer_currentRotaionAngle;
    float nextAttackRangeAngle = 90f;

    [Header("About Launch")]
    int numberOfRocketsFiredOnce;// 한번의 슛에 발사되는 로켓 수

    Vector3 launchVect; // 공격 타게팅 위치(한쪽)
    Vector3 launchVectReverse; // 공격 타게팅 위치(반대쪽)

    Queue<Vector3> targetingVectQueue = new Queue<Vector3>(); //Targeting Vect

    public abstract void Awake();

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        SetDroneBody(_currentWeaponSkillLevel);

        ResetSkill();

        isSettingUpData = true;
    }

    void ResetSkill()
    {
        StopAllCoroutines();

        targetingVectQueue.Clear();

        isCalculating = false;
        moveUp = true;

        checkingTimer_currentRotaionAngle = nextAttackRangeAngle;
        checkingTimer_droneMove = 0f;
        checkingTimer_reload = 0;
        checkingTimer_launch = 0f;

        numberOfRocketsFiredOnce = maxBulletCount / 15; //(생성 초기 로켓 수; skillDuration 증가 시 총 로켓량 증가) / 슛 횟수
    }

    public override void SetToReady(bool _isTimeToReady) { }

    private void Update()
    {
        if (isSettingUpData)
        {
            DroneMove();

            if (!isCalculating)
                ReloadDrone();
            else
                CalculationRocketTargetingVect();

            // 장전된 탄환(RocketTargeting Vect(슛 포지션) Enqueue > 0)이 있으면, ReloadDrone상태라도 shoot
            Skill_Fire(targetingVectQueue.Count > 0);
        }
    }

    void SetDroneBody(int _skillLevel)
    {
        if (_skillLevel == 0)
        {
            droneTrans[0].gameObject.SetActive(true);
            droneTrans[1].gameObject.SetActive(false);
            initialDroneVect = droneTrans[0].localPosition;
        }
        else if (_skillLevel == 5)
        {
            droneTrans[0].gameObject.SetActive(false);
            droneTrans[1].gameObject.SetActive(true);
            initialDroneVect = droneTrans[1].localPosition;
        }
    }

    void ReloadDrone()
    {
        checkingTimer_reload += Time.deltaTime;

        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        if (checkingTimer_reload > _attackInterval)
        {
            isCalculating = true;

            checkingTimer_reload = 0;

            checkingTimer_launch = 0f;

            checkingTimer_currentRotaionAngle = nextAttackRangeAngle;
        }
    }

    void CalculationRocketTargetingVect() // Targeting Vect(반원형) 계산 후, Targeting Vect Enqueue
    {
        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100); // 공격 지속 각도
        float _attackSpeed = attackSpeed - (attackSpeed * buffValue_AttackSpeed / 100); // 공격스피드 = attackableAngle 를 도는데 걸리는 시간 (감소 = 속도 향상)
        float rotationSpeed = _skillDuration / _attackSpeed;   //  (ex 360 / 6.5f) (ex 180 / 4f)

        checkingTimer_currentRotaionAngle += Time.deltaTime * rotationSpeed;

        int shootCountBySkillDuration = (int)Mathf.Round(_skillDuration / angleOfAttack); //한번의 스킬 회전에서의 슛 카운트

        if (checkingTimer_currentRotaionAngle >= nextAttackRangeAngle + _skillDuration)
        {
            isCalculating = false;

            nextAttackRangeAngle = (skillLevel != 5) ? checkingTimer_currentRotaionAngle - 540f : checkingTimer_currentRotaionAngle - 360f;
        }
        else
        {
            Vector3 fromAngleToVect = new Vector3(Mathf.Cos(checkingTimer_currentRotaionAngle * Mathf.Deg2Rad),
                Mathf.Sin(checkingTimer_currentRotaionAngle * Mathf.Deg2Rad), 0);

            launchVect = (fromAngleToVect * circleRadius);

            if (skillLevel == 5)
                launchVectReverse =   (new Vector3(-fromAngleToVect.x, fromAngleToVect.y, 0) * circleRadius);

            checkingTimer_launch += Time.deltaTime;
            if (checkingTimer_launch >= _attackSpeed / shootCountBySkillDuration)
            {
                checkingTimer_launch = 0f;
                StartCoroutine(StartRocketsFiredOnce());
            }
        }
    }

    void Skill_Fire(bool isExistReloadTargeting)
    {
        if (!isExistReloadTargeting)
            return;

        skillAudio.Play();

        Vector3 shootVect = targetingVectQueue.Dequeue();
        DroneRocketShoot(transform.position + shootVect);
    }

    //드론 본체 move
    void DroneMove()
    {
        if (moveUp)
        {
            checkingTimer_droneMove += Time.deltaTime * 0.2f;

            if (checkingTimer_droneMove > 0.1f)
                moveUp = false;
        }
        else
        {
            checkingTimer_droneMove -= Time.deltaTime * 0.2f;

            if (checkingTimer_droneMove < -0.1f)
                moveUp = true;
        }

        Transform tempDroneTrans = skillLevel != 5 ? droneTrans[0] : droneTrans[1];
        Vector3 wantedVect = new Vector3(initialDroneVect.x, initialDroneVect.y + checkingTimer_droneMove, 0f);
        tempDroneTrans.transform.localPosition = Vector3.Lerp(tempDroneTrans.transform.localPosition, wantedVect, 0.1f);

        Vector3 playerJoy = playerTransform.gameObject.GetComponent<PlayerMove>().JoysticVector;

        if (playerJoy.x > 0)
            tempDroneTrans.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (playerJoy.x < 0)
            tempDroneTrans.gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    IEnumerator StartRocketsFiredOnce() //RocketTargeting 위치(슛 포지션) Enqueue & RocketTargeting Spawn
    {
        int targetingCount = 0;

        while (targetingCount < numberOfRocketsFiredOnce)
        {
            Vector3 randomVect = new Vector3(launchVect.x - Random.Range(0, 0.8f), launchVect.y - Random.Range(0, 0.8f), 0);
            DroneRocketTargetingSpawn(randomVect);
            targetingVectQueue.Enqueue(randomVect);

            if (skillLevel == 5)
            {
                Vector3 randomVect_reverse = new Vector3((launchVectReverse.x - Random.Range(0, 0.8f)), launchVectReverse.y - Random.Range(0, 0.8f), 0);
                DroneRocketTargetingSpawn(randomVect_reverse);
                targetingVectQueue.Enqueue(randomVect_reverse);
            }
            targetingCount++;

            yield return new WaitForSeconds(0.15f);
        }
        yield return null;
    }

    void DroneRocketShoot(Vector3 _shootVect) //Damage가 들어가는 RocketExplosion은 DroneRocket 스크립트 내에.
    {
        GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(bulletType);

        Transform tempDroneTrans = skillLevel != 5 ? droneTrans[0] : droneTrans[1];

        if (_bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<DroneRocket>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<DroneRocket>().UpdateSkillInfo(activeSkillType, buffValue_AttackRang);
            _bullet.GetComponent<DroneRocket>().SetMuzzleTransform(tempDroneTrans.transform);
            _bullet.GetComponent<DroneRocket>().Fire(_shootVect);
        }
    }

    void DroneRocketTargetingSpawn(Vector3 _shootVect) // 발사 전 타게팅 표시
    {
        GameObject _targetingFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DRONE_ROCKET_TARGETING);
        if (_targetingFx != null)
        {
            _targetingFx.transform.position = transform.position + _shootVect;
            _targetingFx.SetActive(true);
        }
    }
}
