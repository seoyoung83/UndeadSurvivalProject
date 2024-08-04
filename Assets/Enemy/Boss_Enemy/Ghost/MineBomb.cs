using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MineBomb : MonoBehaviour
{
    enum BombType
    {
        Pink,
        Blue,
    }

    [SerializeField] BombType m_bombType;
    BossBulletType bulletType;

    public float damageValue { get; set; }

    [Header("Bomb Info_ Before Shoot Bullet")]
    Transform mineSpriteObject;
    float checkingTime_scale = 0f;
    float bounceSpeed = 2f;
    bool isBigger = true;
    int checkingBountCount = 0;
    int bounceCount;//Random 설정

    [Header("Bullet Info")]
    [SerializeField] Transform[] muzzleTrans;
    bool isShoot = false;

    private void Awake()
    {
        mineSpriteObject = transform.GetChild(0).transform;
      
        if (m_bombType == BombType.Blue)
            bulletType = BossBulletType.BOSS_GHOST_BLLUE_BULLET;
        else
            bulletType = BossBulletType.BOSS_GHOST_PINK_BULLET;

    }

    private void OnEnable()
    {
        mineSpriteObject.localScale = new Vector3(0.5f, 0.5f, 0);

        if ((int)m_bombType == 0) //Pink
        {
            bounceCount = Random.Range(2, 6); // 2~5 랜덤으로 터지기
        }
        else if ((int)m_bombType == 1) //Blue
        {
            bounceCount = Random.Range(2, 4); // 2~3 랜덤으로 터지기
        }

        isShoot = false;
    }

    private void OnDisable()
    {
        //Reset
        checkingTime_scale = 0f;
        checkingBountCount = 0;
        bounceCount = 0;
    }

    private void Update()
    {
        if (checkingBountCount >= bounceCount)
        {
            if (!isShoot)
                StartCoroutine(ShootingMineBullet());

            return;
        }

        // Bounce Effect (커졌다가 작아지는)
        if (checkingTime_scale >= 1f)
        {
            if (isBigger)
                checkingBountCount++;

            isBigger = false;
        }
        else if (checkingTime_scale <= 0.5f)
        {
            isBigger = true;
        }

        if (isBigger)
        {
            checkingTime_scale += Time.deltaTime * bounceSpeed;

        }
        else if (!isBigger)
        {

            checkingTime_scale -= Time.deltaTime * bounceSpeed;
        }

        mineSpriteObject.transform.localScale = new Vector3(checkingTime_scale, checkingTime_scale, 0);
    }

    public void BombSpawnTransform(Transform _spawnTrans)
    {
        transform.position = _spawnTrans.position;
    }

    IEnumerator ShootingMineBullet()
    {
        isShoot = true;

        int count = 0;
        while( count < muzzleTrans.Length )
        {
            Transform _trans = muzzleTrans[count];

            Vector3 moveVect = (_trans.position - transform.position).normalized;
           
            GameObject _bullet = EnemyBulletPooler.Instance.GetBossBullet(bulletType);
            if (_bullet)
            {
                _bullet.gameObject.SetActive(true);
                _bullet.GetComponent<MineBullet>().SetMuzzleTransform(transform);
                _bullet.GetComponent<MineBullet>().Shooting(moveVect);
            }

            count++;

            yield return null;
        }

        gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f); 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);
        }
    }
}
