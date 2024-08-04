using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    float wantedDistanceFromPlayer = 0;

    void Update()
    {
        if (isDeath)
            return;

        if (base.CheckDistanceFromPlayer(wantedDistanceFromPlayer))
        {
            if (!isShoot)
            {
                checkingTimer_attack += Time.deltaTime;

                if (checkingTimer_attack > attackInterval)
                {
                    checkingTimer_attack = 0;
                    isShoot = true;
                    DropSlime(transform.GetChild(0)); //Slime Spawn Trans
                }
            }
        }
    }

    void DropSlime(Transform _dropTrans)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.ENEMY_SLIME_GREENSLIME);
        if (_bullet != null)
        {
            _bullet.SetActive(true);
            _bullet.GetComponent<SlimeBullet>().SetMuzzleTransform(transform.GetChild(0));//Slime Spawn Trans
            _bullet.GetComponent<SlimeBullet>().Fire(_dropTrans.position);
        }

        isShoot = false;
    }
}
