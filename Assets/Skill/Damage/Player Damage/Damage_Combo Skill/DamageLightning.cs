using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLightning : SkillDamage
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string key = skillCategoryTypeNumber + "";

        float m_damage = PlayerStat.BuffOffense * m_SkillAbilityDataDictionary[key].skillImpactValueOfLevel[level];

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
                _enemyStat.AddDamage(m_damage);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _bossStat = collision.gameObject.GetComponent<BossStat>();
            if (_bossStat && !_bossStat.isDeath)
                _bossStat.AddDamage(m_damage);
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
                _objectStat.AddDamage(m_damage);
        }
    }

    public void GetSkillLevel(int _level)
    {
        level = _level;

        SetDamageArae(_level);
    }

    void SetDamageArae(int _level)
    {
        if (_level != 5)
            GetComponent<CircleCollider2D>().radius = 0.6f;
        else if(_level==5)
            GetComponent<CircleCollider2D>().radius = 0.8f;
    }
}
