using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectOptimizationManager : MonoBehaviour
{
    //Bullet ����ȭ
    // Pickup

    //���� ���� ����
    public void DeleteUnusedObjects(int _round)
    {
        //Enemy && Enemy Bullet ����
        DeleteUnusedEnemy(_round);

        //PickupItem ���� 
    }

    void DeleteUnusedEnemy(int _round)
    {
        switch (_round)
        {
            case 0:
                //�Ϲ� Enemy ����
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_BIGBREAK_BIRD);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_BIGBREAKBIRD_WINDATTACK);
                //Event Enemy ����
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_THREEROCKS);
                break;
            case 1:
                //�Ϲ� Enemy ����
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_MUSHROOM);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_SLIME);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_TURTLE);

                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_SLIME_GREENSLIME);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_TURTLE_THORNBULLET);

                //Event Enemy ����
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK);
                break;
            case 2:
                //������ Enemy
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_BABY_ROCK);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_ADULT_ROCK);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_FYINGBAT);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.ENEMY_TRUNK);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.ENEMY_TRUNK_FRUITBULLET);

                //Event Enemy ����
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_ARMYTRUNK);
                EnemyPooler.Instance.ClearUnusedEnemyType((int)EnemyType.EVENTENEMY_BIGTURTLE);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.EVENTENEMY_ARMYTRUNK_FRUITBULLET);
                EnemyBulletPooler.Instance.DestroyEnemyBullet((int)EnemyBulletType.EVENTENEMY_BIGTURTLE_THORNBULLET);
                break;

        }
    }
}