using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneQuantumBallReleaseType : MonoBehaviour , IPlayerBullet
{
    AudioSource skillAudio;
    [SerializeField] AudioClip damageClip;

    Rigidbody2D rigid;
 
    SpriteRenderer spriteRenderer;

    Transform reloadTransform;

    Vector2 velocity;

    int skillLevel = 5;

    float speed;
    float scaleSize;
    float skillDuration;

    float checkingTimer = 0;

    [Header("For Bullet")]
    int maxBasicBallCount = 3;
    int expiredBall;
    bool isRelease = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSkillLevel(int level) { }

    private void Update()
    {
        velocity = rigid.velocity;

        if (isRelease)
        {
            checkingTimer += Time.deltaTime;
            if (checkingTimer > skillDuration)
            {
                checkingTimer = 0;

                StartCoroutine(StartBallExpired());
            }
        }
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
    }

    public void SetMuzzleTransform(Transform _reloadTransform)
    {
        expiredBall = maxBasicBallCount;

        spriteRenderer.color = Color.white;

        isRelease = false;

        reloadTransform = _reloadTransform;
    }

    public void UpdateSkillInfo(AudioSource _skillAudio, float _speed, float _skillDuration)
    {
        skillAudio = _skillAudio;

        scaleSize = transform.localScale.x;

        speed = _speed;

        skillDuration = _skillDuration;
    }

    IEnumerator StartBallExpired()
    {
        float time = 0;
        while (time < scaleSize)
        {
            time += Time.deltaTime * 2f;

            float alpha = 1 - time;
            Color newColor = spriteRenderer.color.WithAlpha(alpha);
            spriteRenderer.color = newColor;

            gameObject.transform.localScale = new Vector3(scaleSize - time, scaleSize - time, scaleSize - time);
            yield return null;
        }

        gameObject.SetActive(false);    

        yield return null;
    }

    IEnumerator ReleaseQuantumBall(Vector2 reflectDirect) //90 = 120(-30) / 210 = 240(-30) / 330 = 360(-30)
    {
        isRelease = true;

        if (expiredBall <= 0)
            yield return null;

        int count = 0;
        while (count < maxBasicBallCount) 
        {
            Vector2 shootDirect;

            if (reflectDirect.x > 0 )
            {
                shootDirect = new Vector2(reflectDirect.x, reflectDirect.y * (count - 1)).normalized;
                ShootBasicQuantumBall(shootDirect, transform);
            }
            else if (reflectDirect.x < 0)
            {
                shootDirect = new Vector2(-reflectDirect.x, reflectDirect.y * (count - 1)).normalized;
                ShootBasicQuantumBall(shootDirect, transform);
            }
            count++;

            yield return null;
        }

        yield return null;
    }

    void ShootBasicQuantumBall(Vector3 _shootDirect, Transform _muzzle)
    {
        GameObject _bullet = PlayerBulletPooler.Instance.GetPlayerBullet(PlayerBulletType.SOCCERBALL_SPECIAL_BULLET);
        
        if (_bullet != null)
        {
            _bullet.transform.localScale = transform.localScale;
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<OneQuantumBallBasicType>().UpdateSkillInfo(skillAudio, speed, skillDuration);
            _bullet.GetComponent<OneQuantumBallBasicType>().SetMuzzleTransform(_muzzle);
            _bullet.GetComponent<OneQuantumBallBasicType>().Fire(_shootDirect);

            expiredBall--;
        }
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

    //자식 오브젝트"Reflective palyScreec ,BossFence": (Layer:Objects Reflected On PlayScreen_상호작용대상: Boss울타리 & PlayScreen&Enemy)
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

                rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.3f);

                Vector2 reflectDirect = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);

                if (!isRelease)
                    StartCoroutine(ReleaseQuantumBall(reflectDirect));
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

                rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.3f);

                Vector2 direct = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);
                if (!isRelease)
                    StartCoroutine(ReleaseQuantumBall(direct));
            }
        }

        if (collision.gameObject.CompareTag("PlayScreenTool"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.3f);

            Vector2 direct = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);
            if (!isRelease)
                StartCoroutine(ReleaseQuantumBall(direct));

            skillAudio.PlayOneShot(damageClip);
        }

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (speed * 1.3f);

            Vector2 direct = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);
            if (!isRelease)
                StartCoroutine(ReleaseQuantumBall(direct));

            skillAudio.PlayOneShot(damageClip);
        }
    }
}
