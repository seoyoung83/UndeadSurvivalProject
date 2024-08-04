using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFyingBat : Enemy
{
    float wantedDistanceFromPlayer = 2.5f;

    void Update()
    {
        if (isDeath)
            return;

        if (base.CheckDistanceFromPlayer(wantedDistanceFromPlayer))
        {
            enemySpeedUp = 1f;
            animator.SetFloat("runSpeed", 1f);
        }
        else
        {
            enemySpeedUp = 1.7f;
            animator.SetFloat("runSpeed", 3f);
        }
    }
}
