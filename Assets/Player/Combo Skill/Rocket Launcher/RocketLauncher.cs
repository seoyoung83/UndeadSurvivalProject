using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_ROCKETLAUNCHER;

    Transform nearestEnemyTrans;

    float checkingTimer = 0f;

    bool isShooting = false;

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;

        checkingTimer = attackInterval;
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

        isShooting = false;

        checkingTimer = attackInterval;
    }

    private void Update()
    {
        if (isSettingUpData)
        {
            nearestEnemyTrans = GetNearestEnemyTransform();

            if (!isShooting)
                RelodSkill();
        }
    }

    void RelodSkill()
    {
        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        checkingTimer += Time.deltaTime;

        if (checkingTimer > _attackInterval)
        {
            checkingTimer = 0;
            
            isShooting = true;

            if (nearestEnemyTrans != null)
                StartCoroutine(StartShoot());
            else
                isShooting = false;
        }
    }

    IEnumerator StartShoot()
    {
        int count = 0;
        while (count < maxBulletCount)
        {
            if (nearestEnemyTrans != null)
                ShootRocket(nearestEnemyTrans.position);

            count++;
            yield return new WaitForSeconds(0.5f);
        }

        isShooting = false;
    }

    void ShootRocket(Vector3 _shootVect)
    {
        GameObject _bullet;

        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _attackSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        if (skillLevel == 5)
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.ROCKETLAUNCHER_SPECIAL);
        else
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.ROCKETLAUNCHER_NORMAL);

        if (_bullet != null)
        {
            _bullet.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<OneRocket>().UpdateSkillLevel(skillLevel);
            _bullet.GetComponent<OneRocket>().UpdateSkillInfo(skillAudio, _attackSpeed);
            _bullet.GetComponent<OneRocket>().SetMuzzleTransform(playerTransform);
            _bullet.GetComponent<OneRocket>().Fire(_shootVect);
        }
    }

    Transform GetNearestEnemyTransform()
    {
        //Detectting Enemy In Attackable Area
        Collider2D[] enemyInAttackableArea =
            Physics2D.OverlapCircleAll(new Vector2(playerTransform.position.x, playerTransform.position.y), 6.5f, LayerMask.GetMask("Enemy"));

        if (enemyInAttackableArea.Length > 0)
        {
            float shortDistance = Mathf.Infinity;

            foreach (Collider2D enemyCollider in enemyInAttackableArea)
            {
                float distanceFromEnemy = (enemyCollider.gameObject.transform.position - playerTransform.position).magnitude;

                if (shortDistance > distanceFromEnemy)
                {
                    shortDistance = distanceFromEnemy;

                    return enemyCollider.transform;
                }
            }
        }

        return null;
    }

}
