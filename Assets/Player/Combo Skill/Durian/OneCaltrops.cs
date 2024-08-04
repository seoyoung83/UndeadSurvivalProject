using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCaltrops : MonoBehaviour, IPlayerBullet
{
    int skillLevel = 5;

    public AudioSource skillAudio { get; set; }
    Rigidbody2D rigid;

    [SerializeField] Transform[] bulletMuzzleTransform;
    Vector2 velocity;
    bool isAttacking = false;

    float checkingTimer_clatropsBullet = 0;

    float initialAttackInterval = 0.2f;
    float initialAttackRang = 1f;
    float initialAttackSpeed = 4f;
    float initialSkillDuration = 0.5f;

    float attackRang;
    float attackSpeed;
    float skillDuration;

    int buleltTransformIndex = 0;

    float moveSpeed;

    float localScale;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        
        attackRang = initialAttackRang;
        attackSpeed = initialAttackSpeed;
        skillDuration = initialSkillDuration;
    }

    private void Update()
    {
        velocity = rigid.velocity;

        if(isAttacking)
            ShootCaltropsBullet();
    }

    private void OnEnable()
    {
        StartCoroutine(StartSizeEffect());
    }

    private void OnDisable()
    {
        isAttacking = false;

        checkingTimer_clatropsBullet = 0;
    }

    public void UpdateSkillLevel(int level) { }

    public void UpdateSkillInfo(float _speed, float _objectScale)
    {
        moveSpeed = _speed;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(_objectScale, _objectScale, _objectScale), 1f);
    }

    public void Fire(Vector3 _velocity)
    {
        rigid.velocity = _velocity * (moveSpeed + (moveSpeed / 2)); ;
    }

    public void SetMuzzleTransform(Transform _reloadTrans)
    {
        rigid.velocity = Vector3.zero;

        if (_reloadTrans)
        {
            transform.position = _reloadTrans.position;

            checkingTimer_clatropsBullet = 0;
        }
    }

    public void UpdateBuffInfo(float buffAttackSpeed, float buffAttackRang , float buffSkillDuration)
    {
        attackRang = initialAttackRang + (initialAttackRang * buffAttackRang / 100);

        attackSpeed = initialAttackSpeed + (initialAttackSpeed * buffAttackSpeed / 100);

        skillDuration = initialSkillDuration + (initialSkillDuration * buffSkillDuration / 100);
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

        isAttacking = true;

        yield return null;
    }

    void ShootCaltropsBullet()
    {
        checkingTimer_clatropsBullet += Time.deltaTime;

        if (checkingTimer_clatropsBullet > initialAttackInterval)
        {
            checkingTimer_clatropsBullet = 0f;

            Vector3 direct = (transform.parent.transform.localPosition - bulletMuzzleTransform[buleltTransformIndex].transform.localPosition).normalized;

            float radValue = Mathf.Atan2(direct.y, direct.x);
            float shootAngle = radValue * (180 / Mathf.PI);

            ShootCaltropsBullet(direct, shootAngle);

            buleltTransformIndex++;
            if (buleltTransformIndex >= bulletMuzzleTransform.Length)
                buleltTransformIndex = 0;
        }
    }

    void ShootCaltropsBullet(Vector3 _velocity, float _angle)
    {
        GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.DURIAN_SPECIAL_BULLET);

        if (_bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.gameObject.transform.localScale = new Vector3(attackRang, attackRang, 0);
            _bullet.GetComponent<CaltropsBullet>().SetCaltropBulletInfo(transform, attackSpeed, skillDuration, skillAudio);
            _bullet.GetComponent<CaltropsBullet>().Shooting(_velocity, _angle);
        }
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
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * moveSpeed;
        }

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * moveSpeed;
        }
    }
}
