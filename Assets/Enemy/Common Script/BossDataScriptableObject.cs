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
        public float maxHp; //체력 최대치
        public float moveSpeed; //움직임 스피드
        public float attackInterval; //공격 간격
        public float clashDamageValue; //충돌시 데미지
    }

    //EnemyBulletPooler에 오브젝트 생성하고 보스전 끝나면 Destroy!
    [System.Serializable]
    public class BossBunnyAttackDamageDatas
    {
        //돌격:clashDamageValue와 동일-
        public float bigDustAttackDamageValue; //1개
        public float smallDustAttackDamageValue; //6개

        public float carrotDamageValue; //4개 (Stay)
    }

    [System.Serializable]
    public class BossGhostAttackDamageDatas
    {
        public float blueMinCrushDamage;//(3개) Stay 
        public float pinkMinCrushDamage;// Stay 

        public float blueBulletDamage;// 8개발사
        public float pinkBulletDamage;// 8개 발사
    }

    [System.Serializable]
    public class BossNinjaFrogAttackDamageDatas //ThrowingStar
    {
        //허수아비(8개) **착륙 시 dust 공격 추가하기
        //허수아비 적중시 Fire (8개)
        //허수아비로 날리는 파이어 불렛(8개) _현재 데미지 설정 100

        //큰 수리창(3개) _현재 데미지 설정 200
        //큰 수리창에서 파생되는 작은 수리창(큰 수리창 하나당 8개)  _현재 설정 100
        //작은 수리창(반사)(7방향 3개 총 21개) _현재 데미지 설정 50    

        public float ScarecrowTargetCrushDamage;// (8개) stay  
        public float ScarecrowTargetLandingDustDamage; // 8개 
        public float ScarecrowTargetFireDamage; //8개
        public float fireBulletAtScarecrowTargetDamage;

        public float bigThrowingStarDamage; //큰 수리창(3개) 
        public float bigThrowingStarChildDamage; //큰 수리창에서 파생되는 작은 수리창(큰 수리창 하나당 8개)
        
        public float reflectiveThrowingStarDamage; //21개  
    }


    public BossDatas[] bossDatasSet;

    public BossBunnyAttackDamageDatas bossBunnyAttackDamageDatasSet;

    public BossGhostAttackDamageDatas bossGhostAttackDamageDatasSet;

    public BossNinjaFrogAttackDamageDatas bossNinjaFrogAttackDamageDatasSet;
}
