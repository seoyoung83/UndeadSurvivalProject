using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.TOP_ACTIVE_BOOMERANG;

    [SerializeField] AudioClip[] shootClip;

    float eachAttackInterval;//하나의 공격당

    int currentLoadedBoomerangCount; //현재 장전된 부메랑 수( 5 Lev 사용X) 

    bool isAttacking = false;

    float checkingTimer = 0f;

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;
    }
 
    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        //Reset
        ResetSkill();

        isSettingUpData = true;
    }

    public override void SetToReady(bool _isTimeToReady)
    {
        isSettingUpData = !_isTimeToReady;

        if (_isTimeToReady)
        {
            if (skillLevel != 5)
                ResetSkill();
        }
    }

    void ResetSkill()
    {
        StopAllCoroutines();

        if (skillLevel > 0 && skillLevel < 5)
            PlayerBulletPooler.Instance.ExpiredPlayerBullet(PlayerBulletType.BOOMERANG_NORMAL);

        isAttacking = false;

        checkingTimer = attackInterval;

        eachAttackInterval = skillLevel != 5 ? 0.3f : 0.6f;//위치

        currentLoadedBoomerangCount = maxBulletCount;
    }

    private void Update()
    {
        if (isSettingUpData)
        {
            if (isAttacking)
                return;

            Skill_Fire();
        }
    }

    void Skill_Fire()
    {
        float _attackInterval = attackInterval - (attackInterval * buffValue_AttackInterval / 100);

        if (skillLevel != 5 && currentLoadedBoomerangCount > 0)
        {
            checkingTimer += Time.deltaTime;

            if (checkingTimer > _attackInterval)
            {
                checkingTimer = 0;

                StartCoroutine(StartShootingBoomerang(false, currentLoadedBoomerangCount));
            }
        }
        else if (skillLevel == 5)
        {
            checkingTimer += Time.deltaTime;
            if (checkingTimer > _attackInterval)
            {
                checkingTimer = 0;

                StartCoroutine(StartShootingBoomerang(false, maxBulletCount));
            }
        }
    }

    IEnumerator StartShootingBoomerang(bool waiteSeconds ,int _currentLoadedBoomerangCount)
    {
        isAttacking = true;

        if (waiteSeconds)
            yield return new WaitForSeconds(attackInterval * 0.05f);

        if(skillLevel == 5)
        {
            skillAudio.volume = 0.2f;
            skillAudio.pitch = 2f;
            skillAudio.PlayOneShot(shootClip[1]);
        }

        int count = 0;
        int maxCount = _currentLoadedBoomerangCount;
        while (count < maxCount)
        {
            if (skillLevel != 5)
            {
                Transform nearestEnemyTransform = GetNearestEnemyPosition();

                if (nearestEnemyTransform != null)
                {
                    Vector3 _direct = (nearestEnemyTransform.position - playerTransform.position).normalized;
                    ShootOneBoomerang(_direct, playerTransform);
                    currentLoadedBoomerangCount--;
                }
            }
            else if (skillLevel == 5)
            {
                ShootOneBoomerang(Vector3.zero, playerTransform);
            }
           
            count++;
            yield return new WaitForSeconds(eachAttackInterval);
        }

        isAttacking = false;
    }

    void ShootOneBoomerang(Vector3 _direct, Transform _playerTransform)
    {
        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100); 
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        if (skillLevel != 5)
        {
            GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BOOMERANG_NORMAL);
            if (_bullet)
            {
                skillAudio.PlayOneShot(shootClip[0]);
                _bullet.gameObject.SetActive(true);
                _bullet.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, 0);
                _bullet.GetComponent<OneBoomerang>().UpdateSkillLevel(skillLevel);
                _bullet.GetComponent<OneBoomerang>().UpdateSkillInfo(_bulletSpeed);
                _bullet.GetComponent<OneBoomerang>().SetMuzzleTransform(_playerTransform);
                _bullet.GetComponent<OneBoomerang>().Fire(_direct);
            }  
        }
        else if (skillLevel == 5)
        {
            GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BOOMERANG_SPECIAL);
            if (_bullet)
            {
                _bullet.gameObject.SetActive(true);
                _bullet.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, 0);
                _bullet.GetComponent<OneMagnetBoomerang>().UpdateSkillLevel(skillLevel);
                _bullet.GetComponent<OneMagnetBoomerang>().UpdateSkillInfo(_bulletSpeed);
                _bullet.GetComponent<OneMagnetBoomerang>().SetMuzzleTransform(_playerTransform);
            }
        }
    }

    public void ExpiredBoomerang(OneBoomerang _boomerang, bool isCatched)
    {
        _boomerang.gameObject.SetActive(false);
        _boomerang.transform.position = playerTransform.position;

        currentLoadedBoomerangCount++;

        if (skillLevel != 5 && isCatched && !isAttacking)
            StartCoroutine(StartShootingBoomerang(true, currentLoadedBoomerangCount));
    }

    Transform  GetNearestEnemyPosition()
    {
        Transform tempNearestEnemyTransform = null;
        //Detectting Enemy In Attackable Area
        Collider2D[] enemyInAttackableArea =
            Physics2D.OverlapCircleAll(new Vector2(playerTransform.position.x, playerTransform.position.y), 6f, LayerMask.GetMask("Enemy"));

        if (enemyInAttackableArea.Length > 0)
        {
            float shortDistance = Mathf.Infinity;

            foreach (Collider2D enemyCollider in enemyInAttackableArea)
            {
                float distanceFromEnemy = (enemyCollider.gameObject.transform.position - playerTransform.position).magnitude;

                if (shortDistance > distanceFromEnemy)
                {
                    shortDistance = distanceFromEnemy;
                    tempNearestEnemyTransform = enemyCollider.transform;
                }
            }
        }
        return tempNearestEnemyTransform;
    }
}
