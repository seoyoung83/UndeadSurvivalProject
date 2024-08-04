using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour , IEnemyBullet
{
    public float damageValue { get; set; }

    Transform reloadTransform;

    float lifeTime = 3f;
    float expiredTimer = 0f;

    private void Update()
    {
        expiredTimer += Time.deltaTime;

        if (expiredTimer > lifeTime)
        {
            expiredTimer = 0;

            gameObject.SetActive(false);
        }
    }

    public void Fire(Vector3 _spawnVect)
    {
        Reload();

        transform.transform.position = _spawnVect;
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

            PlayerMove _playerMove= collision.gameObject.GetComponentInParent<PlayerMove>();
            if(_playerMove)
                _playerMove.AbnormalStat();

            gameObject.SetActive(false);
        }
    }
}
