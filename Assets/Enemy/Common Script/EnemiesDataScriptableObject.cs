using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies Datas", menuName = "ScriptableObjects/EnemiesDataScriptableObject", order = 4)]
public class EnemiesDataScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class EnemiesDatas
    {
        public uint enemyId;
        public EnemyType enemyType;

        public float maxHp; //체력 최대치

        public float moveSpeed; //움직임 스피드
        public float attackInterval; //공격 간격

        public float clashDamageValue; //충돌시 데미지
        public float attackDamageValue; //(불렛 등) 공격 데미지
    }

    public EnemiesDatas[] enemiesDatasSet;

}
