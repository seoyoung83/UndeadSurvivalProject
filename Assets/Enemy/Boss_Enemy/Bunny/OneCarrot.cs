using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCarrot : MonoBehaviour
{
    Rigidbody2D rigid;

    Vector3 reloadVect;
    Vector2 velocity;

    public float damageValue { get; set; } //6f

    float shootSpeed = 4;
    int bossTriggerCount = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        velocity = rigid.velocity;
    }

    public void GetInitialVelocity(Vector2 _velocity)
    {
        Reload();

        rigid.velocity = _velocity * shootSpeed;
    }

    void Reload()
    {
        

        if (reloadVect != null)
        {
            rigid.velocity = Vector3.zero;
            transform.position = reloadVect;
        }
    }

    public void SetMuzzleTransform(Vector3 _vect)
    {
        bossTriggerCount = 0;

        reloadVect = _vect;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            bossTriggerCount++;

            if (bossTriggerCount > 1)
                collision.gameObject.GetComponent<BossBunny>().CarrotBulletExpired(this);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //자식 오브젝트"Reflective Only Wall": Boss울타리만 감지(Layer)=벽만 물리효과 
    {
        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (shootSpeed );
        }
    }
}
