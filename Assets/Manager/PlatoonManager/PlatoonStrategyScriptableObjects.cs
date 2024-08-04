using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Platoon Strategy Datas", menuName = "ScriptableObjects/PlatoonStrategyScriptableObjects", order = 4)]

public class PlatoonStrategyScriptableObjects : ScriptableObject
{
    [System.Serializable]
    public class PlattonStrategyDatas
    {
        public int spawnMinute;
        public SpawnEnemy[] spawnEnemies;
    }

    [System.Serializable]
    public class SpawnEnemy
    {
        public EnemyType spawnEnemyType;
        public int count;
    }

    public PlattonStrategyDatas[] m_plattonStrategyDatas;
}
