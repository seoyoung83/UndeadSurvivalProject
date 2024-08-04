using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneGuardian : MonoBehaviour, IPlayerBullet
{
    Guardian m_guardian;
    AudioSource skillAudio;

    int skillLevel;

    float spinSpeed = 300f;

    float checkingTimer = 0f;
    float duration;

    bool isNoLimit;

    Vector3 localScale;

    private void Start()
    {
        m_guardian= GameObject.FindObjectOfType<Guardian>();
    }

    private void OnEnable()
    {
        localScale = transform.localScale;
        StartCoroutine(StartEffect());
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(AudioSource _skillAudio, bool _isNoLimit, float _duration)
    {
        checkingTimer = 0f;

        duration = _duration;

        isNoLimit = _isNoLimit;

        skillAudio = _skillAudio;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0,0, -Time.time * spinSpeed);

        if (!isNoLimit)
        {
            checkingTimer += Time.deltaTime;

            if (checkingTimer > duration)
            {
                checkingTimer = 0f;

                m_guardian.GuardianExpired();
                gameObject.SetActive(false);
            }
        }
    }

    public void Fire(Vector3 shootVect) { }

    public void SetMuzzleTransform(Transform muzzleTransform) { }

    IEnumerator StartEffect()
    {
        float time = 0;
        while (time < localScale.x) 
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(time, time, time);

            yield return null;
        }
        transform.localScale = localScale;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_GUARDIAN);

                if (_damage != null)
                {
                    _damage.GetComponent<DamageGuardin>().GetSkillLevel(skillLevel);
                    _damage.transform.position = collision.transform.position;
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
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_GUARDIAN);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageGuardin>().GetSkillLevel(skillLevel);
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
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_GUARDIAN);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageGuardin>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                    skillAudio.Play();
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

        if (collision.gameObject.CompareTag("DestructibleEnemyBullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
