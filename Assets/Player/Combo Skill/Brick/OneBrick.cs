using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneBrick : MonoBehaviour, IPlayerBullet
{
    Transform reloadTransform;

    Rigidbody2D rigid;

    Vector2 forwardNormal;

    int skillLevel;

    float moveFactor;

    float checkingTimer = 0f;

    [SerializeField] float attackTime;

    bool IsPushEnemy = false;
    float pushForce = 1900f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        checkingTimer += Time.deltaTime;

        if (checkingTimer > attackTime)
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _moveFacor)
    {
        moveFactor = _moveFacor;
    }

    public void Fire(Vector3 _shootVect)
    {
        Reload();

        if (skillLevel != 5)
            forwardNormal = _shootVect.normalized;
        else if (skillLevel == 5)
            forwardNormal = (_shootVect - transform.position).normalized;

        float radValue = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        rigid.AddForce(forwardNormal * moveFactor, ForceMode2D.Force);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
    }

    void Reload()
    {
        if (reloadTransform)
        {
            rigid.velocity = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.position = reloadTransform.position;
            IsPushEnemy = false;
        }

        checkingTimer = 0;
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
                if (skillLevel != 5)
                {
                    if (!IsPushEnemy)
                    {
                        Rigidbody2D _enemyRigid = collision.gameObject.GetComponent<Rigidbody2D>();
                        Vector3 reflectiveVect = (collision.transform.position - transform.position).normalized;

                        _enemyRigid.AddForce(reflectiveVect * pushForce, ForceMode2D.Force);

                        IsPushEnemy = true;
                    }
                }
                else if (skillLevel == 5)
                {
                    Rigidbody2D _enemyRigid = collision.gameObject.GetComponent<Rigidbody2D>();
                    Vector3 reflectiveVect = (collision.transform.position - transform.position).normalized;

                    _enemyRigid.AddForce(reflectiveVect * pushForce, ForceMode2D.Force);
                }
               

                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BRICK);
                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBrick>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BRICK);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBrick>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BRICK);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBrick>().GetSkillLevel(skillLevel);
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
