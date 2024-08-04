using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneLightning : MonoBehaviour, IPlayerBullet
{
    LightningEmitter m_lightningEmitter;

    int skillLevel;

    float duration;
    float checkingTimer = 0f;

    private void Start()
    {
        m_lightningEmitter = GameObject.FindObjectOfType<LightningEmitter>();
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _skillDuration)
    {
        duration = _skillDuration;
        
        checkingTimer = 0;
    }

    private void Update()
    {
        checkingTimer += Time.deltaTime;

        if (checkingTimer > duration)
            m_lightningEmitter.LightningExpired(this.gameObject);
    }

    public void Fire(Vector3 shootVect) { }
    public void SetMuzzleTransform(Transform muzzleTransform) { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_LIGHTNINGENITTER);
                if (_damage != null)
                {
                    _damage.transform.position = transform.position;
                    _damage.GetComponent<DamageLightning>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_LIGHTNINGENITTER);
                if (_damage != null)
                {
                    _damage.transform.position = transform.position;
                    _damage.GetComponent<DamageLightning>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_LIGHTNINGENITTER);
                if (_damage != null)
                {
                    _damage.transform.position = transform.position;
                    _damage.GetComponent<DamageLightning>().GetSkillLevel(skillLevel);
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
