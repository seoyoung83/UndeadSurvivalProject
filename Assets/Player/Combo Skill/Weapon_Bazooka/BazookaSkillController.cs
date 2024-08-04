using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaSkillController : WeaponSkill
{
    PlayerSkillCategory thisControllerType = PlayerSkillCategory.TOP_WEAPON_BAZOOKA;

    [SerializeField] AudioClip shootClip; //shoot 볼륨 04 피치 09
    [SerializeField] AudioClip blackholeDisappearClip;//블랙홀 사라질때 볼륭 02 피치3  msfx_chrono_latency_pitch

    float skillDuration_blackhole;//공격 지속시간
    float skillDuration_shield;//공격 지속시간

    float attackSpeed; //무기 및 탄약 스피드

    float attackRang_blackhole; //무기 및 탄약 범위 
    float attackRang_shield;  //무기 및 탄약 범위 

    private void Start()
    {
        curruntWeaponTypeNumber = (int)thisControllerType;

        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BAZOOKA_BLACKHOLE_BULLET);
        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BAZOOKA_BLACKHOLE);
        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.BAZOOKA_BARRIER);
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        skillLevel = _currentWeaponSkillLevel;

        SetData();
    }

    public override void SetData()
    {
        string index_bullet = curruntWeaponTypeNumber + "a";
        string index_blackhol = curruntWeaponTypeNumber + "b";
        string index_shield = curruntWeaponTypeNumber + "c";

        attackInterval = m_SkillAbilityDataDictionary[index_bullet].attackIntervalOfLevel[skillLevel];

        attackSpeed = m_SkillAbilityDataDictionary[index_bullet].speedOfLevel[skillLevel];

        skillDuration_blackhole = m_SkillAbilityDataDictionary[index_blackhol].skillDurationOfLevel[skillLevel];
        skillDuration_shield = m_SkillAbilityDataDictionary[index_shield].skillDurationOfLevel[skillLevel];

        attackRang_blackhole = m_SkillAbilityDataDictionary[index_blackhol].attackRangOfLevel[skillLevel];
        attackRang_shield = m_SkillAbilityDataDictionary[index_shield].attackRangOfLevel[skillLevel];
    }

    public override void Shoot()
    {
        BlackholeBullet(m_playerMove.JoysticVector);
        isShooting = false;
    } 

    void BlackholeBullet(Vector3 _shootvect)
    {
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BAZOOKA_BLACKHOLE_BULLET);
        if (_bullet)
        {
            weaponAudio.pitch = 0.9f;
            weaponAudio.PlayOneShot(shootClip, 0.3f);

            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<BlackholeBullet>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<BlackholeBullet>().SetMuzzleTransform(m_weaponsMuzzleTransform.transform);
            _bullet.GetComponent<BlackholeBullet>().UpdateSkillInfo(_bulletSpeed);
            _bullet.GetComponent<BlackholeBullet>().Fire(_shootvect);
        }
    }

    public void BlackholeBulletExpired( BlackholeBullet _bullet)
    {
        Blackhole(_bullet.transform.position);

        _bullet.gameObject.SetActive(false);
        _bullet.transform.position = m_weaponsMuzzleTransform.transform.position;
    }

    void Blackhole(Vector3 _spawnVect)
    {
        float _skillDuration = skillDuration_blackhole + (skillDuration_blackhole * buffValue_SkillDuration / 100);

        float _attackRang = attackRang_blackhole + (attackRang_blackhole * buffValue_AttackRang / 100);

        GameObject _blackhole = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BAZOOKA_BLACKHOLE);
        if (_blackhole)
        {
            _blackhole.gameObject.SetActive(true);
            _blackhole.GetComponent<Blackhole>().UpdateSkillLevel(skillLevel);
            _blackhole.GetComponent<Blackhole>().UpdateSkillInfo(_attackRang, _skillDuration);
            _blackhole.GetComponent<Blackhole>().SpawnEffect();
            _blackhole.gameObject.transform.position = _spawnVect;
        }
    }

    public void BlackholeExpired(Blackhole _blackhole)
    {
        weaponAudio.pitch = 2.5f;
        weaponAudio.PlayOneShot(blackholeDisappearClip, 0.05f); 

        if (skillLevel == 5)
        {
            ForceBarrier(_blackhole.transform.position);
        }
        _blackhole.gameObject.SetActive(false);
        _blackhole.transform.position = m_weaponsMuzzleTransform.transform.position;
    }

    void ForceBarrier(Vector3 _spawnVect)
    {
       
        float _skillDuration = skillDuration_shield + (skillDuration_shield * buffValue_SkillDuration / 100);

        float _attackRang = attackRang_shield + (attackRang_shield * buffValue_AttackRang / 100);

        GameObject _barrier = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BAZOOKA_BARRIER);
        if (_barrier)
        {
            _barrier.gameObject.SetActive(true);
            _barrier.gameObject.GetComponent<ForceBarrier>().UpdateSkillInfo(_attackRang, _skillDuration);
            _barrier.gameObject.transform.position = _spawnVect;
        }
    }
}
