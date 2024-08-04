using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiBullet : MonoBehaviour, IPlayerBullet
{
    Rigidbody2D rigid;

    Transform reloadTransform;

    int skillLevel;

    float lifeTime = 0.8f;
    float expiredTimer = 0f;

    float moveFactor;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _moveFactor)
    {
        moveFactor = _moveFactor;
    }

    private void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    public void Fire(Vector3 _shootVect)
    {
        Reload();

        Vector2 forwardNormal = (_shootVect - transform.position).normalized;

        float radValue = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        rigid.AddForce(forwardNormal * moveFactor, ForceMode2D.Force);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
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

    public void SetMuzzleTransform(Transform _transform)
    {
        reloadTransform = _transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_KUNAI);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageKunai>().GetSkillLevel(skillLevel); 
                    _damage.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_KUNAI);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageKunai>().GetSkillLevel(skillLevel); 
                    _damage.SetActive(true);
                }
                gameObject.SetActive(false);
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
