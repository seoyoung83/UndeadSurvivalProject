using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drone : ActiveComboSkill
{
    protected PlayerSkillCategory thisSkillType;
    protected SkillType activeSkillType;
    protected PlayerBulletType bulletType;

    [Header("About Drone Body")]
    [SerializeField] Transform[] droneTrans; //Drone Transform [0]: ��0-4 / [1]: �� 5
    Vector3 initialDroneVect; //�ʱ� ���� ��ġ
    bool moveUp = true;
    float checkingTimer_droneMove = 0f;

    [Header("About Reload")]
    float checkingTimer_reload = 0f;
    bool isCalculating = false; //isCalculation

    [Header("About Calculation Rocket Launch Vect")]
    const float circleRadius = 2.45f; // ���� ��Ŭ ������
    const int angleOfAttack = 24; // 25�� �� �ѹ� ��
    float checkingTimer_launch = 0;
    float checkingTimer_currentRotaionAngle;
    float nextAttackRangeAngle = 90f;

    [Header("About Launch")]
    int numberOfRocketsFiredOnce;// �ѹ��� ���� �߻�Ǵ� ���� ��

    Vector3 launchVect; // ���� Ÿ���� ��ġ(����)
    Vector3 launchVectReverse; // ���� Ÿ���� ��ġ(�ݴ���)

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

        numberOfRocketsFiredOnce = maxBulletCount / 15; //(���� �ʱ� ���� ��; skillDuration ���� �� �� ���Ϸ� ����) / �� Ƚ��
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

            // ������ źȯ(RocketTargeting Vect(�� ������) Enqueue > 0)�� ������, ReloadDrone���¶� shoot
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

    void CalculationRocketTargetingVect() // Targeting Vect(�ݿ���) ��� ��, Targeting Vect Enqueue
    {
        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100); // ���� ���� ����
        float _attackSpeed = attackSpeed - (attackSpeed * buffValue_AttackSpeed / 100); // ���ݽ��ǵ� = attackableAngle �� ���µ� �ɸ��� �ð� (���� = �ӵ� ���)
        float rotationSpeed = _skillDuration / _attackSpeed;   //  (ex 360 / 6.5f) (ex 180 / 4f)

        checkingTimer_currentRotaionAngle += Time.deltaTime * rotationSpeed;

        int shootCountBySkillDuration = (int)Mathf.Round(_skillDuration / angleOfAttack); //�ѹ��� ��ų ȸ�������� �� ī��Ʈ

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

    //��� ��ü move
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

    IEnumerator StartRocketsFiredOnce() //RocketTargeting ��ġ(�� ������) Enqueue & RocketTargeting Spawn
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

    void DroneRocketShoot(Vector3 _shootVect) //Damage�� ���� RocketExplosion�� DroneRocket ��ũ��Ʈ ����.
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

    void DroneRocketTargetingSpawn(Vector3 _shootVect) // �߻� �� Ÿ���� ǥ��
    {
        GameObject _targetingFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DRONE_ROCKET_TARGETING);
        if (_targetingFx != null)
        {
            _targetingFx.transform.position = transform.position + _shootVect;
            _targetingFx.SetActive(true);
        }
    }
}
