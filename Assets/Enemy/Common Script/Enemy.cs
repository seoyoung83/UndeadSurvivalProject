using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour 
{
    public EnemiesDataScriptableObject m_enemiesDataScriptableObject { get; set; }
    public Transform target { get; set; }

    protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;

    protected float enemySpeed;
    protected float enemySpeedUp = 1;
    protected float attackInterval;
    float clashDamage;

    protected bool isDeath = false;
    protected bool isShoot = false;

    float checkingTimer_clashDamage = 0;
    protected float checkingTimer_attack = 0;
   
    //Hp는 EnemyPooler에서 넣음
    //AttackDamage(bullet)값은 EnemyBulletPooler에서 넣음

    private void Awake()
    {
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemySpeed = m_enemiesDataScriptableObject.enemiesDatasSet[(int)GetComponent<EnemyStat>().enemyType].moveSpeed;

        attackInterval= m_enemiesDataScriptableObject.enemiesDatasSet[(int)GetComponent<EnemyStat>().enemyType].attackInterval;

        clashDamage = m_enemiesDataScriptableObject.enemiesDatasSet[(int)GetComponent<EnemyStat>().enemyType].clashDamageValue;
    }

    protected void OnEnable()
    {
        //Reset
        isDeath = false;

        spriteRenderer.color = new Color(1, 1, 1, 1f);

        animator.SetBool("IsDeath", false);

        checkingTimer_attack = 0;

        isShoot = false;
    }

    private void FixedUpdate()
    {
        if (isDeath)
            return;

        Move();
    }

    private void Move()
    {
        //Move
        Vector3 directVect = (target.position - transform.position).normalized;
        rigid.velocity = directVect * enemySpeed * enemySpeedUp;

        //Sprite Direction
        spriteRenderer.flipX = target.position.x > transform.position.x;
    }

    protected bool CheckDistanceFromPlayer(float _wantedDistanceWithPlayer)
    {
        if(_wantedDistanceWithPlayer==0)
            return true;

        float currentDistanceFromTarget = (transform.position - target.position).magnitude;

        bool didItWithinWantedDistance = currentDistanceFromTarget < _wantedDistanceWithPlayer ? true : false;

        if (didItWithinWantedDistance)
            return true;
        else
            return false;
    }

    public void EnemyDeath()
    {
        isDeath = true;

        animator.SetBool("IsDeath", true);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        spriteRenderer.color = new Color(1, 1, 1, 0.7f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDeath)
            return;

        checkingTimer_clashDamage += Time.deltaTime;

        if (checkingTimer_clashDamage > 1)
        {
            checkingTimer_clashDamage = 0;

            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
                if (_playerStat)
                    _playerStat.AddDamage(clashDamage);
            }
        }
    }
}
