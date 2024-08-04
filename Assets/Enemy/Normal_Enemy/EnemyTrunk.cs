using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrunk : Enemy
{
    float wantedDistanceFromPlayer = 4;

    void Update()
    {
        if (isDeath)
            return;

        if (base.CheckDistanceFromPlayer(wantedDistanceFromPlayer))
        {
            //Shoot
            if (!isShoot)
            {
                checkingTimer_attack += Time.deltaTime;

                animator.SetFloat("shoot", checkingTimer_attack);

                if (checkingTimer_attack > attackInterval)
                {
                    checkingTimer_attack = 0;
                    isShoot = true;

                    TrunkShoot(target);
                }
            }
        }
    }

    void TrunkShoot(Transform _target)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.ENEMY_TRUNK_FRUITBULLET);

        if (_bullet != null)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<TrunkBullet>().SetMuzzleTransform(transform.GetChild(0));//trunk Spawn Trans
            _bullet.GetComponent<TrunkBullet>().Fire(_target.position);
        }
        isShoot = false;
    }
}
