using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEmitter : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_LIGHTNINGEMITTER;

    int expiredBullet;

    float checkingTimer = 0;

    bool isAttacking = false;

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
    public override void SetToReady(bool _isTimeToReady)
    {
        isSettingUpData = !_isTimeToReady;

        if (_isTimeToReady)
            ResetSkill();
    }

    void ResetSkill()
    {
        StopAllCoroutines();

        isAttacking = false;

        if (skillLevel == 5)
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.LIGHTNINGEMITTER_SPECIAL);
        else
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.LIGHTNINGEMITTER_NORMAL);

        checkingTimer = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        expiredBullet = maxBulletCount;
    }

    void Update()
    {
        if (isSettingUpData)
        {
            if (isAttacking)
                return;

            float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

            checkingTimer += Time.deltaTime;
            if (checkingTimer > _attackInterval)
            {
                checkingTimer = 0f;
                isAttacking = true;
                StartCoroutine(StartShootLightning());
            }
        }
    }

    IEnumerator StartShootLightning()
    {
        int count = 0;

        while (count < maxBulletCount)
        {
            Transform spawnTransform = GetEnemyTransformRandomly();
            
            if(spawnTransform!=null)
                Shoot(spawnTransform);

            count++;

            yield return new WaitForSeconds(0.2f);
        }

        isAttacking = false;
    }

    void Shoot(Transform _spawnTransfrom)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100); 

        GameObject _bullet;

        if (skillLevel == 5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.LIGHTNINGEMITTER_SPECIAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.LIGHTNINGEMITTER_NORMAL);

        if (_bullet != null)
        {
            skillAudio.Play();
            _bullet.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.SetActive(true);
            _bullet.transform.position = _spawnTransfrom.position;
            _bullet.GetComponent<OneLightning>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<OneLightning>().UpdateSkillInfo(_skillDuration);

            expiredBullet--;
        }
    }

    public void LightningExpired(GameObject _bullet)
    {
        _bullet.SetActive(false);
        _bullet.transform.position = transform.position;

        expiredBullet++;

        if (expiredBullet > maxBulletCount - 1)
            isAttacking = false;
    }

    Transform GetEnemyTransformRandomly( )
    {
        float cameraSize = m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize;
        Vector3 attackableArea = new Vector3(cameraSize + 0.4f, (cameraSize * 1.8f) + 0.4f, 0);

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D detectEnemyArea = Physics2D.OverlapBox(playerTransform.position, attackableArea, 0f, enemyLayer);

        if (detectEnemyArea != null && detectEnemyArea.CompareTag("Enemy"))
            return detectEnemyArea.transform;
        else if(detectEnemyArea != null && detectEnemyArea.CompareTag("Boss"))
            return detectEnemyArea.transform;

        return null;
    }
}
