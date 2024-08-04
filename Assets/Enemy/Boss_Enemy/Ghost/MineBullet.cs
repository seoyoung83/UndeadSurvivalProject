using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform reloadTransform;

    //°øÅë
    [SerializeField] float spinSpeed ;

    [SerializeField] float moveFactor;

    public float damageValue { get; set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (spinSpeed == 0)
        {
            return;
        }

        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.World);
    }

    public void Shooting(Vector3 _moveVect)
    {
        Reload();

        rigid.AddForce(_moveVect * moveFactor, ForceMode2D.Force);
    }

    public void Reload()
    {
        if (reloadTransform)
        {
            transform.position = reloadTransform.position;
        }
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

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            gameObject.SetActive(false);
        }
    }
}
