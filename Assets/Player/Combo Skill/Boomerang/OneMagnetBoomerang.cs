using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneMagnetBoomerang : MonoBehaviour, IPlayerBullet
{
    int skillLevel;

    Vector3 axisOfRotation;

    float moveSpeed;
    float radiusSpeed = 0.6f;
    
    float radius = 0;
    float angle = 0;

    private void Update()
    {
        if (radius > 4f)
        {
            gameObject.SetActive(false);
            return;
        }

        if (axisOfRotation != null)
            Fire(axisOfRotation);
    }

    public void Fire(Vector3 shootVect)
    {
        radius += Time.deltaTime * radiusSpeed;
        angle += Time.deltaTime * moveSpeed;

        transform.position = axisOfRotation + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        Vector3 directVect = (transform.position - axisOfRotation).normalized;
        float radValue = Mathf.Atan2(directVect.y, directVect.x);
        float shootAngle = radValue * (180 / Mathf.PI);
        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _moveSpeed)
    {
        angle = 0;
        radius = 0;

        moveSpeed = _moveSpeed;
    }

    public void SetMuzzleTransform(Transform _reloadTransform)
    {
        if (_reloadTransform != null)
        {
            transform.position = _reloadTransform.position;
            axisOfRotation = _reloadTransform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BOOMERANG);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageBoomerang>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BOOMERANG);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageBoomerang>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BOOMERANG);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageBoomerang>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("PickupBox"))
        {
            PickupBox _box = collision.gameObject.GetComponent<PickupBox>();
            if (_box)
            {
                _box.GetComponent<PickupBox>().SetBoxState(false);
            }
        }
    }
}
