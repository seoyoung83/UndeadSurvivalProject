using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeBullet : MonoBehaviour, IPlayerBullet
{
    BazookaSkillController m_bazookaSkillController;

    Rigidbody2D rigid;

    Transform reloadTransform;

    int skillLevel;

    float lifeTime = 0.9f;
    float expiredTimer = 0f;

    float moveFactor;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        m_bazookaSkillController = GameObject.FindObjectOfType<BazookaSkillController>();
    }

    public void UpdateSkillLevel(int _skillLevel)
    {
        skillLevel = _skillLevel;
    }

    public void UpdateSkillInfo(float _moveFactor)
    {
        moveFactor = _moveFactor;
    }

    void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > lifeTime)
        {
            expiredTimer = 0;
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
        }
        expiredTimer = 0f;
    }

    public void SetMuzzleTransform(Transform _trans)
    {
        reloadTransform = _trans;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BAZOOKABULLET);
                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBazooka>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }

                m_bazookaSkillController.BlackholeBulletExpired(this);
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BAZOOKABULLET);
                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBazooka>().GetSkillLevel(0);
                    _damage.SetActive(true);
                }

                m_bazookaSkillController.BlackholeBulletExpired(this);
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject")) 
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                m_bazookaSkillController.BlackholeBulletExpired(this);
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
