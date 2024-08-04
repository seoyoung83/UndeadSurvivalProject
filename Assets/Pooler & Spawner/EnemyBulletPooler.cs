using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EnemyBulletType
{
    //Normal_Enemy
    ENEMY_SLIME_GREENSLIME,
    ENEMY_TRUNK_FRUITBULLET,
    ENEMY_BIGBREAKBIRD_WINDATTACK,
    ENEMY_TURTLE_THORNBULLET,
    //Event_Enemy
    EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK,
    EVENTENEMY_ARMYTRUNK_FRUITBULLET,
    EVENTENEMY_BIGTURTLE_THORNBULLET,
    ENEMYBULLET_MAX_SIZ,
}
public enum BossBulletType
{
    BOSS_BUNNY_BIGDUST,
    BOSS_BUNNY_SMALLDUST,
    BOSS_BUNNY_CARROT,
    BOSS_GHOST_BLLUE_MINE,
    BOSS_GHOST_PINK_MINE,
    BOSS_GHOST_BLLUE_BULLET,
    BOSS_GHOST_PINK_BULLET,
    BOSS_NINJAFROG_SCARECROWTARGET,
    BOSS_NINJAFROG_SCARECROWTARGET_DUST,
    BOSS_NINJAFROG_SCARECROWTARGET_FIRE,
    BOSS_NINJAFROG_FIREBULLET,
    BOSS_NINJAFROG_BIG_THROWINGSTAR,
    BOSS_NINJAFROG_BIG_THROWINGSTARCHILD,
    BOSS_NINJAFROG_REFLECTIVE_THROWINGSTAR,
    BOSSBULLET_MAX_SIZ,
}


public class EnemyBulletPooler : MonoBehaviour
{
    public static EnemyBulletPooler Instance;

    [SerializeField] EnemiesDataScriptableObject m_enemiesDataScriptableObject;
    [SerializeField] BossDataScriptableObject m_bossDataScriptableObject;

    [Header("Enemy Slime")]
    [SerializeField] GameObject enemySlimeBullet;
    [SerializeField] int countEnemySlimeBulletPool;

    [Header("Enemy Trunk")]
    [SerializeField] GameObject enemyTrunkBullet;
    [SerializeField] int countEnemyTrunkPool;

    [Header("Enemy BigBreakBird")]
    [SerializeField] GameObject enemyBigBreakBirdBullet;
    [SerializeField] int countEnemyBigBreakBirdBulletPool;

    [Header("Enemy Turtle")]
    [SerializeField] GameObject enemyTurtleBullet;
    [SerializeField] int countEnemyTurtleBulletPool; 

    [Header("EventEnemy King BigBreakBird")]
    [SerializeField] GameObject eventEnemyKingBigBreakBirdBullet;
    [SerializeField] int countEventEnemyKingBigBreakBirdBulletPool;

    [Header("EventEnemy Army Trunk")]
    [SerializeField] GameObject eventEnemyArmyTrunkBullet;
    [SerializeField] int countEventEnemyArmyTrunkPool;

    [Header("EventEnemy Big Turtle")]
    [SerializeField] GameObject eventEnemyBigTurtleBullet;
    [SerializeField] int countEventEnemyBigTurtleBulletPool;

    [Header("Boss Bunny Attacks")]
    [SerializeField] GameObject bossBunnyBigDust; 
    [SerializeField] int countBossBunnyBigDustPool;
   
    [SerializeField] GameObject bossBunnySmallDust;
    [SerializeField] int countBossBunnySmallDustPool;

    [SerializeField] GameObject bossBunnyCarrot;
    [SerializeField] int countBossBunnyCarrotPool; 

    [Header("Boss Ghost Attacks")]
    [SerializeField] GameObject bossGhostBlueMine;
    [SerializeField] int countBossGhostBlueMinePool;

    [SerializeField] GameObject bossGhostPinkMine;
    [SerializeField] int countBossGhostPinkMinePool;

    [SerializeField] GameObject bossGhostBlueBullet;
    [SerializeField] int countBossGhostBlueBulletPool;

    [SerializeField] GameObject bossGhostPinkBullet;
    [SerializeField] int countBossGhostPinkBulletPool; 

    [Header("Boss NinjaFrog Attacks")]
    [SerializeField] GameObject bossNinjaFrogScarecrowTarget; 
    [SerializeField] int countBossNinjaFrogScarecrowTargetPool;

