using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingStar : MonoBehaviour
{
    Rigidbody2D rigid;

    Vector3 reloadVect;

    public float damageValue { get; set; }
   
    float speed = 3f;

    float checkingTime_Timer = 0;
    float activeTime = 3;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 _moveVect)
    {
        Reload();

        rigid.velocity = _moveVect * speed;

        checkingTime_Timer += Time.deltaTime;
        if (checkingTime_Timer > activeTime)
        {
            gameObject.SetActive(false);
            checkingTime_Timer = 0;
        }
    }

    void Reload()
    {
        checkingTime_Timer = 0;

        if (reloadVect != null)
        {
            transform.position = reloadVect;
            rigid.velocity = Vector3.zero;
            transform.localScale = new Vector3(0.7f,0.7f,1);
        }
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

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            gameObject.SetActive(false);
        }
    }
}
