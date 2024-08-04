using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigBreakBird : Enemy
{
    float wantedDistanceFromPlayer = 2.5f;

    void Update()
    {
        if (isDeath)
            return;

        if (base.CheckDistanceFromPlayer(wantedDistanceFromPlayer))
        {
            enemySpeedUp = 1;
            animator.SetFloat("BirdRunSpeed", 1f);

            //Shoot
            if (!isShoot)
            {
                checkingTimer_attack += Time.deltaTime;

                if (checkingTimer_attack > attackInterval)
                {
                    checkingTimer_attack = 0;
                    isShoot = true;
                    ShootOneWind(target);
                }
            }
        }
        else
        {
            enemySpeedUp = 1.5f;
            animator.SetFloat("BirdRunSpeed", 2.0f);
        }
    }
  
    void ShootOneWind(Transform _target)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.ENEMY_BIGBREAKBIRD_WINDATTACK);

        if (_bullet != null)
        {
            _bullet.transform.position = transform.position;
            _bullet.SetActive(true);
            _bullet.GetComponent<BigBreakBirdWindBullet>().SetMuzzleTransform(transform.GetChild(0));
            _bullet.GetComponent<BigBreakBirdWindBullet>().Fire(_target.position);
        }

        isShoot = false;
    }
}
