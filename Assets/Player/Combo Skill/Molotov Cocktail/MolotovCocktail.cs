using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovCocktail : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_MOLOTOVCOCKTAIL;

    float checkingTimer = 0f;

    bool isShooting = false;

    int attackEulerZ = 0;

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

        checkingTimer = attackInterval;

        isSettingUpData = true;
    }

    public override void SetToReady(bool _isTimeToReady) { }

    private void Update()
    {
        if (isSettingUpData)
        {
            if (!isShooting)
                Skill_Fire();
            else
                SkillDurationTimer();
        }
    }

    void Skill_Fire()
    {
        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        checkingTimer += Time.deltaTime;

        if (checkingTimer > _attackInterval)
        {
            checkingTimer = 0f;

            StartCoroutine(StartShootMolotovCocktail(45 * attackEulerZ));

            attackEulerZ++;

            if (attackEulerZ == 8)
                attackEulerZ = 0;

            isShooting = true;
        }
    }

    void SkillDurationTimer()
    {
        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100);

        checkingTimer += Time.deltaTime;

        if (checkingTimer > _skillDuration)
        {
            checkingTimer = 0f;
            isShooting = false;
        }
    }

    IEnumerator StartShootMolotovCocktail(float _eulerZ) //EULER
    {
        int bottleCount = 0;
        while (bottleCount < maxBulletCount)
        {
            Quaternion _rotation = Quaternion.Euler(0f, 0f, _eulerZ + (360 / maxBulletCount) * bottleCount);

            Vector3 _direction = _rotation * Vector3.up * 2.2f;

            Vector3 shootTargetVect = playerTransform.transform.position + _direction;

            ShootMolotovCocktail(playerTransform.transform, shootTargetVect); 

            bottleCount++;

            yield return new WaitForSeconds(0.1f);
        }

    }

    void ShootMolotovCocktail(Transform _muzzle, Vector3 _shootTargetVect)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100);

        GameObject _bullet;

        if (skillLevel == 5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.MOLOTOVCOCKTAIL_SPECIAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.MOLOTOVCOCKTAIL_NORMAL);

        if (_bullet != null)
        {
            _bullet.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.SetActive(true);
            _bullet.GetComponent<BottleOfPentagonFlame>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<BottleOfPentagonFlame>().SetMuzzleTransform(_muzzle);
            _bullet.GetComponent<BottleOfPentagonFlame>().UpdateSkillInfo(skillAudio, _shootTargetVect, _skillDuration);
        }
    }
}
