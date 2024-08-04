using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_SOCCERBALL;

    Transform nearestEnemyTransform;

    float checkingTimer = 0;

    bool isShooting = false;

    bool isThereLoadedBullet = true;

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
        StopAllCoroutines();

        checkingTimer = 0;

        isShooting = false;
        isThereLoadedBullet = true;

        if (skillLevel != 5)
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.SOCCERBALL_NORMAL);
    }

    public override void SetToReady(bool _isTimeToReady)
    {
        isSettingUpData = !_isTimeToReady;

        if (_isTimeToReady)
        {
            StopAllCoroutines();

            checkingTimer = attackInterval;
            isShooting = false;
            isThereLoadedBullet = true;

            if (skillLevel != 5)
                PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.SOCCERBALL_NORMAL);
            else
            {
                PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.SOCCERBALL_SPECIAL);
                PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.SOCCERBALL_SPECIAL_BULLET);
            }
        }
    }

    private void Update()
    {
        if (isSettingUpData)
        {
            if (!isShooting && isThereLoadedBullet)
                ReloadSkill();
            else if (isShooting && !isThereLoadedBullet)
                SkillDurationTimer();
        }
    }

    void ReloadSkill()
    {
        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        checkingTimer += Time.deltaTime;

        if (checkingTimer > _attackInterval)
        {
            checkingTimer = 0;

            nearestEnemyTransform = GetNearestEnemyTransform();

            if (nearestEnemyTransform != null)
                StartCoroutine(StartShoot());
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
            isThereLoadedBullet = true;
        }
    }

    IEnumerator StartShoot()
    {
        isShooting = true;

        int count = 0;
        while (count < maxBulletCount)
        {
            if(nearestEnemyTransform.position != null)
                ShootBall(nearestEnemyTransform.position);
            else
                yield return null;

            count++;
            yield return new WaitForSeconds(0.3f);
        }

        isThereLoadedBullet = false;

    }

    void ShootBall(Vector3 _direct)
    {
        GameObject _bullet;

        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);

        float _attackSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        float _skillDuration = skillDuration + (skillDuration * buffValue_SkillDuration / 100);

        if (skillLevel != 5)
        {
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SOCCERBALL_NORMAL);
            if (_bullet != null)
            {
                skillAudio.volume = 0.6f;
                skillAudio.pitch = 1f;
                _bullet.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
                _bullet.gameObject.SetActive(true);
                _bullet.GetComponent<OneSoccerBall>().UpdateSkillLevel(skillLevel);
                _bullet.GetComponent<OneSoccerBall>().UpdateSkillInfo(skillAudio, _attackSpeed, _skillDuration);
                _bullet.GetComponent<OneSoccerBall>().SetMuzzleTransform(playerTransform);
                _bullet.GetComponent<OneSoccerBall>().Fire(_direct);
            }
        }
        else if (skillLevel == 5)
        {
            _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SOCCERBALL_SPECIAL);
            if (_bullet != null)
            {
                skillAudio.volume = 0.15f;
                skillAudio.pitch = 0.4f;
                _bullet.transform.localScale = new Vector3(_attackRang, _attackRang, _attackRang);
                _bullet.gameObject.SetActive(true);
                _bullet.GetComponent<OneQuantumBallReleaseType>().UpdateSkillInfo(skillAudio, _attackSpeed, _skillDuration);
                _bullet.GetComponent<OneQuantumBallReleaseType>().SetMuzzleTransform(playerTransform);
                _bullet.GetComponent<OneQuantumBallReleaseType>().Fire(_direct);
            }
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

            Transform nearestEnemy = null;

            foreach (Collider2D enemyCollider in enemyInAttackableArea)
            {
                float distanceFromEnemy = (enemyCollider.gameObject.transform.position - playerTransform.position).magnitude;

                if (shortDistance > distanceFromEnemy)
                {
                    shortDistance = distanceFromEnemy;
                    nearestEnemy = enemyCollider.transform;
                }
            }

            return nearestEnemy;
        }
        else
            return null;
    }
}
