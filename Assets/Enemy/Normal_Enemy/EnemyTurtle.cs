using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyTurtle : Enemy
{
    [SerializeField] Transform[] thornShootTrans;
    [SerializeField] Transform thornMuzzleTrans;

    float wantedDistanceFromPlayer = 3.5f;

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

            float radValue = Mathf.Atan2(moveVect.y, moveVect.x);
            float shootAngle = radValue * (180 / Mathf.PI);

            ShootOneThorn(_trans ,moveVect , shootAngle);

            count++;
            yield return new WaitForFixedUpdate();
        }
        
        yield return isShoot = false;
    }

    void ShootOneThorn(Transform _muzzleTrans, Vector3 _shootTrans, float _angle)
    {
        GameObject _bullet = EnemyBulletPooler.Instance.GetEnemyBullet(EnemyBulletType.ENEMY_TURTLE_THORNBULLET);

        if (_bullet != null)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.GetComponent<TurtleThronBullet>().SetMuzzleTransform(_muzzleTrans);
            _bullet.GetComponent<TurtleThronBullet>().Fire(_shootTrans);
        }

    }
}
