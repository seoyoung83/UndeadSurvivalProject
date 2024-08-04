using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundStrategy : ICompositeStrategy
{
    Transform targetPlayer;
    EnemyType m_eEnemyType;
    float m_distance ; //*****Warmming일때 거리 변경

    public SurroundStrategy(Transform _tr)
    {
        targetPlayer = _tr;
    }

    public void Composite()
    {
        for (int i = 0; i < 18; ++i)
        {
            float _distance = Random.Range(m_distance - 0.5f, m_distance + 0.5f);

            Quaternion _rotation = Quaternion.Euler(0f, 0f, i * 20f);
            Vector3 _direction = _rotation * Vector3.up;
            Vector3 _position = targetPlayer.position + _direction * _distance;

            EnemySpawner.Instance.SpawnEnemy(m_eEnemyType, _position);
        }
    }

    public void SetAttackType(EnemyType _enemyType, float _distance)
    {
        m_eEnemyType = _enemyType;
        m_distance = _distance;
    }
}
