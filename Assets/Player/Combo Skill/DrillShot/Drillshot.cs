using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Drillshot : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_DRILLSHOT;

    float checkigTimer = 0;

    GameObject whistlingArrow;
    float whistlingArrowAngularAttackSpeed = 1400;

    Vector3[] drillShootVector = { new Vector3(0.4f, 0.2f, 0), new Vector3(-0.4f, 0.2f, 0), new Vector3(0.4f, -0.2f, 0), new Vector3(-0.4f, -0.2f, 0) };

    bool isShooting = false;

    delegate void CurrentLevelShoot();
    CurrentLevelShoot CurrentShoot;

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        CurrentShoot = (skillLevel == 0) ? OneDrillshot : (skillLevel == 5) ? OneWhistlingArrow : OneDrillshot;

        ResetSkill();

        isSettingUpData = true;
    }

    public override void SetToReady(bool _isTimeToReady)
    {
        isSettingUpData = !_isTimeToReady;

        if (_isTimeToReady)
            ResetSkill();

        if (skillLevel == 5)
            whistlingArrow.GetComponent<OneWhistlingArrow>().SetToReady(_isTimeToReady);
    }

    void ResetSkill()
    {
        StopAllCoroutines();

        isShooting = false;

        checkigTimer = 0;

        if (skillLevel != 5)
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.DRILLSHOT_NORMAL);
    }

    private void Update()
    {
        if (isSettingUpData)
        {
            Skill_Fire();

            if (skillLevel == 5)
                UpdateWhistlingArrowInfo();
        }
    }

    void Skill_Fire()
    {
        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        if (!isShooting)
        {
            checkigTimer += Time.deltaTime;

            if (checkigTimer > _attackInterval)
            {
                checkigTimer = 0;

                isShooting = true;

                StartCoroutine(StartShoot());
            }
        }
        else if (isShooting)
        {
            if (skillLevel == 5)
                return;

            float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100);

            checkigTimer += Time.deltaTime;

            if (checkigTimer > _skillDuration)
            {
                checkigTimer = 0;
                isShooting = false;
            }
        }
    }

    IEnumerator StartShoot()
    {
        yield return new WaitForSeconds(0.1f);

        int count = 0;
        while (count < maxBulletCount)
        {
            CurrentShoot();
            count++;
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OneDrillshot()
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100);

        GameObject _drill = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.DRILLSHOT_NORMAL);

        if (_drill)
        {
            _drill.SetActive(true);
            _drill.transform.localScale = new Vector3(_attackRang, _attackRang, 0);
            _drill.GetComponent<OneDrillshot>().UpdateSkillLevel(skillLevel);
            _drill.GetComponent<OneDrillshot>().SetMuzzleTransform(transform);
            _drill.GetComponent<OneDrillshot>().UpdateSkillInfo(_bulletSpeed, _skillDuration, skillAudio);
            _drill.GetComponent<OneDrillshot>().Fire(drillShootVector[Random.Range(0, 4)]);
        }
    }

    void OneWhistlingArrow()
    {
        if (whistlingArrow != null)
            return;

        float _attackRangX = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _attackRangY = (attackRang - 0.35f) + ((attackRang - 0.35f) * buffValue_AttackRang / 100);

        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);
        float _angularAttackSpeed = whistlingArrowAngularAttackSpeed + (whistlingArrowAngularAttackSpeed * buffValue_AttackSpeed / 100);

        skillAudio.volume = 1;
        skillAudio.pitch = 1;

        whistlingArrow = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.DRILLSHOT_SPECIAL);
        if (whistlingArrow)
        {
            whistlingArrow.GetComponent<OneWhistlingArrow>().InitializedData(this, m_cameraMove, skillAudio, PlayerBulletPooler.Instance.transform);
            whistlingArrow.SetActive(true);
            whistlingArrow.transform.localScale = new Vector3(_attackRangX, _attackRangY, 0);
            whistlingArrow.GetComponent<OneWhistlingArrow>().SetPlayState(WhistlingArrowPlayState.Targeting);
            whistlingArrow.GetComponent<OneWhistlingArrow>().UpdateSkillLevel(skillLevel);
            whistlingArrow.GetComponent<OneWhistlingArrow>().UpdateSkillInfo(_bulletSpeed, _angularAttackSpeed);
        }
    }

    void UpdateWhistlingArrowInfo()
    {
        if (!whistlingArrow)
            return;

        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _attackRangY = (attackRang - 0.35f) + ((attackRang - 0.35f) * buffValue_AttackRang / 100);

        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);
        float _angularAttackSpeed = whistlingArrowAngularAttackSpeed + (whistlingArrowAngularAttackSpeed * buffValue_AttackSpeed / 100);

        whistlingArrow.transform.localScale = new Vector3(_attackRang, _attackRangY, 0);
        
        whistlingArrow.GetComponent<OneWhistlingArrow>().UpdateSkillInfo(_bulletSpeed, _angularAttackSpeed);
    }
}