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

        public float maxHp; //ü�� �ִ�ġ

        public float moveSpeed; //������ ���ǵ�
        public float attackInterval; //���� ����

        public float clashDamageValue; //�浹�� ������
        public float attackDamageValue; //(�ҷ� ��) ���� ������
    }

    public EnemiesDatas[] enemiesDatasSet;

}
