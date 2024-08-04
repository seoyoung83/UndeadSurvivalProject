using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ForceBarrier : MonoBehaviour
{
    CapsuleCollider2D m_collider;

    SpriteRenderer m_spriteRenderer;

    float checkingTimer = 0f;

    bool isTurnToDisappear;
    bool isTriggerBoss = false;

    float blackholeScale;
    float skillDuration;
    float expansionSpeed = 10f;


    private void Awake()
    {
        m_collider = GetComponent<CapsuleCollider2D>();

        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSkillInfo(float _scale, float _skillDuration)
    {
        blackholeScale = _scale;

        skillDuration = _skillDuration;
    }

    private void Update()
    {
        if (isTurnToDisappear)
        {
            checkingTimer += Time.deltaTime;

            if (checkingTimer > skillDuration)
            {
                isTurnToDisappear = false;
                checkingTimer = 0f;
                StartCoroutine(DisappearEffect());
            }
        }
    }

    private void OnEnable()
    {
        m_spriteRenderer.color = m_spriteRenderer.color.WithAlpha(1);

        isTurnToDisappear = false;

        isTriggerBoss = false;

        checkingTimer = 0f;

        StartCoroutine(AppearEffect());
    }

    IEnumerator AppearEffect()
    {
        m_collider.enabled = true;

        float time = 0;
        while (time < blackholeScale)
        {
            time += Time.deltaTime * expansionSpeed;

            transform.localScale = new Vector3(time, time, 0f);
            yield return null;
        }

        transform.localScale = new Vector3(blackholeScale, blackholeScale, blackholeScale);

        isTurnToDisappear = true;
    }

    IEnumerator DisappearEffect()
    {
        m_collider.enabled = false;

        float time = 0;

        while (time < 0.3f)
        {
            time += Time.deltaTime * (expansionSpeed / 2);

            transform.localScale = new Vector3(blackholeScale + time, blackholeScale + time, 0f);

            float alpha = 1 - (time * 3.5f);

            Color newColor = m_spriteRenderer.color.WithAlpha(alpha);

            m_spriteRenderer.color = newColor;

            yield return null;
        }

        m_spriteRenderer.color = m_spriteRenderer.color.WithAlpha(0);

        gameObject.SetActive(false);

        yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().DoDefend(true);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStat _bossStat = collision.gameObject.GetComponent<BossStat>();
            if (_bossStat && !_bossStat.isDeath)
            {
                if (!isTriggerBoss)
                {
                    isTriggerBoss = true;
                    StartCoroutine(DisappearEffect());
                }
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStat _enemyStat = collision.gameObject.GetComponent<EnemyStat>();
            if (_enemyStat && !_enemyStat.isDeath)
            {
                Vector3 direct = (collision.transform.position - transform.position).normalized;

                Rigidbody2D enemyRigid = _enemyStat.transform.GetComponent<Rigidbody2D>();

                enemyRigid.AddForce(direct * 80, ForceMode2D.Force);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().DoDefend(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DestructibleEnemyBullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
