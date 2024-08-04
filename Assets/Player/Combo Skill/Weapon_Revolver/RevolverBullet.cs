using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverBullet : MonoBehaviour, IPlayerBullet
{
    RevolverSkillController m_revolverSkillController;

    Rigidbody2D rigid;

    Transform reloadTransform;

    int skillLevel = 0;

    float lifeTime = 2f;
    float expiredTimer = 0f;

    float moveFactor;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        m_revolverSkillController = GameObject.FindObjectOfType<RevolverSkillController>();
    }

    private void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > lifeTime)
        {
            m_revolverSkillController.RevolverBulletExpired(false,this);   
        }
    }

    public void UpdateSkillLevel(int _skillLevel)
    {
        skillLevel = _skillLevel;
    }

    public void UpdateSkillInfo(float _moveFactor)
    {
        moveFactor = _moveFactor;
    }

    public void Fire(Vector3 _shootVect)
    {
        Reload();

        float radValue = Mathf.Atan2(_shootVect.normalized.y, _shootVect.normalized.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        rigid.AddForce(_shootVect.normalized * moveFactor, ForceMode2D.Force);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
    }

    void Reload()
    {
        rigid.gravityScale = 0f;
        rigid.angularVelocity = 0;
        rigid.velocity = Vector2.zero;

        if (reloadTransform != null)
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
                GameObject _revolverBulletSkill = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_REVOLVER);

                if (_revolverBulletSkill != null)
                {
                    _revolverBulletSkill.transform.position = collision.transform.position;
                    _revolverBulletSkill.GetComponent<DamageRevolver>().GetSkillLevel(skillLevel);
                    _revolverBulletSkill.SetActive(true);
                }

                m_revolverSkillController.RevolverBulletExpired(true,this);
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat)
            {
                GameObject _revolverBulletSkill = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_REVOLVER);

                if (_revolverBulletSkill != null)
                {
                    _revolverBulletSkill.transform.position = collision.transform.position;
                    _revolverBulletSkill.GetComponent<DamageRevolver>().GetSkillLevel(skillLevel);
                    _revolverBulletSkill.SetActive(true);
                }
                m_revolverSkillController.RevolverBulletExpired(true, this);
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _revolverBulletSkill = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_REVOLVER);

                if (_revolverBulletSkill != null)
                {
                    _revolverBulletSkill.transform.position = collision.transform.position;
                    _revolverBulletSkill.GetComponent<DamageRevolver>().GetSkillLevel(skillLevel);
                    _revolverBulletSkill.SetActive(true);
                }

                m_revolverSkillController.RevolverBulletExpired(true, this);
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
