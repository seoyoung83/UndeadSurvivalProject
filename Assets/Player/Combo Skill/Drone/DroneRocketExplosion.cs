using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRocketExplosion : MonoBehaviour
{
    SkillType m_skillType;

    int skillLevel;

    public void GetInfo(SkillType _skillType, int _level)
    {
        m_skillType = _skillType;
        skillLevel = _level;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(m_skillType);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageDrone>().GetSkillLevel(skillLevel);
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
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(m_skillType);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageDrone>().GetSkillLevel(skillLevel);
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
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(m_skillType);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageDrone>().GetSkillLevel(skillLevel);
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
