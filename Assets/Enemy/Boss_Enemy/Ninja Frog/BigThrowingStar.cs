using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigThrowingStar : MonoBehaviour
{
    Rigidbody2D rigid;
 
    Vector3 reloadVect;

    public float damageValue { get; set; }//200

    float speed = 10f;

    bool isTriggerWall = false;

    int maxSpawnStars = 8;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 _moveVect)
    {
        Reload();

        if (!isTriggerWall)
        {
            rigid.velocity = _moveVect * speed;
        }
    }

    void Reload()
    {
        isTriggerWall = false;

        GetComponent<SpriteRenderer>().enabled = true;

        GetComponent<Collider2D>().enabled = true;

        if (reloadVect != null)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
            rigid.velocity = Vector3.zero;
            transform.position = reloadVect;
        }
    }

    public void SetMuzzleTransform(Vector3 _vect)
    {
        reloadVect = _vect;
    }

    IEnumerator ShootSmallStars()
    { 
        int count = 0;
        while (count < maxSpawnStars)
        {
            Quaternion _rotation = Quaternion.Euler(0f,0f, (count + 1) * 45f);

            Vector3 _direction = _rotation * Vector3.up;

            Vector3 _spawnPosition = transform.position + _direction;

            Vector3 _moveVect = (_spawnPosition - transform.position).normalized;

            ShootSmallOneStar(_spawnPosition, _moveVect);

            count++;

            yield return null;
        }

        gameObject.SetActive(false);
    }

    void ShootSmallOneStar(Vector3 _spawnVect, Vector3 _direction)
    {
        GameObject _smallStar = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTARCHILD);
        if (_smallStar)
        {
            _smallStar.gameObject.SetActive(true);
            _smallStar.GetComponent<ThrowingStar>().SetMuzzleTransform(_spawnVect);
            _smallStar.GetComponent<ThrowingStar>().Shoot(_direction);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);
        }

        if (collision.gameObject.CompareTag("BossFenceWall"))
        {
            isTriggerWall = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(ShootSmallStars());
        }
    }
}
