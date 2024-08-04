using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBreakBirdWindBullet : MonoBehaviour, IEnemyBullet
{
    public float damageValue { get; set; }

    Transform reloadTransform;

    Rigidbody2D rigid;

    float moveFactor = 150f;
    [SerializeField] float lifeTime; //N:0.3 , kING : 0.35F
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

        Vector3 direct = (_target - transform.position).normalized;

        float radValue = Mathf.Atan2(direct.y, direct.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        rigid.AddForce(direct * moveFactor, ForceMode2D.Force);

        transform.rotation = Quaternion.Euler(0, 0, shootAngle + 180);
    }

    void Reload()
    {
        if (reloadTransform)
        {
            transform.position = reloadTransform.position;
        }
        expiredTimer = 0f;
    }

    public void SetMuzzleTransform(Transform _muzzleTrans)
    {
        reloadTransform = _muzzleTrans;
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
