using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDirectionType
{
    NorthWest,
    SouthWest,
    NorthEast,
    SouthEast,
    MAX_SIZE,
}

public class CubeShapeStrategy : ICompositeStrategy
{
    AttackDirectionType m_eAttackDirectionType;
    EnemyType m_eEnemyType;
    Transform targetPlayer;
    float m_guidePosition;

    public CubeShapeStrategy(Transform _tr)
    {
        targetPlayer = _tr;
    }

    public void Composite()
    {
        switch (m_eAttackDirectionType)
        {
            case AttackDirectionType.NorthWest:
                NorthWest();
                break;
            case AttackDirectionType.SouthWest:
                SouthWest();
                break;
            case AttackDirectionType.NorthEast:
                NorthEast();
                break;
            case AttackDirectionType.SouthEast:
                SouthEast();
                break;
        }
    }

    public void SetAttackType(AttackDirectionType _eDirectionType, EnemyType _enemyType, float _guidePosition)
    {
        m_eAttackDirectionType = _eDirectionType;
        m_eEnemyType = _enemyType;
        m_guidePosition = _guidePosition;
    }

    void NorthWest()//µ¿ºÏ
    {
        Vector2 cubePoint = new Vector2(targetPlayer.position.x + Random.Range(m_guidePosition - 2f, m_guidePosition),
            targetPlayer.position.y + Random.Range(m_guidePosition - 2.5f, m_guidePosition));
       
        Collider2D enemySpawnCollider = Physics2D.OverlapBox(cubePoint, new Vector2(1.8f, 1.8f), 0, LayerMask.GetMask("Player"));

        if (enemySpawnCollider == null)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Vector2 position = new Vector2(cubePoint.x +( i * 0.6f), cubePoint.y + (j * 0.6f));

                    if ((int)Random.Range(1, 10) % (int)Random.Range(2, 4) != 0) 
                        EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, position);

                }
            }
        }
    }

    void SouthWest()//¼­ºÏ
    {
         Vector2 cubePoint = new Vector2(targetPlayer.position.x + Random.Range(-m_guidePosition + 2f, -m_guidePosition),
             targetPlayer.position.y + Random.Range(m_guidePosition - 2.5f, m_guidePosition));
       
        Collider2D enemySpawnCollider = Physics2D.OverlapBox(cubePoint, new Vector2(1.8f, 1.8f), 0, LayerMask.GetMask("Player"));

        if (enemySpawnCollider == null)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Vector2 position = new Vector2(cubePoint.x + (i * 0.6f), cubePoint.y + (j * 0.6f));

                    if ((int)Random.Range(1, 10) % (int)Random.Range(2,4) != 0)
                        EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, position);

                }
            }
        }
    }

    void NorthEast()//µ¿³²
    {
        Vector2 cubePoint = new Vector2(targetPlayer.position.x + Random.Range(m_guidePosition - 2f, m_guidePosition),
            targetPlayer.position.y + Random.Range(-m_guidePosition + 2.5f, -m_guidePosition));

        Collider2D enemySpawnCollider = Physics2D.OverlapBox(cubePoint, new Vector2(1.8f, 1.8f), 0, LayerMask.GetMask("Player"));

        if (enemySpawnCollider == null)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Vector2 position = new Vector2(cubePoint.x + (i * 0.6f), cubePoint.y + (j * 0.6f));

                    if ((int)Random.Range(1, 10) % (int)Random.Range(2, 4) != 0)
                        EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, position);

                }
            }
        }
    }

    void SouthEast()//¼­³²
    {
        Vector2 cubePoint = new Vector2(targetPlayer.position.x + Random.Range(-m_guidePosition + 2f, -m_guidePosition),
            targetPlayer.position.y + Random.Range(-m_guidePosition + 2.5f, -m_guidePosition));

        Collider2D enemySpawnCollider = Physics2D.OverlapBox(cubePoint, new Vector2(1.8f, 1.8f), 0, LayerMask.GetMask("Player"));

        if (enemySpawnCollider == null)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Vector2 position = new Vector2(cubePoint.x + (i * 0.6f), cubePoint.y + (j * 0.6f));

                    if ((int)Random.Range(1, 10) % (int)Random.Range(2, 4) != 0)
                        EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, position);

                }
            }
        }
    }
}
