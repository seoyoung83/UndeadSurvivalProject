using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletAtTarget : MonoBehaviour
{
    Rigidbody2D rigid;

    Transform reloadTransform;

    public float damageValue { get; set; }

    float moveFactor = 200f;

    float lifeTime = 15f;
    float expiredTimer = 0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > lifeTime)
        {
           gameObject.SetActive(false);
        }
    }

    public void Shooting(Vector3 _shootVect)
    {
        Reload();

        Vector2 forwardNormal = (_shootVect - transform.position).normalized;

        float radValue = Mathf.Atan2(forwardNormal.y, forwardNormal.x);
        float shootAngle = radValue * (180 / Mathf.PI) - 90;

        rigid.AddForce(forwardNormal * moveFactor, ForceMode2D.Force);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle +90 );
    }

    void Reload()
    {
        rigid.gravityScale = 0f;
        rigid.angularVelocity = 0;
        rigid.velocity = Vector2.zero;

        if (reloadTransform)
        {
            transform.position = reloadTransform.position;
            transform.eulerAngles = reloadTransform.eulerAngles;
        }
        expiredTimer = 0f;
    }

    public void SetMuzzleTransform(Transform _transform)
    {
        reloadTransform = _transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ScarecrowTarget>())
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);
        }
    }
}
