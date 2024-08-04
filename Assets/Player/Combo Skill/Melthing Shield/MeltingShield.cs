using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingShield : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_MELTHINGSHIELD;

    float checkingTimer = 0;
    float checkingTimer_audio = 0;
    float audioIPlaylTime = 1.5f;

    int rotate = 0;

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        ResetSkill();

        isSettingUpData = true;
    }

    void ResetSkill()
    {   
        if (skillLevel > 0 && skillLevel < 5)
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.MELTHINGSHIELD_NORMAL);

        checkingTimer_audio = 0;
        checkingTimer = attackInterval - (attackInterval * buffValue_AttackInterval / 100) - 1f;
    }

    public override void SetToReady(bool _isTimeToReady) { }

    private void Update()
    {
        if (!isSettingUpData)
            return;

        checkingTimer += Time.deltaTime;

        if (checkingTimer > attackInterval)
        {
            checkingTimer = 0;

            SpawnShield();

            rotate = rotate == 0 ? 1 : 0;
        }

        checkingTimer_audio += Time.deltaTime;
        if (checkingTimer_audio > audioIPlaylTime)
        {
            checkingTimer_audio = 0;
            skillAudio.Play();
        }
    }

    void SpawnShield()
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        GameObject _shield;

        if (skillLevel == 5)
            _shield = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.MELTHINGSHIELD_SPECIAL);
        else
            _shield = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.MELTHINGSHIELD_NORMAL);

        if (_shield != null)
        {
            _shield.SetActive(true);
            _shield.transform.position = transform.position;
            _shield.GetComponent<OneOfMeltingShield>().UpdateSkillLevel(skillLevel);
            _shield.GetComponent<OneOfMeltingShield>().Fire(new Vector3(0, 0, 25 * rotate));
            _shield.GetComponent<OneOfMeltingShield>().SetMuzzleTransform(playerTransform);
            _shield.GetComponent<OneOfMeltingShield>().UpdateSkillInfo(_attackRang, attackSpeed);
        }
    }
}
