using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDrillshot : MonoBehaviour, IPlayerBullet
{
    AudioSource skillAudio;
    Rigidbody2D rigid;

    Transform reloadTransform;

    [SerializeField] GameObject trailRenderer;

    Vector2 velocity;

    int skillLevel;

    float speed;

    float skillDuration;

    float checkingTimer = 0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;
    }

    public void UpdateSkillInfo(float _speed, float _skillDuration, AudioSource _skillAudio)
    {
        speed = _speed;

        skillDuration = _skillDuration;

        skillAudio = _skillAudio;
    }


    private void Update()
    {
        velocity = rigid.velocity;

        checkingTimer += Time.deltaTime;

        if (checkingTimer > skillDuration || rigid.velocity.x == 0 || rigid.velocity.y == 0)
        {
            checkingTimer = 0f;

            trailRenderer.SetActive(false);

            StartCoroutine(StartEffect());
        }
    }

    public void Fire(Vector3 _velocity)
    {
        Vector2 forwardNormal = _velocity.normalized;

        float radValue = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        rigid.velocity = forwardNormal * speed;

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle); 

        trailRenderer.SetActive(true);
    }

    public void SetMuzzleTransform(Transform _reloadTrans)
    {
        if (_reloadTrans != null)
        {
            rigid.velocity = Vector2.zero;

            transform.localRotation = Quaternion.identity;

            reloadTransform = _reloadTrans;

            transform.position = reloadTransform.position;
        }

        checkingTimer = 0;
    }

    IEnumerator StartEffect()
    {
        float time = 0;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();

        while (time < 1)
        {
            if (time < 1)
                time += Time.deltaTime * (speed);

            sprite.color = new Color(1, 1, 1, 1 - time);
            yield return null;
        }

        gameObject.SetActive(false);

        sprite.color = new Color(1, 1, 1, 1);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DRILLSHOT);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDrillshot>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _enemyStat = collision.gameObject.GetComponent<BossStat>();

            if (_enemyStat && !_enemyStat.isDeath)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DRILLSHOT);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDrillshot>().GetSkillLevel(skillLevel);
                    _damage.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("InfiniteHPObject"))
        {
            InfiniteStat _objectStat = collision.gameObject.GetComponent<InfiniteStat>();
            if (_objectStat)
            {
                GameObject _damage = SkillDamagePooler.Instance.GetPlayerSkill(SkillType.SKILL_DRILLSHOT);

                if (_damage != null)
                {
                    _damage.transform.position = collision.transform.position;
                    _damage.GetComponent<DamageDrillshot>().GetSkillLevel(skillLevel);
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
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed);

            Vector2 forwardNormal = rigid.velocity.normalized;

            float radValue = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
            float shootAngle = radValue * (180 / Mathf.PI);

            transform.localRotation = Quaternion.Euler(0, 0, shootAngle);

            skillAudio.Play();
        }

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed);

            Vector2 forwardNormal = rigid.velocity.normalized;

            float radValue = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
            float shootAngle = radValue * (180 / Mathf.PI);

            transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
            skillAudio.Play();
        }
    }
}
