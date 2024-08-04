using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnemyTurtle : Enemy
{
    [SerializeField] Transform[] thornShootTrans;
    [SerializeField] Transform thornMuzzleTrans;

    float wantedDistanceFromPlayer = 4f;

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

                if (checkingTimer_attack > attackInterval)
                {
                    checkingTimer_attack = 0;
                    isShoot = true;

                    StartCoroutine(FiveThornBulletShoot());
                }

                animator.SetFloat("shoot", checkingTimer_attack);
            }
        }
    }

    IEnumerator FiveThornBulletShoot()
    {
        int count = 0;

        while (count < thornShootTrans.Length )
        {
            Transform _trans = thornShootTrans[count];

            Vector3 moveVect = (_trans.position - thornMuzzleTrans.position).normalized;

            ShootOneThorn(_trans, moveVect);

            count++;
            yield return new WaitForFixedUpdate();
        }

        yield return isShoot = false;
    }

    void ShootOneThorn(Transform _muzzleTrans, Vector3 _shootTrans)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.EVENTENEMY_BIGTURTLE_THORNBULLET);

        if (_bullet != null)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<TurtleThronBullet>().SetMuzzleTransform(_muzzleTrans);//trunk Spawn Trans
            _bullet.GetComponent<TurtleThronBullet>().Fire(_shootTrans);
        }
    }
}