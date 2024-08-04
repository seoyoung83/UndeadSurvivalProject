using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonFlame : MonoBehaviour
{
    int skillLevel;

    float checkingTimer = 0f;
    float triggerInterval = 0.5f;

    private void OnEnable()
    {
        checkingTimer = 0f;
        StartCoroutine(StartBigger());
    }

    public void GetSkillLevel(int _level)
    {
        skillLevel = _level;
    }
    
    IEnumerator StartBigger()
    {
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime * 5f;

            transform.localScale = new Vector3(timer, timer, 0);

            yield return null;
        }
        transform.localScale = new Vector3(1, 1, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        checkingTimer += Time.deltaTime;

        if (checkingTimer > triggerInterval)
            checkingTimer = 0f;
        else
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_MOLOTOVCOCKTAIL);
                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageMolotovCocktail>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_MOLOTOVCOCKTAIL);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageMolotovCocktail>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);

                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_MOLOTOVCOCKTAIL);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageMolotovCocktail>().GetSkillLevel(skillLevel);
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
