using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectOptimizationManager : MonoBehaviour
{
    //Bullet 최적화
    // Pickup

    //보스 스폰 직전
    public void DeleteUnusedObjects(int _round)
    {
        //Enemy && Enemy Bullet 정리
        DeleteUnusedEnemy(_round);

        //PickupItem 정리 
    }

    void DeleteUnusedEnemy(int _round)
    {
        switch (_round)
        {
            case 0:
                //일반 Enemy 삭제
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_BIGBREAK_BIRD);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_BIGBREAKBIRD_WINDATTACK);
                //Event Enemy 삭제
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_THREEROCKS);
                break;
            case 1:
                //일반 Enemy 삭제
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_MUSHROOM);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_SLIME);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_TURTLE);

                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_SLIME_GREENSLIME);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_TURTLE_THORNBULLET);

                //Event Enemy 삭제
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK);
                break;
            case 2:
                //나머지 Enemy
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_BABY_ROCK);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_ADULT_ROCK);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_FYINGBAT);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_TRUNK);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_TRUNK_FRUITBULLET);

                //Event Enemy 삭제
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_ARMYTRUNK);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_BIGTURTLE);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.EVENTENEMY_ARMYTRUNK_FRUITBULLET);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.EVENTENEMY_BIGTURTLE_THORNBULLET);
                break;

        }
    }
}