using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventEnemyKingBigBreakBird : Enemy
{
    [SerializeField] Transform muzzleTransform;

    float wantedDistanceFromPlayer = 2f;

    void Update()
    {
        if (isDeath)
            return;

        if (CheckDistanceFromPlayer(wantedDistanceFromPlayer))
        {
            enemySpeedUp = 1f;

            animator.SetFloat("BirdRunSpeed", 1f);

            //Shoot
            if (!isShoot)
            {
                checkingTimer_attack += Time.deltaTime;

                if (checkingTimer_attack > attackInterval)
                {
                    checkingTimer_attack = 0;
                    isShoot = true;
                    StartCoroutine(ShootWind(target));
                }
            }
        }
        else
        {
            enemySpeedUp = 2f;
            animator.SetFloat("BirdRunSpeed", 2f);
        }
    }

    IEnumerator ShootWind(Transform _target)
    {
        int count = 0;

        while (count < 2)
        {
            ShootOneWind(_target);
            count++;
            yield return new WaitForSeconds(0.1f);
        }

        isShoot = false;
    }

    void ShootOneWind(Transform _target)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK);

        if (_bullet != null)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<BigBreakBirdWindBullet>().SetMuzzleTransform(muzzleTransform); 
            _bullet.GetComponent<BigBreakBirdWindBullet>().Fire(_target.position);
        }
    }
}
