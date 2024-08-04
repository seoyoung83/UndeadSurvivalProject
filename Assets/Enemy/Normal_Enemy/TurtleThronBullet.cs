using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleThronBullet : MonoBehaviour
{
    Transform reloadTransform;

    Rigidbody2D rigid;

    public float damageValue { get; set; }

    float moveFactor = 100f;

    [SerializeField] float lifeTime; // N: 0.25 , Big: 0.35f
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

    public void Fire(Vector3 _shootVect)
    {
        Reload();

        float radValue = Mathf.Atan2(_shootVect.y, _shootVect.x);
        float shootAngle = radValue * (180 / Mathf.PI);

        rigid.AddForce(_shootVect * moveFactor , ForceMode2D.Force);

        transform.localRotation = Quaternion.Euler(0, 0, shootAngle);
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
