using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneRocket : MonoBehaviour , IPlayerBullet
{
    AudioSource skillAudio;

    EffectType m_effectType;
    
    int skillLevel;

    Transform reloadTransform;
    
    Rigidbody2D rigid;

    float moveFactor;

    float checkingTimer = 0f;
    float skillDuration = 3.5f; 

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (skillLevel != 5)
            m_effectType = EffectType.EFFECTTYPE_ROCKET_EXPLOSION;
        else if(skillLevel==5)
            m_effectType = EffectType.EFFECTTYPE_SHARK_ROCKET_EXPLOSION;
    }

    private void Update()
    {
        checkingTimer += Time.deltaTime;

        if (checkingTimer > skillDuration)
            gameObject.SetActive(false);
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;

        if (skillLevel != 5)
            m_effectType = EffectType.EFFECTTYPE_ROCKET_EXPLOSION;
        else if (skillLevel == 5)
            m_effectType = EffectType.EFFECTTYPE_SHARK_ROCKET_EXPLOSION;
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
        if (reloadTransform)
        {
            rigid.velocity = Vector3.zero;
            transform.position = reloadTransform.position;
            transform.eulerAngles = reloadTransform.eulerAngles;
        }

        checkingTimer = 0;
    }

    public void SetMuzzleTransform(Transform _transform)
    { 
        reloadTransform = _transform;
    }

    public void UpdateSkillInfo(AudioSource _skillAudio, float _moveFactor)
    {
        skillAudio = _skillAudio;

        moveFactor = _moveFactor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _exposionFx = EffectPooler.Instance.GetEffect(m_effectType);
                if (_exposionFx != null)
                {
                    _exposionFx.transform.position = collision.transform.position;
                    _exposionFx.SetActive(true);
                }

                GameObject _exposionDamage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_ROCKET_EXPLOSION);

                if (_exposionDamage != null)
                {
                    _exposionDamage.GetComponent<DamageRocket>().GetSkillLevel(skillLevel);
                    _exposionDamage.transform.position = collision.transform.position;
                    _exposionDamage.SetActive(true);
                }

               gameObject.SetActive(false);
               skillAudio.Play();
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _exposionFx = EffectPooler.Instance.GetEffect(m_effectType);
                if (_exposionFx != null)
                {
                    _exposionFx.transform.position = collision.transform.position;
                    _exposionFx.SetActive(true);
                }

                GameObject _exposionDamage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_ROCKET_EXPLOSION);

                if (_exposionDamage != null)
                {
                    _exposionDamage.GetComponent<DamageRocket>().GetSkillLevel(skillLevel);
                    _exposionDamage.transform.position = collision.transform.position;
                    _exposionDamage.SetActive(true);
                }

                gameObject.SetActive(false);
                skillAudio.Play();
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_ROCKET_EXPLOSION);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageRocket>().GetSkillLevel(skillLevel);
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
