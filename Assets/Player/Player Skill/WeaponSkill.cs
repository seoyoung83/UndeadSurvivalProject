using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class WeaponSkill : MonoBehaviour, IWeapon
{
    protected PlayerMove m_playerMove;

    protected WeaponsMuzzleTransform m_weaponsMuzzleTransform;

    protected AudioSource weaponAudio;

    protected IReadOnlyDictionary<string, JSkillAbilityData> m_SkillAbilityDataDictionary;

    protected int curruntWeaponTypeNumber;

    protected int skillLevel;

    protected bool isShooting = false;
    float checkingTimer = 0;
    protected bool isTimeToReady = false;

    protected float attackInterval;
    protected static float buffValue_AttackInterval = 0;
    protected static float buffValue_SkillDuration = 0;
    protected static float buffValue_AttackSpeed = 0;
    protected static float buffValue_AttackRang = 0;

    public void InitializedData(PlayerMove _playerMove, WeaponsMuzzleTransform _weaponsMuzzleTransform)
    {
        m_playerMove = _playerMove;

        m_weaponsMuzzleTransform = _weaponsMuzzleTransform;

        m_SkillAbilityDataDictionary = DataManager.Instance.SkillAbilityDataDictionary;

        weaponAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isTimeToReady)
            return;

        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        if (!isShooting)
        {
            checkingTimer += Time.deltaTime;

            if (checkingTimer > _attackInterval)
            {
                checkingTimer = 0f;
                isShooting = true;
                Shoot();
            }

            bool activeShootGaugeBar;

            if (curruntWeaponTypeNumber == (int)PlayerSkillCategory.WEAPON_KUNAI)
                activeShootGaugeBar = skillLevel == 5 ? false : true;
            else if (curruntWeaponTypeNumber == (int)PlayerSkillCategory.WEAPON_REVOLVER)
                activeShootGaugeBar = false;
            else
                activeShootGaugeBar = true;

            PlayerUIManager.Instance.UpdateShootGaugeBar(activeShootGaugeBar, checkingTimer, _attackInterval);
        }
    }

    public abstract void SetData();

    public abstract void Shoot();

    public abstract void LevelUp(int _currentWeaponSkillLevel);

    public virtual void SetToReady(bool isTimeToReady)
    {
    }

    public void DoPassiveBuff(PlayerSkillCategory _passivType, float _value)
    {
        switch (_passivType)
        {
            case PlayerSkillCategory.TOP_PASSIVE_EXO_BRACER:  //효과 지속시간 증가
                buffValue_SkillDuration = _value;
                break;
            case PlayerSkillCategory.PASSIVE_ENERGYCUBE: //모든 공격 시간 간격 감소 
                buffValue_AttackInterval = _value;
                break;
            case PlayerSkillCategory.PASSIVE_AMOTHURSTER: //탄약과 무기속도 증가
                buffValue_AttackSpeed = _value;
                break;
            case PlayerSkillCategory.PASSIVE_HE_FUEL:  //모든 탄약 및 무기범위 증가
                buffValue_AttackRang = _value;
                break;
        }
    }

}
