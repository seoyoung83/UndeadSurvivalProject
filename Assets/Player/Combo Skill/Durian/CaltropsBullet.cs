using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropsBullet : MonoBehaviour
{
    int skillLevel = 5;

    AudioSource skillAudio;
    Transform reloadTransform;

    Rigidbody2D rigid;

    float skillDuration;

    float expiredTimer = 0f;

    float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > skillDuration)
        {
            gameObject.SetActive(false);
        }
    }

    public void Shooting(Vector3 _velocity, float _angle)
    {
        Reload();

        rigid.velocity = _velocity * speed;
        transform.localRotation = Quaternion.Euler(0, 0, _angle);
    }

    void Reload()
    {
        if (reloadTransform)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            transform.position = reloadTransform.position;
        }
        expiredTimer = 0f;
    }

    public void SetCaltropBulletInfo(Transform _muzzleTrans, float _speed, float _skillDuration, AudioSource _skillAudio)
    {
        reloadTransform = _muzzleTrans;

        speed = _speed;

        skillDuration = _skillDuration;

        skillAudio = _skillAudio;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DURIAN);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDurian>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                    skillAudio.Play();
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DURIAN);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDurian>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                    skillAudio.Play();
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DURIAN);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDurian>().GetSkillLevel(skillLevel);
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
