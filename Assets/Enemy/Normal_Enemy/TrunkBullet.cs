using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkBullet : MonoBehaviour, IEnemyBullet
{
    Transform reloadTransform;

    Rigidbody2D rigid;

    public float damageValue { get; set; }

    float moveFactor = 150f;

    [SerializeField] float lifeTime; // N:2 , Army: 5f
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

    public void Fire(Vector3 _target)
    {
        Reload();

        float radValue = Mathf.Atan2(_target.normalized.y, _target.normalized.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        Vector3 direct = (_target - transform.position).normalized;

        rigid.AddForce(direct * moveFactor , ForceMode2D.Force);

        transform.rotation = Quaternion.Euler(0, 0, shootAngle );
    }

    void Reload()
    {
        if (reloadTransform)
        {
            transform.position = reloadTransform.position;
        }
        expiredTimer = 0f;
    }

    public void SetMuzzleTransform(Transform _trans)
    {
        reloadTransform = _trans;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);

            gameObject.SetActive(false);
        }
    }
}
