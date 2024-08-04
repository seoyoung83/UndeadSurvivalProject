using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackLineType
{
    LEFT,
    RIGHT,
    TOP,
    BOTTOM,
    MAX_SIZE,
}

public class LineStrategy : ICompositeStrategy
{
    AttackLineType m_eAttackLineType ;
    EnemyType m_eEnemyType;
    Transform targetPlayer;
    float m_guidePosition = 6f;
    int count = 11;

    public LineStrategy(Transform _tr)
    {
        targetPlayer = _tr;
    }

    public void Composite()
    {
        switch (m_eAttackLineType)
        {
            case AttackLineType.LEFT:
                CompositeAttackLineLeft();
                break;
            case AttackLineType.RIGHT:
                CompositeAttackLineRight();
                break;
            case AttackLineType.TOP:
                CompositeAttackLineTop();
                break;
            case AttackLineType.BOTTOM:
                CompositeAttackLineBottom();
                break;
        }
    }

    public void SetAttackType(AttackLineType _eLineType, EnemyType _enemyType, float _guidePosition )
    {
        m_eAttackLineType = _eLineType;
        m_eEnemyType = _enemyType;
        m_guidePosition = _guidePosition - 1;
    }

    void CompositeAttackLineLeft()
    {
        int randomExcludeCount = Random.Range(2, 6);

        for (int i = 0; i < count; i++)
        {
            if (i > 0 && i % randomExcludeCount == 0)
            {
                continue;
            }

            float randomXvalue = Random.Range(m_guidePosition - 0.5f, m_guidePosition + 0.5f);
            float _value = (2 * m_guidePosition) / count;
            EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, 
                new Vector3(targetPlayer.position.x - randomXvalue, targetPlayer.position.y+ m_guidePosition - i * _value, 0));

        }
    }

    void CompositeAttackLineRight()
    {
        int randomExcludeCount = Random.Range(2, 6);

        for (int i = 0; i < count; i++)
        {
            if (i > 0 && i % randomExcludeCount == 0)
            {
                continue;
            }

            float randomXvalue = Random.Range(m_guidePosition - 0.5f, m_guidePosition + 0.5f);
            float _value = (2 * m_guidePosition) / count;
            EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, 
                new Vector3(targetPlayer.position.x + randomXvalue, targetPlayer.position.y + m_guidePosition - i * _value, 0 ));
        }
    }

    void CompositeAttackLineTop()
    {
        int randomExcludeCount = Random.Range(2, 6);

        for (int i = 0; i < count; i++)
        {
            if (i > 0 && i % randomExcludeCount == 0)
            {
                continue;
            }

            float randomXvalue = Random.Range(m_guidePosition - 0.5f, m_guidePosition + 0.5f);

            float _value = (2 * m_guidePosition) / count;
            EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, 
                new Vector3(targetPlayer.position.x + m_guidePosition - i * _value, targetPlayer.position.y + randomXvalue, 0 ));
        }
    }

    void CompositeAttackLineBottom()
    {
        int randomExcludeCount = Random.Range(2, 6);

        for (int i = 0; i < count; i++)
        {
            if (i > 0 && i % randomExcludeCount == 0)
            {
                continue;
            }

            float randomXvalue = Random.Range(m_guidePosition - 0.5f, m_guidePosition + 0.5f);

            float _value = (2 * m_guidePosition) / count;
            EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, 
                new Vector3(targetPlayer.position.x + m_guidePosition - i * _value, targetPlayer.position.y - randomXvalue, 0));
        }
    }
}