    [SerializeField] GameObject bossNinjaFrogScarecrowTargetDust;
    [SerializeField] int countBossNinjaFrogScarecrowTargetDustPool;
    
    [SerializeField] GameObject bossNinjaFrogScarecrowTargetFire;
    [SerializeField] int countBossNinjaFrogScarecrowTargetFirePool; 

    [SerializeField] GameObject bossNinjaFrogFireBullet;
    [SerializeField] int countBossNinjaFrogFireBulletPool; 

    [SerializeField] GameObject bossNinjaFrogBigStar;
    [SerializeField] int countBossNinjaFrogBigStarPool; 

    [SerializeField] GameObject bossNinjaFrogBigStarChild;
    [SerializeField] int countBossNinjaFrogBigStarChildPool; 

    [SerializeField] GameObject bossNinjaFrogReflectiveStar;
    [SerializeField] int countBossNinjaFrogReflectiveStarPool;

    List<GameObject>[] enemyBulletPool = new List<GameObject>[(int)EnemyBulletType.ENEMYBULLET_MAX_SIZ];

    List<GameObject>[] currentBossBulletPool = new List<GameObject>[(int)BossBulletType.BOSSBULLET_MAX_SIZ];

    private void Awake()
    {
        Instance = this;

        //Normal Enemy Bullet
        enemyBulletPool[(int)EnemyBulletType.ENEMY_SLIME_GREENSLIME] = new List<GameObject>();   

        for (int i = 0; i < countEnemySlimeBulletPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(enemySlimeBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent<SlimeBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_SLIME].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.ENEMY_SLIME_GREENSLIME].Add(_enemyBullet);
        }

