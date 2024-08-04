using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    BazookaSkillController m_bazookaSkillController;

    Animator animator;

    int skillLevel;

    float checkingTimer_active = 0f;

    float skillDuration;
    float blackholeScale;

    float rotateSpeed = 400f;

    bool isBigger = false;

    float checkingTimer_damage = 0f;
    float triggerInterval = 0.5f;

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _scale, float _skillDuration)
    {
        blackholeScale = _scale;

        skillDuration= _skillDuration;

        isBigger = false;

        checkingTimer_damage = 0f;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        m_bazookaSkillController = GameObject.FindObjectOfType<BazookaSkillController>();
    }

    private void Update()
    {
        checkingTimer_active += Time.deltaTime;

        if (checkingTimer_active > skillDuration - 0.2f)
        {
            animator.SetTrigger("deactive");

            if (checkingTimer_active > skillDuration)
            {
                m_bazookaSkillController.BlackholeExpired(this);
                checkingTimer_active = 0f;
            }
        }
    }

    IEnumerator StartActiveEffect()
    {
        isBigger = true;

        float time = 0;

        while(time < blackholeScale)
        {
            time += Time.deltaTime;

            float newScale = Mathf.Lerp(0, blackholeScale, time * 8f);
            transform.localScale = new Vector2(newScale, newScale);

            yield return null;
        }

        transform.localScale = new Vector3(blackholeScale, blackholeScale, blackholeScale); 
    }

    IEnumerator StartPullInEnemy(bool isBoss, Collider2D _enemyCollision)
    {
        float radValue = Mathf.Atan2(_enemyCollision.transform.position.normalized.y, _enemyCollision.transform.position.normalized.x);

        float angleIndegrees = radValue * Mathf.Deg2Rad;

        float diistanceToTargrt = (_enemyCollision.transform.position - transform.position).magnitude - (GetComponent<CircleCollider2D>().bounds.extents.x / 4);

        float checkingRotaion = angleIndegrees;

        while (gameObject.activeInHierarchy && _enemyCollision != null)
        {
            if (isBoss)
            {
                transform.position = Vector3.Lerp(transform.position, _enemyCollision.transform.position, Time.deltaTime * 1.25f);
            }
            else
            {
                checkingRotaion += Time.deltaTime * rotateSpeed;

                Vector3 fromAngleToVect = new Vector3(Mathf.Cos(checkingRotaion * Mathf.Deg2Rad), Mathf.Sin(checkingRotaion * Mathf.Deg2Rad), 0);

                if (diistanceToTargrt > 0.15f)
                    diistanceToTargrt -= Time.deltaTime * GetComponent<CircleCollider2D>().bounds.extents.x; 

                Vector3 wantedVect = transform.position + (fromAngleToVect * diistanceToTargrt);

                _enemyCollision.transform.position = wantedVect;
            }
            yield return null;
        }
    }

    public void SpawnEffect()
    {
        if (!isBigger)
            StartCoroutine(StartActiveEffect());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                StartCoroutine(StartPullInEnemy(false ,collision));
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                StartCoroutine(StartPullInEnemy(true, collision));
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        checkingTimer_damage += Time.deltaTime;

        if (checkingTimer_damage > triggerInterval)
            checkingTimer_damage = 0f;
        else
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BAZOOKABLACKHOLETRAP);
                if (_damage != null)
                {
                    _damage.transform.localScale = new Vector3(blackholeScale, blackholeScale, blackholeScale);
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBazooka>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_BAZOOKABLACKHOLETRAP);
                if (_damage != null)
                {
                    _damage.transform.localScale = new Vector3(blackholeScale, blackholeScale, blackholeScale);
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageBazooka>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }
    }
}