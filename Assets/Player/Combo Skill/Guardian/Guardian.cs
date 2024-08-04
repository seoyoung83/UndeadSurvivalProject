using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_GARDIAN;

    int expiredBullet;

    const float distanceFromPlayer = 1.8f;

    float checkingTimer = 0;
    float checkingCurrentRotaionAngle = 0f;

    bool isShooting = false;

    List<Transform> guardianTransformList = new List<Transform>();

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;

        isShooting = false;

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        ResetSkill();

        isSettingUpData = true;
    }

    void ResetSkill()
    {
        if (skillLevel > 0 && skillLevel < 5)
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.GARDIAN_NORMAL);

        checkingTimer = attackInterval - (attackInterval * buffValue_AttackInterval / 100) - 1f;
        expiredBullet = maxBulletCount;
        checkingCurrentRotaionAngle = 0f;
    }

    public override void SetToReady(bool _isTimeToReady) { }

    private void Update()
    {
        if (isSettingUpData)
        {
            transform.position = playerTransform.position;

            if (!isShooting)
                ReloadGuardian();
            else if (isShooting)
            {
                UpdatBulletInfo();
                EulerGuardian();
            }

        }
    }

    void Shoot()
    {
        isShooting = true;

        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);
        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100);
        bool _isNoLimitSkillDuration = skillLevel == 5 ? true : false;

        GameObject _bullet;

        if (skillLevel == 5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.GARDIAN_SPECIAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.GARDIAN_NORMAL);

        if (_bullet != null)
        {
            _bullet.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.SetActive(true);
            _bullet.GetComponent<OneGuardian>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<OneGuardian>().UpdateSkillInfo(skillAudio, _isNoLimitSkillDuration, _skillDuration);

            guardianTransformList.Add(_bullet.transform);

            expiredBullet--;
        }
    }

    void ReloadGuardian()
    {
        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        checkingTimer += Time.deltaTime;

        if (checkingTimer > _attackInterval)
        {
            checkingTimer = 0;

            checkingCurrentRotaionAngle = 0f;

            guardianTransformList = new List<Transform>();

            for (int i = 0; i < maxBulletCount; ++i)
                Shoot();
        }
    }

    void EulerGuardian()
    {
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        checkingCurrentRotaionAngle += Time.deltaTime * _bulletSpeed;

        for (int i = 0; i < guardianTransformList.Count; ++i)
        {
            if (guardianTransformList[i].gameObject.activeInHierarchy)
            {
                float angle = i * (360 / guardianTransformList.Count);

                Vector3 fromAngleToVect = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

                Quaternion rotation = Quaternion.Euler(0, 0, checkingCurrentRotaionAngle);

                Vector3 guardianVect = fromAngleToVect * distanceFromPlayer;

                guardianTransformList[i].transform.position = transform.position + rotation * guardianVect;
            }
        }
    }

    public void GuardianExpired()
    {
        expiredBullet++;

        if(expiredBullet > maxBulletCount - 1)
        {
            isShooting = false;
            transform.localRotation = Quaternion.identity;
        }
    }

    void UpdatBulletInfo()
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        for (int i = 0; i < guardianTransformList.Count; ++i)
        {
            if (guardianTransformList[i].gameObject.activeInHierarchy)
            {
                guardianTransformList[i].transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            }
        }
    }
}

