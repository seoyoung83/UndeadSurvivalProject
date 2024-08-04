using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Datas", menuName = "ScriptableObjects/BossDataScriptableObject", order = 3)]
public class BossDataScriptableObject : ScriptableObject 
{
    [System.Serializable]
    public class BossDatas
    {
        public uint enemyId;
        public BossType bossType;
        public float maxHp; //ü�� �ִ�ġ
        public float moveSpeed; //������ ���ǵ�
        public float attackInterval; //���� ����
        public float clashDamageValue; //�浹�� ������
    }

    //EnemyBulletPooler�� ������Ʈ �����ϰ� ������ ������ Destroy!
    [System.Serializable]
    public class BossBunnyAttackDamageDatas
    {
        //����:clashDamageValue�� ����-
        public float bigDustAttackDamageValue; //1��
        public float smallDustAttackDamageValue; //6��

        public float carrotDamageValue; //4�� (Stay)
    }

    [System.Serializable]
    public class BossGhostAttackDamageDatas
    {
        public float blueMinCrushDamage;//(3��) Stay 
        public float pinkMinCrushDamage;// Stay 

        public float blueBulletDamage;// 8���߻�
        public float pinkBulletDamage;// 8�� �߻�
    }

    [System.Serializable]
    public class BossNinjaFrogAttackDamageDatas //ThrowingStar
    {
        //����ƺ�(8��) **���� �� dust ���� �߰��ϱ�
        //����ƺ� ���߽� Fire (8��)
        //����ƺ�� ������ ���̾� �ҷ�(8��) _���� ������ ���� 100

        //ū ����â(3��) _���� ������ ���� 200
        //ū ����â���� �Ļ��Ǵ� ���� ����â(ū ����â �ϳ��� 8��)  _���� ���� 100
        //���� ����â(�ݻ�)(7���� 3�� �� 21��) _���� ������ ���� 50    

        public float ScarecrowTargetCrushDamage;// (8��) stay  
        public float ScarecrowTargetLandingDustDamage; // 8�� 
        public float ScarecrowTargetFireDamage; //8��
        public float fireBulletAtScarecrowTargetDamage;

        public float bigThrowingStarDamage; //ū ����â(3��) 
        public float bigThrowingStarChildDamage; //ū ����â���� �Ļ��Ǵ� ���� ����â(ū ����â �ϳ��� 8��)
        
        public float reflectiveThrowingStarDamage; //21��  
    }


    public BossDatas[] bossDatasSet;

    public BossBunnyAttackDamageDatas bossBunnyAttackDamageDatasSet;

    public BossGhostAttackDamageDatas bossGhostAttackDamageDatasSet;

    public BossNinjaFrogAttackDamageDatas bossNinjaFrogAttackDamageDatasSet;
}
