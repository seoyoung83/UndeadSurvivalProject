using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SwordBlade : MonoBehaviour, IPlayerBullet
{
    Rigidbody2D rigid;
    Transform reloadTransform;

    [SerializeField] int bladeType;
    SkillType skillType;

    int skillLevel;

    float moveFactor;
    float skillDuration;
    float expiredTimer = 0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        skillType = bladeType == 0 ? SkillType.SKILL_SWORD_BLADE : SkillType.SKILL_SWORD_BLADEWIND;
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _moveFactor, float _skillDuration)
    {
        moveFactor = _moveFactor;

        skillDuration = _skillDuration;
    }

    private void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > skillDuration)
        {
           gameObject.SetActive(false);
        }
    }

    public void Fire(Vector3 _shootVect)
    {
        Reload();

        float radValue = Mathf.Atan2(_shootVect.normalized.y, _shootVect.normalized.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);

        rigid.AddForce(_shootVect.normalized * moveFactor, ForceMode2D.Force);
    }

    void Reload()
    {
        rigid.gravityScale = 0f;
        rigid.angularVelocity = 0;
        rigid.velocity = Vector2.zero;

        if (reloadTransform)
        {
            transform.position = reloadTransform.position;
            transform.eulerAngles = reloadTransform.eulerAngles;
        }
        expiredTimer = 0f;
    }

    public void SetMuzzleTransform(Transform _reloadTransform)
    {
        reloadTransform = _reloadTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _bladeDamage = SkillDamagePooler.Instance.GetPlayerSkill(skillType);

                if (_bladeDamage != null)
                {
                    _bladeDamage.transform.position = collision.transform.position;
                    _bladeDamage.GetComponent<DamageSword>().GetSkillLevel(skillLevel);
                    _bladeDamage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat)
            {
                GameObject _bladeDamage = SkillDamagePooler.Instance.GetPlayerSkill(skillType);

                if (_bladeDamage != null)
                {
                    _bladeDamage.transform.position = collision.transform.position;
                    _bladeDamage.GetComponent<DamageSword>().GetSkillLevel(skillLevel);
                    _bladeDamage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _bladeDamage = SkillDamagePooler.Instance.GetPlayerSkill(skillType);

                if (_bladeDamage != null)
                {
                    _bladeDamage.transform.position = collision.transform.position;
                    _bladeDamage.GetComponent<DamageSword>().GetSkillLevel(skillLevel);
                    _bladeDamage.SetActive(true);
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

        if (collision.gameObject.CompareTag("DestructibleEnemyBullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
