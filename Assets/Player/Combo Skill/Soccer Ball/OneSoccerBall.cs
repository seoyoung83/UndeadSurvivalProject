using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSoccerBall : MonoBehaviour , IPlayerBullet
{
    AudioSource skillAudio;
    [SerializeField] AudioClip damageClip;

    Rigidbody2D rigid;

    Transform reloadTransform;

    Vector2 velocity;

    int skillLevel;

    float speed;
    float skillDuration;
    float scaleSize;

    bool isDisappearing = false;
    float checkingTimer = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        velocity = rigid.velocity;
      
        if (!isDisappearing)
        {
            checkingTimer += Time.deltaTime;
            if (checkingTimer > skillDuration)
            {
                isDisappearing = true;
                checkingTimer = 0;
                BallExpired();
            }
        }
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void Fire(Vector3 _shootVect)
    {
        Reload();

        Vector3 direct = (_shootVect - reloadTransform.position).normalized;

        rigid.velocity = direct * speed;
    }

    void Reload()
    {
        if (reloadTransform != null)
        {
            rigid.velocity = Vector3.zero;
            transform.position = reloadTransform.position;
        }
        checkingTimer = 0;
        isDisappearing = false;
    }

    public void SetMuzzleTransform(Transform _reloadTrans)
    {
        GetComponent<SpriteRenderer>().color = Color.white;

        reloadTransform = _reloadTrans;
    }

    public void UpdateSkillInfo(AudioSource _skillAudio, float _speed, float _skillDuration)
    {
        speed = _speed;

        skillDuration = _skillDuration;

        scaleSize = transform.localScale.x;

        skillAudio = _skillAudio;
    }

    public void BallExpired()
    {
        StartCoroutine(StartBallExpired());
    }

    IEnumerator StartBallExpired()
    {
        float time = 0;
        while (time < scaleSize)
        {
            time += Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - time);
            gameObject.transform.localScale = new Vector3(scaleSize - time, scaleSize - time, scaleSize - time);
            yield return null;
        }

        gameObject.SetActive(false);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_SOCCERBALL);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageSoccerBall>().GetSkillLevel(skillLevel);
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

    //자식 오브젝트"Reflective palyScreec ,BossFence": (Layer:Objects Reflected On PlayScreen_상호작용대상: Boss울타리 & PlayScreen & Enemy)
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_SOCCERBALL);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageSoccerBall>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                    skillAudio.PlayOneShot(damageClip);
                }
                rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.25f);
            }

        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_SOCCERBALL);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageSoccerBall>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                    skillAudio.PlayOneShot(damageClip);
                }

                rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.25f);
            }
        }

        if (collision.gameObject.CompareTag("PlayScreenTool"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.25f);
            skillAudio.PlayOneShot(damageClip);
        }

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.25f);
            skillAudio.PlayOneShot(damageClip);
        }
    }
}
