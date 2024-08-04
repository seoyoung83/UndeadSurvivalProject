using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : ActiveComboSkill
{
    PlayerSkillCategory thisSkillType = PlayerSkillCategory.ACTIVE_BRICK;

    bool isAttacking = false;

    float checkingTimer = 0f;

    private void Awake()
    {
        curruntActiveSkillTypeNumber = (int)thisSkillType;
    }

    public override void LevelUp(int _currentWeaponSkillLevel)
    {
        isSettingUpData = false;

        isAttacking = false;

        skillLevel = _currentWeaponSkillLevel;

        SetData((int)thisSkillType, skillLevel);

        //Reset
        ResetSkill();

        isSettingUpData = true;
    }

    void ResetSkill()
    {
        StopAllCoroutines();

        checkingTimer = attackInterval;
    }

    public override void SetToReady(bool _isTimeToReady)
    {

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

        checkingTimer += Time.deltaTime;

        if (checkingTimer > _attackInterval)
        {
            checkingTimer = 0;

            isAttacking = true;

            StartCoroutine(ShootBrick(skillLevel));
        }
    }
    IEnumerator ShootBrick(int _skillLevel)
    {
        if (_skillLevel != 5)
        {
            int count = 0;

            while (count < maxBulletCount)
            {
                Vector3[] direct = { Vector3.up + new Vector3(-0.3f ,0, 0), Vector3.up + new Vector3(-0.2f, 0, 0) , Vector3.up + new Vector3(-0.1f,0, 0), Vector3.up,
                Vector3.up + new Vector3(0.1f ,0, 0),Vector3.up + new Vector3(0.2f,0, 0),Vector3.up + new Vector3(0.3f,0, 0)};

                ShootBrick(direct[Random.Range(0, direct.Length)]); 
                count++;
                yield return new WaitForSeconds(0.4f);
            }
        }
        else if (_skillLevel == 5)
        {
            int dumbbellcount = 0;
            while(dumbbellcount< maxBulletCount)
            {
                Quaternion _rotation = Quaternion.Euler(0f, 0f, (360 / maxBulletCount) * dumbbellcount);

                Vector3 _direction = _rotation * Vector3.up;

                Vector3 _position = playerTransform.position + _direction;

                ShootBrick(_position);

                dumbbellcount++;

                yield return null;
            }
        }

        isAttacking = false;
    }

    void ShootBrick(Vector3 _direct)
    {
        GameObject _brick;

        float _attackRang = attackRang + (attackRang * buffValue_AttackRang / 100);
        float _bulletSpeed = attackSpeed + (attackSpeed * buffValue_AttackSpeed / 100);

        float _speed;

        if (skillLevel != 5)
        {
            _brick = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BRICK_NORMAL);
            _speed = Random.Range(_bulletSpeed - 50, _bulletSpeed + 50); 
        }
        else
        {
            _brick = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.BRICK_SPECIAL);
            _speed = _bulletSpeed; 
        }

        if (_brick)
        {
            skillAudio.Play();
            _brick.gameObject.SetActive(true);
            _brick.gameObject.transform.localScale = new Vector3(_attackRang, _attackRang, 0);
            _brick.GetComponent<OneBrick>().UpdateSkillInfo(_speed);
            _brick.GetComponent<OneBrick>().SetMuzzleTransform(playerTransform);
            _brick.GetComponent<OneBrick>().UpdateSkillLevel(skillLevel);
            _brick.GetComponent<OneBrick>().Fire(_direct);
        }
    }
}