        enemyBulletPool[(int)EnemyBulletType.ENEMY_TRUNK_FRUITBULLET] = new List<GameObject>();
        for (int i = 0; i < countEnemyTrunkPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(enemyTrunkBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent <TrunkBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_TRUNK].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.ENEMY_TRUNK_FRUITBULLET].Add(_enemyBullet);
        }

        enemyBulletPool[(int)EnemyBulletType.ENEMY_BIGBREAKBIRD_WINDATTACK] = new List<GameObject>();
        for (int i = 0; i < countEnemyBigBreakBirdBulletPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(enemyBigBreakBirdBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent<BigBreakBirdWindBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_BIGBREAK_BIRD].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.ENEMY_BIGBREAKBIRD_WINDATTACK].Add(_enemyBullet);
        }

        enemyBulletPool[(int)EnemyBulletType.ENEMY_TURTLE_THORNBULLET] = new List<GameObject>();
        for (int i = 0; i < countEnemyTurtleBulletPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(enemyTurtleBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent<TurtleThronBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_TURTLE].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.ENEMY_TURTLE_THORNBULLET].Add(_enemyBullet);
        }

        //Event Enemy Bullet
        enemyBulletPool[(int)EnemyBulletType.EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK] = new List<GameObject>();
        for (int i = 0; i < countEventEnemyKingBigBreakBirdBulletPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(eventEnemyKingBigBreakBirdBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent<BigBreakBirdWindBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK].Add(_enemyBullet);
        }

        enemyBulletPool[(int)EnemyBulletType.EVENTENEMY_ARMYTRUNK_FRUITBULLET] = new List<GameObject>();
        for (int i = 0; i < countEventEnemyArmyTrunkPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(eventEnemyArmyTrunkBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent<TrunkBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_ARMYTRUNK].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.EVENTENEMY_ARMYTRUNK_FRUITBULLET].Add(_enemyBullet);
        }

        enemyBulletPool[(int)EnemyBulletType.EVENTENEMY_BIGTURTLE_THORNBULLET] = new List<GameObject>();
        for (int i = 0; i < countEventEnemyBigTurtleBulletPool; ++i)
        {
            GameObject _enemyBullet = (GameObject)Instantiate(eventEnemyBigTurtleBullet);
            _enemyBullet.SetActive(false);
            _enemyBullet.transform.SetParent(transform);
            _enemyBullet.GetComponent<TurtleThronBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_BIGTURTLE].attackDamageValue;
            enemyBulletPool[(int)EnemyBulletType.EVENTENEMY_BIGTURTLE_THORNBULLET].Add(_enemyBullet);
        }
    }

    public GameObject GetEnemyBullet(EnemyBulletType _enemyBulletType)
    {
        GameObject returnedGameObject = null;

        if (enemyBulletPool[(int)_enemyBulletType] == null)
            return null;

        for (int i = 0; i < enemyBulletPool[(int)_enemyBulletType].Count; ++i)
        {
            if (!enemyBulletPool[(int)_enemyBulletType][i].activeInHierarchy)
            {
                returnedGameObject = enemyBulletPool[(int)_enemyBulletType][i];
                return returnedGameObject;
            }
        }

        if (returnedGameObject == null)
        {
            return CreatEnemyBullet(_enemyBulletType);
        }

        return null;
    }

    GameObject CreatEnemyBullet(EnemyBulletType _newEnemyBulletType)
    {
        GameObject _creatEnemyBullet = null;

        switch (_newEnemyBulletType)
        {
            case EnemyBulletType.ENEMY_SLIME_GREENSLIME:
                _creatEnemyBullet = (GameObject)Instantiate(enemySlimeBullet);
                _creatEnemyBullet.GetComponent<SlimeBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_SLIME].attackDamageValue;
                break;
            case EnemyBulletType.ENEMY_TRUNK_FRUITBULLET:
                _creatEnemyBullet = (GameObject)Instantiate(enemyTrunkBullet);
                _creatEnemyBullet.GetComponent<TrunkBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_TRUNK].attackDamageValue;
                break;
            case EnemyBulletType.ENEMY_BIGBREAKBIRD_WINDATTACK:
                _creatEnemyBullet = (GameObject)Instantiate(enemyBigBreakBirdBullet);
                _creatEnemyBullet.GetComponent<BigBreakBirdWindBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_BIGBREAK_BIRD].attackDamageValue;
                break;
            case EnemyBulletType.ENEMY_TURTLE_THORNBULLET:
                _creatEnemyBullet = (GameObject)Instantiate(enemyTurtleBullet);
                _creatEnemyBullet.GetComponent<TurtleThronBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_TURTLE].attackDamageValue;
                break;
            case EnemyBulletType.EVENTENEMY_KINGBIGBREAKBIRD_WINDATTACK:
                _creatEnemyBullet = (GameObject)Instantiate(eventEnemyKingBigBreakBirdBullet);
                _creatEnemyBullet.GetComponent<BigBreakBirdWindBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD].attackDamageValue;
                break;
            case EnemyBulletType.EVENTENEMY_ARMYTRUNK_FRUITBULLET:
                _creatEnemyBullet = (GameObject)Instantiate(eventEnemyArmyTrunkBullet);
                _creatEnemyBullet.GetComponent<TrunkBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_ARMYTRUNK].attackDamageValue;
                break;
            case EnemyBulletType.EVENTENEMY_BIGTURTLE_THORNBULLET:
                _creatEnemyBullet = (GameObject)Instantiate(eventEnemyBigTurtleBullet);
                _creatEnemyBullet.GetComponent<TurtleThronBullet>().damageValue = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_BIGTURTLE].attackDamageValue;
                break;
        }

        if (_creatEnemyBullet != null)
        {
            _creatEnemyBullet.SetActive(false);
            _creatEnemyBullet.transform.SetParent(transform);
            enemyBulletPool[(int)_newEnemyBulletType].Add(_creatEnemyBullet);
        }


        for (int i = 0; i < enemyBulletPool[(int)_newEnemyBulletType].Count; ++i)
        {
            if (!enemyBulletPool[(int)_newEnemyBulletType][i].activeInHierarchy)
            {
                return enemyBulletPool[(int)_newEnemyBulletType][i];
            }
        }

        return null;

    }

    public  void BossBulletInitialized(BossType bossType)
    {
        switch (bossType)
        {
            case BossType.BOSS_BUNNY:
                currentBossBulletPool[(int)BossBulletType.BOSS_BUNNY_BIGDUST] = new List<GameObject>();
                for (int i = 0; i < countBossBunnyBigDustPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossBunnyBigDust);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<OneDust>().damageValue = m_bossDataScriptableObject.bossBunnyAttackDamageDatasSet.bigDustAttackDamageValue;
                    currentBossBulletPool[(int)BossBulletType.BOSS_BUNNY_BIGDUST].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_BUNNY_SMALLDUST] = new List<GameObject>();
                for (int i = 0; i < countBossBunnySmallDustPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossBunnySmallDust);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<OneDust>().damageValue = m_bossDataScriptableObject.bossBunnyAttackDamageDatasSet.smallDustAttackDamageValue;
                    currentBossBulletPool[(int)BossBulletType.BOSS_BUNNY_SMALLDUST].Add(_enemyBullet);
                }
                currentBossBulletPool[(int)BossBulletType.BOSS_BUNNY_CARROT] = new List<GameObject>();
                for (int i = 0; i < countBossBunnyCarrotPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossBunnyCarrot);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<OneCarrot>().damageValue = m_bossDataScriptableObject.bossBunnyAttackDamageDatasSet.carrotDamageValue;
                    currentBossBulletPool[(int)BossBulletType.BOSS_BUNNY_CARROT].Add(_enemyBullet);
                }
                break;
            case BossType.BOSS_GHOST: 
                currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_BLLUE_MINE] = new List<GameObject>();
                for (int i = 0; i < countBossGhostBlueMinePool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossGhostBlueMine);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<MineBomb>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.blueMinCrushDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_BLLUE_MINE].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_PINK_MINE] = new List<GameObject>();
                for (int i = 0; i < countBossGhostPinkMinePool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossGhostPinkMine);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<MineBomb>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.pinkMinCrushDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_PINK_MINE].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_BLLUE_BULLET] = new List<GameObject>();
                for (int i = 0; i < countBossGhostBlueBulletPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossGhostBlueBullet);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<MineBullet>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.blueBulletDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_BLLUE_BULLET].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_PINK_BULLET] = new List<GameObject>();
                for (int i = 0; i < countBossGhostPinkBulletPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossGhostPinkBullet);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<MineBullet>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.pinkBulletDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_GHOST_PINK_BULLET].Add(_enemyBullet);
                }
                break;
            case BossType.BOSS_NINJAFROG: 
                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogScarecrowTargetPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogScarecrowTarget);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<ScarecrowTarget>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.ScarecrowTargetCrushDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET].Add(_enemyBullet);
                }
 
                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_DUST] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogScarecrowTargetDustPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogScarecrowTargetDust);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<ScarecrowTargetLandingDust>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.ScarecrowTargetLandingDustDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_DUST].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_FIRE] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogScarecrowTargetFirePool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogScarecrowTargetFire);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponentInChildren<BrurningScarecroeTarget>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.ScarecrowTargetFireDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_FIRE].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_FIREBULLET] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogFireBulletPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogFireBullet);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<FireBulletAtTarget>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.fireBulletAtScarecrowTargetDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_FIREBULLET].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTAR] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogBigStarPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogBigStar);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<BigThrowingStar>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.bigThrowingStarDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTAR].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTARCHILD] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogBigStarChildPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogBigStarChild);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<ThrowingStar>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.bigThrowingStarChildDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTARCHILD].Add(_enemyBullet);
                }

                currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_REFLECTIVE_THROWINGSTAR] = new List<GameObject>();
                for (int i = 0; i < countBossNinjaFrogReflectiveStarPool; ++i)
                {
                    GameObject _enemyBullet = (GameObject)Instantiate(bossNinjaFrogReflectiveStar);
                    _enemyBullet.SetActive(false);
                    _enemyBullet.transform.SetParent(transform);
                    _enemyBullet.GetComponent<ReflectiveThrowingStar>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.reflectiveThrowingStarDamage;
                    currentBossBulletPool[(int)BossBulletType.BOSS_NINJAFROG_REFLECTIVE_THROWINGSTAR].Add(_enemyBullet);
                }
                break;
        }
    }

    public GameObject GetBossBullet(BossBulletType _bossBulletType)
    {
        GameObject returnedGameObject = null;

        if (currentBossBulletPool[(int)_bossBulletType] == null)
            return null;

        for (int i = 0; i < currentBossBulletPool[(int)_bossBulletType].Count; ++i)
        {
            if (!currentBossBulletPool[(int)_bossBulletType][i].activeInHierarchy)
            {
                returnedGameObject = currentBossBulletPool[(int)_bossBulletType][i];
                return returnedGameObject;
            }
        }

        if (returnedGameObject == null)
        {
            return CreatBossBullet(_bossBulletType);
        }

        return null;
    }

    GameObject CreatBossBullet(BossBulletType _bossBulletType)
    {
        GameObject _creatBossBullet = null;

        switch (_bossBulletType)
        {
            case BossBulletType.BOSS_BUNNY_BIGDUST:
                _creatBossBullet = (GameObject)Instantiate(bossBunnyBigDust);
                _creatBossBullet.GetComponent<OneDust>().damageValue = m_bossDataScriptableObject.bossBunnyAttackDamageDatasSet.bigDustAttackDamageValue;
                break;
            case BossBulletType.BOSS_BUNNY_SMALLDUST:
                _creatBossBullet = (GameObject)Instantiate(bossBunnySmallDust);
                _creatBossBullet.GetComponent<OneDust>().damageValue = m_bossDataScriptableObject.bossBunnyAttackDamageDatasSet.smallDustAttackDamageValue;
                break;
            case BossBulletType.BOSS_BUNNY_CARROT:
                _creatBossBullet = (GameObject)Instantiate(bossBunnyCarrot);
                _creatBossBullet.GetComponent<OneCarrot>().damageValue = m_bossDataScriptableObject.bossBunnyAttackDamageDatasSet.carrotDamageValue;
                break;
            case BossBulletType.BOSS_GHOST_BLLUE_MINE:
                _creatBossBullet = (GameObject)Instantiate(bossGhostBlueMine);
                _creatBossBullet.GetComponent<MineBomb>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.blueMinCrushDamage;
                break;
            case BossBulletType.BOSS_GHOST_PINK_MINE:
                _creatBossBullet = (GameObject)Instantiate(bossGhostPinkMine);
                _creatBossBullet.GetComponent<MineBomb>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.pinkMinCrushDamage;
                break;
            case BossBulletType.BOSS_GHOST_BLLUE_BULLET:
                _creatBossBullet = (GameObject)Instantiate(bossGhostBlueBullet);
                _creatBossBullet.GetComponent<MineBullet>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.blueBulletDamage;
                break;
            case BossBulletType.BOSS_GHOST_PINK_BULLET:
                _creatBossBullet = (GameObject)Instantiate(bossGhostPinkBullet);
                _creatBossBullet.GetComponent<MineBullet>().damageValue = m_bossDataScriptableObject.bossGhostAttackDamageDatasSet.pinkBulletDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogScarecrowTarget);
                _creatBossBullet.GetComponent<ScarecrowTarget>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.ScarecrowTargetCrushDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_DUST:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogScarecrowTargetDust);
                _creatBossBullet.GetComponent<ScarecrowTargetLandingDust>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.ScarecrowTargetLandingDustDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_SCARECROWTARGET_FIRE:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogScarecrowTargetFire);
                _creatBossBullet.GetComponent<BrurningScarecroeTarget>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.ScarecrowTargetFireDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_FIREBULLET:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogFireBullet);
                _creatBossBullet.GetComponent<FireBulletAtTarget>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.fireBulletAtScarecrowTargetDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTAR:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogBigStar);
                _creatBossBullet.GetComponent<BigThrowingStar>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.bigThrowingStarDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_BIG_THROWINGSTARCHILD:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogBigStarChild);
                _creatBossBullet.GetComponent<ThrowingStar>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.bigThrowingStarChildDamage;
                break;
            case BossBulletType.BOSS_NINJAFROG_REFLECTIVE_THROWINGSTAR:
                _creatBossBullet = (GameObject)Instantiate(bossNinjaFrogReflectiveStar);
                _creatBossBullet.GetComponent<ReflectiveThrowingStar>().damageValue = m_bossDataScriptableObject.bossNinjaFrogAttackDamageDatasSet.reflectiveThrowingStarDamage;
                break;
        }

        if (_creatBossBullet != null)
        {
            _creatBossBullet.SetActive(false);
            _creatBossBullet.transform.SetParent(transform);
            currentBossBulletPool[(int)_bossBulletType].Add(_creatBossBullet);
        }


        for (int i = 0; i < currentBossBulletPool[(int)_bossBulletType].Count; ++i)
        {
            if (!currentBossBulletPool[(int)_bossBulletType][i].activeInHierarchy)
            {
                return currentBossBulletPool[(int)_bossBulletType][i];
            }
        }

        return null;
    }

    public void DestroyEnemyBullet(int _EnemyBulletType)
    {
        for (int i = 0; i < enemyBulletPool[_EnemyBulletType].Count; ++i)
        {
            if (enemyBulletPool[_EnemyBulletType][i] != null)
                Destroy(enemyBulletPool[_EnemyBulletType][i]);
        }
    }

    public void DestroyBossBullet()
    {
        for (int i = 0; i < (int)BossBulletType.BOSSBULLET_MAX_SIZ; ++i)
        {
            if (currentBossBulletPool[i] != null)
            {
                for (int j = 0; j < currentBossBulletPool[i].Count; ++j)
                {
                    Destroy(currentBossBulletPool[i][j].gameObject);
                }
            }
           
        }
    }
}
