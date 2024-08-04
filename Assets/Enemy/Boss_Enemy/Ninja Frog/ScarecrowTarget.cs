using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowTarget : MonoBehaviour 
{
    SpriteRenderer spriteRenderer;

    [Header("Landing")]
    Vector3 landingVect; //착륙할 위치

    float landingSpeed = 0.2f;
    float langingHigh = 4f;

    bool didLanding = false;

    //보스 불렛에 맞으면 deactive =>보스가 던진 불에 맞으면 활활 타고(이펙트) 꺼지기!!!!
    public float damageValue { get; set; }

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        GetComponent<Collider2D>().enabled = false;
    }

    public void GetSpawnPositionInfo(Vector3 _landingVect)
    {
        GetComponent<Collider2D>().enabled = false;

        spriteRenderer.color = new Color(1f, 1f, 1f, 0);
        
        didLanding = false;

        landingVect = _landingVect;

        transform.position = new Vector3(landingVect.x, landingVect.y + langingHigh, 0);

        StartCoroutine(LandingAtSpawnPosition());
    }

    IEnumerator LandingAtSpawnPosition()
    {
        float checkingAlpha_Timer = 0;

        while (transform.position != landingVect && checkingAlpha_Timer <= 1)
        {
            checkingAlpha_Timer += Time.deltaTime * 1.3f;

            if(checkingAlpha_Timer<=1)
                spriteRenderer.color = new Color(1f, 1f, 1f, checkingAlpha_Timer);

            transform.position = Vector3.Lerp(transform.position, landingVect, landingSpeed);
            yield return null;
        }

        GameObject _dust = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_DUST);
        if (_dust)
        {
            _dust.SetActive(true);
            _dust.transform.position = landingVect;
        }

        yield return new WaitForSeconds(0.1f);

        didLanding = true;

        GetComponent<Collider2D>().enabled = true;

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (didLanding)
        {
            if (collision.gameObject.GetComponent<FireBulletAtTarget>())
            {
                GameObject _fire = EnemyBulletPooler.Instance.GetBossBullet(BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_FIRE);
                if (_fire)
                {
                    _fire.SetActive(true);
                    _fire.transform.position = transform.position;
                }

                gameObject.SetActive(false);
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
                if (_playerStat)
                    _playerStat.AddDamage(damageValue);

                Vector3 direct = (collision.transform.position - transform.position).normalized;

                Rigidbody2D playerRigid = collision.gameObject.GetComponent<Rigidbody2D>();

                playerRigid.AddForce(direct * 50, ForceMode2D.Force);
            }
        }
    }
}
