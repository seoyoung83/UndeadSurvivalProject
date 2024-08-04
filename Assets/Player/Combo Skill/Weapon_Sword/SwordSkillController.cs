using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SwordSkillController : WeaponSkill
{
    PlayerSkillCategory thisControllerType = PlayerSkillCategory.WEAPON_SWORD;

    float skillDuration_mainBlade;
    float skillDuration_sideBlade;

    float attackSpeed;
    float attackRang;

    int mainBladeCount; //한번 공격에 발사되는 검기 수
    int sideBladeCount; //한번 공격에 발사되는 기동도 수

    private void Start()
    {
        curruntWeaponTypeNumber = (int)thisControllerType;

        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SWORD_MAINBLADE_NORMAL);
        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SWORD_SIDEBLADE_NORMAL);

        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SWORD_MAINBLADE_SPECIAL);
        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.SWORD_SIDEBLADE_SPECIAL);
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        skillLevel = _currentWeaponSkillLevel;

        SetData();
    }

    public override void SetData()
    {
        string indexA = curruntWeaponTypeNumber + "a";
        string indexB = curruntWeaponTypeNumber + "b";

        attackInterval = m_SkillAbilityDataDictionary[indexA].attackIntervalOfLevel[skillLevel];

        attackSpeed = m_SkillAbilityDataDictionary[indexA].speedOfLevel[skillLevel];

        attackRang = m_SkillAbilityDataDictionary[indexA].attackRangOfLevel[skillLevel];

        skillDuration_mainBlade = m_SkillAbilityDataDictionary[indexA].skillDurationOfLevel[skillLevel];
        skillDuration_sideBlade = m_SkillAbilityDataDictionary[indexB].skillDurationOfLevel[skillLevel];

        mainBladeCount = m_SkillAbilityDataDictionary[indexA].attackCountOfOneTimeOfLevel[skillLevel];
        sideBladeCount = m_SkillAbilityDataDictionary[indexB].attackCountOfOneTimeOfLevel[skillLevel];
    }

    public override void Shoot()
    {
        StartCoroutine(ShootSwordBlade(mainBladeCount, m_playerMove.JoysticVector));
    }

    IEnumerator ShootSwordBlade(int _mainBlade, Vector3 _playerDrect)
    {
        weaponAudio.Play();

        if (_playerDrect == Vector3.zero)
        {
            _playerDrect = Vector3.left;
        }
      
        int count = 0;
        int sideBradeCount = 0;

        while (count < _mainBlade) 
        {
            int opp = count == 1 ? -1 : 1;
           
            if (sideBladeCount > 0)
            {
                if (sideBradeCount < sideBladeCount)
                {
                    ShootSideBlade(_playerDrect * opp);
                    sideBradeCount++;
                    yield return new WaitForSeconds(0.15f);
                }
            }

            ShootMainBlade(_playerDrect * opp);
            count++;
            yield return new WaitForSeconds(0.17f);
        }
        isShooting = false;
    }

    void ShootMainBlade(Vector3 _movetoward)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);
     
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        float _skillDuration = skillDuration_mainBlade + (skillDuration_mainBlade * buffValue_SkillDuration / 100);

        GameObject _bullet;

        if (skillLevel != 5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SWORD_MAINBLADE_NORMAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SWORD_MAINBLADE_SPECIAL);

        if (_bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.GetComponent<SwordBlade>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<SwordBlade>().SetMuzzleTransform(m_weaponsMuzzleTransform.transform);
            _bullet.GetComponent<SwordBlade>().UpdateSkillInfo(_bulletSpeed, _skillDuration);
            _bullet.GetComponent<SwordBlade>().Fire(_movetoward);
        }
    }

    void ShootSideBlade(Vector3 _movetoward)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        float _skillDuration = skillDuration_sideBlade + (skillDuration_sideBlade * buffValue_SkillDuration / 100);

        GameObject _bullet;

        if (skillLevel != 5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SWORD_SIDEBLADE_NORMAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SWORD_SIDEBLADE_SPECIAL);

        if (_bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.GetComponent<SwordBlade>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<SwordBlade>().SetMuzzleTransform(m_weaponsMuzzleTransform.transform);
            _bullet.GetComponent<SwordBlade>().UpdateSkillInfo(_bulletSpeed, _skillDuration);
            _bullet.GetComponent<SwordBlade>().Fire(_movetoward);
        }
    }
}
