using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class RevolverSkillController : WeaponSkill
{
    PlayerSkillCategory thisControllerType = PlayerSkillCategory.WEAPON_REVOLVER;

    PlayerUIManager m_playerUIManager;

    Transform weaponMuzzle;

    float attackSpeed;
    float attackRang;
    int numberOfBulletsFiredOnce; //한번 공격에 발사되는 불렛 수

    int maxRevolverShootCount = 5;
    int loadedBulletCount = 5;//현재 장전되어 있는 탄환

    [Header("For Revolver UI")]
    int bulletPrefabType = 0; //UI불렛 프리펩 종류
    bool isRevolerBulletUILoading = false; //UI 재장전 중

    private void Start()
    {
        curruntWeaponTypeNumber = (int)thisControllerType;

        m_playerUIManager = transform.parent.GetComponentInChildren<PlayerUIManager>();

        m_playerUIManager.GetRevolverSkillController(this);

        PlayerBulletPooler.Instance.PlayerBulletInitialized(PlayerBulletType.REVOLVER_BULLE_NORMAL);
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        skillLevel = _currentWeaponSkillLevel;

        SetData();

        if (skillLevel == 5)
        {
            bulletPrefabType = 1;
            m_playerUIManager.ChangeRevolverBulletUIObject();

            StopCoroutine(ShootRevolver(Vector3.zero));
            loadedBulletCount = maxRevolverShootCount;
        }
    }

    public override void SetData()
    {
        string index = curruntWeaponTypeNumber + "";

        attackInterval = m_SkillAbilityDataDictionary[index].attackIntervalOfLevel[skillLevel];

        attackSpeed = m_SkillAbilityDataDictionary[index].speedOfLevel[skillLevel];

        attackRang = m_SkillAbilityDataDictionary[index].attackRangOfLevel[skillLevel];

        numberOfBulletsFiredOnce = m_SkillAbilityDataDictionary[index].attackCountOfOneTimeOfLevel[skillLevel];
    }

    public override void Shoot()
    {
        StartCoroutine(ShootRevolver(m_playerMove.JoysticVector));
    }

    IEnumerator ShootRevolver(Vector3 _shootVect)
    {
        weaponAudio.Play();

        int count = 0;

        while (count < numberOfBulletsFiredOnce) 
        {
            if (skillLevel != 5)
                weaponMuzzle = m_weaponsMuzzleTransform.transform;
            else if (skillLevel == 5)
            {
                float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

                float radValue = Mathf.Atan2(_shootVect.normalized.y, _shootVect.normalized.x) + 90;

                Vector3 parallelVector= new Vector2(Mathf.Cos(radValue), Mathf.Sin(radValue));

                float distanceBetween = (_attackRang / 20) * ((count * 2) - 1);

                weaponMuzzle.position = m_weaponsMuzzleTransform.transform.position + (parallelVector * distanceBetween);
            }

            Revolver(weaponMuzzle, _shootVect);

            count++;
            yield return null;
        }

        loadedBulletCount--;

        m_playerUIManager.DeleteRevolverBulletIconUI();

        if (loadedBulletCount != 0)
            isShooting = false;
        else if (loadedBulletCount == 0)
            RevolerLoading(true);
    }

    void Revolver(Transform _muzzle, Vector3 _shootVect)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.REVOLVER_BULLE_NORMAL);
        if (_bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.GetComponent<RevolverBullet>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<RevolverBullet>().SetMuzzleTransform(_muzzle);
            _bullet.GetComponent<RevolverBullet>().UpdateSkillInfo(_bulletSpeed);
            _bullet.GetComponent<RevolverBullet>().Fire(_shootVect);
        }
    }

    public void RevolverBulletExpired(bool isHit ,RevolverBullet _bullet)
    {
        _bullet.gameObject.SetActive(false);
        _bullet.transform.position = m_weaponsMuzzleTransform.transform.position;
        
        //적중시 추가
        if (isHit && !isRevolerBulletUILoading)
        {
            if (loadedBulletCount > maxRevolverShootCount)
                return;

            loadedBulletCount++;
            m_playerUIManager.CreateOneRevolverBulletIcon(bulletPrefabType);
        }
    }

    public void RevolerLoading(bool _isloading)
    {
        isRevolerBulletUILoading = _isloading;
     
        if (_isloading)
         {
            loadedBulletCount = maxRevolverShootCount;
            m_playerUIManager.DeleteRevolverBulletIconUI();
        }
        else if (!_isloading)
        {
            isShooting = false;
        }
    }
}
