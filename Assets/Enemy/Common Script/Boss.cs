using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Boss : MonoBehaviour
{
    public BossDataScriptableObject m_bossDataScriptableObject { get; set; }

    public Transform target { get; set; }

    protected Animator animator;

    protected SpriteRenderer m_bossSpriteRenderer;

    protected float bossSpeed;

    protected float attackInterval;

    protected float clashDamage;

    protected bool isDeath;
    
    //�ҷ� �� ������ ���ݿ� ���� �����ʹ� ��ũ���ͺ� ���� ���� 

    protected float checkingTime_attack = 0; //���ݽð� üũ
    protected float attackInternalTime;

    private void Start()
    {
        animator = GetComponent<Animator>();

        m_bossSpriteRenderer = GetComponent<SpriteRenderer>();

        bossSpeed = m_bossDataScriptableObject.bossDatasSet[(int)GetComponent<BossStat>().bossType].moveSpeed;

        attackInterval = m_bossDataScriptableObject.bossDatasSet[(int)GetComponent<BossStat>().bossType].attackInterval;

        clashDamage = m_bossDataScriptableObject.bossDatasSet[(int)GetComponent<BossStat>().bossType].clashDamageValue;

        isDeath = GetComponent<BossStat>().isDeath;

        GetComponent<Collider2D>().enabled = true;

        checkingTime_attack = 0;

        m_bossSpriteRenderer.color = new Color(1, 1, 1, 1);

        BossSpawnSet(true);
    }

    public abstract void BossSpawnSet(bool _spawn);

    public abstract void BossAttack();

    private void Update()
    {
        BossAttack();
    }

    protected void Move()
    {
        //Move
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * bossSpeed);

        //Sprite Direction
        m_bossSpriteRenderer.flipX = target.position.x < transform.position.x;
    }

    public void BossDeath()
    {
        isDeath = true;

        GetComponent<Collider2D>().enabled = false;

        m_bossSpriteRenderer.color = new Color(1, 1, 1, 0.5f);

        BossSpawnSet(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isDeath)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
                if (_playerStat)
                    _playerStat.AddDamage(clashDamage);
            }
        }
    }
}
