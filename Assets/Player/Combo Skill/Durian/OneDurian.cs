using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDurian : MonoBehaviour, IPlayerBullet
{
    public AudioSource skillAudio { get; set; }

    Rigidbody2D rigid;

    Vector2 velocity;

    int skillLevel;

    float speed;

    float localScale;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(StartSizeEffect());
    }
    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _speed, float _objectScale)
    {
        speed = _speed;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(_objectScale, _objectScale, _objectScale), 1f);
    }

    private void Update()
    {
        velocity = rigid.velocity;
    }

    public void Fire(Vector3 _velocity)
    {
        rigid.velocity = _velocity * (speed + (speed / 2));
    }

    public void SetMuzzleTransform(Transform _reloadTrans)
    {
        rigid.velocity = Vector3.zero;

        if (_reloadTrans)
            transform.position = _reloadTrans.position;
    }

    IEnumerator StartSizeEffect()
    {
        float time = 0;

        while (time < localScale)
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(time, time, time);
            yield return null;
        }
        transform.localScale = new Vector3(localScale, localScale, localScale);
        yield return null;
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

    //자식 오브젝트"Reflective palyScreen ,BossFence": (Layer:Objects Reflected On PlayScreen_상호작용대상: Boss울타리 & PlayScreen )
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayScreenTool"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.2f);
        }

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.2f);
        }
    }
}
