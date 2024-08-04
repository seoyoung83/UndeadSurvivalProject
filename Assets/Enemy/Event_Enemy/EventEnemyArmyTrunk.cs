using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnemyArmyTrunk : Enemy
{   
    [SerializeField] Transform shootTransform;

    float wantedDistanceFromPlayer = 5;

    void Update()
    {
        if (isDeath)
            return;

        if (CheckDistanceFromPlayer(wantedDistanceFromPlayer))
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

        bool isOnTheLeft = target.position.x > transform.position.x;

        float shootPositionX = isOnTheLeft == true ? 0.15f : -0.15f;

        shootTransform.localPosition = new Vector3(shootPositionX, -0.05f, 0);
    }

    void TrunkShoot(Transform _target)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.EVENTENEMY_ARMYTRUNK_FRUITBULLET);

        if (_bullet != null)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<TrunkBullet>().SetMuzzleTransform(shootTransform);//trunk Spawn Trans
            _bullet.GetComponent<TrunkBullet>().Fire(_target.position);
        }

        isShoot = false;
    }
}
