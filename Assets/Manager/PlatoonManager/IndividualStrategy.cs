using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualStrategy : ICompositeStrategy
{
    EnemyType m_eEnemyType;
    Transform targetPlayer;

    float m_guidePosition;
    int spawnCount;

    int enemySpawnDirectIndex = -1;
    int[] enemySpawnDirect = { 0, 1, 2, 3, 1, 0, 3, 2 };

    public IndividualStrategy(Transform _tr)
    {
        targetPlayer = _tr;
    }

    public void Composite()
    {
        enemySpawnDirectIndex++;

        if (enemySpawnDirectIndex > enemySpawnDirect.Length - 1 )
            enemySpawnDirectIndex = 0;

        for (int i = 0; i < spawnCount; i++)
        {
            float randomValue = Random.Range(m_guidePosition, m_guidePosition);

            float _value = (2 * m_guidePosition) / spawnCount;
         
            switch (enemySpawnDirect[enemySpawnDirectIndex])
            {
                case 0:
                    //Left
                    EnemySpawner.Instance.SpawnEnemy(m_eEnemyType,
                        new Vector3(targetPlayer.position.x - randomValue + 2, targetPlayer.position.y + m_guidePosition - i * _value, 0));
                    break;
                case 1:
                    //Right
                    EnemySpawner.Instance.SpawnEnemy(m_eEnemyType,
                       new Vector3(targetPlayer.position.x + randomValue - 2, targetPlayer.position.y + m_guidePosition - i * _value, 0));
                    break;
                case 2:
                    //Top
                    EnemySpawner.Instance.SpawnEnemy(m_eEnemyType,
                      new Vector3(targetPlayer.position.x + m_guidePosition - i * _value, targetPlayer.position.y + randomValue, 0));
                    break;
                case 3:
                    //Bottom
                    EnemySpawner.Instance.SpawnEnemy(m_eEnemyType,
                      new Vector3(targetPlayer.position.x + m_guidePosition - i * _value, targetPlayer.position.y - randomValue, 0));
                    break;
            }
        }
    }

    public void SetAttackType(EnemyType _enemyType,float _gidPosition, int _spawnCount)
    {
        m_eEnemyType = _enemyType;
        m_guidePosition = _gidPosition - 1;
        spawnCount = _spawnCount;
    }
}
