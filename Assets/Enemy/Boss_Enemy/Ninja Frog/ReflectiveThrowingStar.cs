using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectiveThrowingStar : MonoBehaviour
{
    Rigidbody2D rigid;

    public float damageValue { get; set; }

    float shootSpeed = 5.5f;

    Vector3 reloadVect;
    Vector2 velocity;

    int contactCount = 0;

    float checkingTime_Timer = 0;
    float activeTime = 5f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        velocity = rigid.velocity;

        checkingTime_Timer += Time.deltaTime;
        if (checkingTime_Timer > activeTime)
        {
            gameObject.SetActive(false);
            checkingTime_Timer = 0;
        }
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
        contactCount = 0;
        checkingTime_Timer = 0;
    }

    public void SetMuzzleTransform(Vector3 _vect)
    {
        reloadVect = _vect;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);

            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //자식 오브젝트"Reflective Only Wall": Boss울타리만 감지(Layer)=벽만 물리효과 
    {
        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            rigid.velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal) * (shootSpeed * 1.2f);

            ++contactCount;

            if (contactCount > 1)
            {
                gameObject.SetActive(false);
                contactCount = 0;
            }
        }
    }
}
