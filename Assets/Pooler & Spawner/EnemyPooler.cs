using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    //일반몹
    ENEMY_MUSHROOM, 
    ENEMY_BABY_ROCK,
    ENEMY_ADULT_ROCK, 
    ENEMY_FYINGBAT, 
    ENEMY_SLIME,
    ENEMY_TRUNK,
    ENEMY_BIGBREAK_BIRD, 
    ENEMY_TURTLE,
    //이벤트 보스
    EVENTENEMY_THREEROCKS,
    EVENTENEMY_KINGBIGBREAK_BIRD,
    EVENTENEMY_ARMYTRUNK,
    EVENTENEMY_BIGTURTLE,
    ENEMYTYPE_MAX_SIZE,
}

public enum BossType
{
    BOSS_BUNNY,
    BOSS_GHOST,
    BOSS_NINJAFROG,
    BOSSTYPE_MAX_SIZE,
}

public class EnemyPooler : MonoBehaviour
{
    public static EnemyPooler Instance;

    [SerializeField] EnemiesDataScriptableObject m_enemiesDataScriptableObject;

    [SerializeField] BossDataScriptableObject m_bossDataScriptableObject;

    [Header("Enemy Mushroom")]
    [SerializeField] GameObject enemyMushroom;
    [SerializeField] int countEnemyMushroomPool;   
    
    [Header("Enemy Baby Rock")]
    [SerializeField] GameObject enemyBabyRock;
    [SerializeField] int countEnemyBabyRockPool;   
    
    [Header("Enemy Adult Rock")]
    [SerializeField] GameObject enemyAdultRock;
    [SerializeField] int countEnemyAdultRockPool;

    [Header("Enemy FylingBat")]
    [SerializeField] GameObject enemyFylingBat;
    [SerializeField] int countEnemyFylingBatPool;

    [Header("Enemy Slime")]
    [SerializeField] GameObject enemySlime;
    [SerializeField] int countEnemySlimePool;   
    
    [Header("Enemy Trunk")]
    [SerializeField] GameObject enemyTrunk;
    [SerializeField] int countEnemyTrunkPool;  
    
    [Header("Enemy BigBreakBird")]
    [SerializeField] GameObject enemyBigBreakBird;
    [SerializeField] int countEnemyBigBreakBirdPool;   
    
    [Header("Enemy Turtle")]
    [SerializeField] GameObject enemyTurtle;
    [SerializeField] int countEnemyTurtlePool;

    [Header("Event Enemy Three Rock")] 
    [SerializeField] GameObject enemyThreeRocks;
    [SerializeField] int countEnemyThreeRocksPool;

    [Header("Event Enemy KingBigBreakBird")] 
    [SerializeField] GameObject enemyKingBird;
    [SerializeField] int countEnemyKingBirdPool;   
    
    [Header("Event Enemy Army Trunk")]
    [SerializeField] GameObject enemyArmyTrunk;
    [SerializeField] int countEnemyArmyTrunkPool;   
    
    [Header("Event Enemy BigTurtle")] 
    [SerializeField] GameObject enemyBigTurtle; 
    [SerializeField] int countEnemyBigTurtlePool;

    [Header("Boss Prefabs _Ghost")]
    [SerializeField] GameObject bossGhostPrefab;
    [Header("Boss Prefabs _Bunny")]
    [SerializeField] GameObject bossBunnyPrefab;
    [Header("Boss Prefabs _Ninja Frog")]
    [SerializeField] GameObject bossNinjaFrogPrefab;

    GameObject currentSpawnBoss;
    
    Transform followTarget;

    List<GameObject>[] enemyPool = new List<GameObject>[(int)EnemyType.ENEMYTYPE_MAX_SIZE];

    private void Awake()
    {
        Instance = this;

        followTarget = GameObject.FindObjectOfType<PlayerMove>().transform;

        InitializedEnemyPool();

        InitializedEventEnemyPool();

    }

    void InitializedEnemyPool()
    {
        enemyPool[(int)EnemyType.ENEMY_MUSHROOM] = new List<GameObject>();
        for (int i = 0; i < countEnemyMushroomPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyMushroom);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_MUSHROOM;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_MUSHROOM].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_MUSHROOM].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_BABY_ROCK] = new List<GameObject>();
        for (int i = 0; i < countEnemyBabyRockPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyBabyRock);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_BABY_ROCK;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_BABY_ROCK].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_BABY_ROCK].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_ADULT_ROCK] = new List<GameObject>(); 
        for (int i = 0; i < countEnemyAdultRockPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyAdultRock);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_ADULT_ROCK;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_ADULT_ROCK].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_ADULT_ROCK].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_FYINGBAT] = new List<GameObject>(); 
        for (int i = 0; i < countEnemyFylingBatPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyFylingBat);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_FYINGBAT;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_FYINGBAT].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_FYINGBAT].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_SLIME] = new List<GameObject>();
        for (int i = 0; i < countEnemySlimePool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemySlime);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_SLIME;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_SLIME].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_SLIME].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_TRUNK] = new List<GameObject>();
        for (int i = 0; i < countEnemyTrunkPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyTrunk);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_TRUNK;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_TRUNK].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_TRUNK].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_BIGBREAK_BIRD] = new List<GameObject>();
        for (int i = 0; i < countEnemyBigBreakBirdPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyBigBreakBird);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_BIGBREAK_BIRD;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_BIGBREAK_BIRD].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_BIGBREAK_BIRD].Add(_enemy);
        }

        enemyPool[(int)EnemyType.ENEMY_TURTLE] = new List<GameObject>();
        for (int i = 0; i < countEnemyTurtlePool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyTurtle);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.ENEMY_TURTLE;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.ENEMY_TURTLE].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.ENEMY_TURTLE].Add(_enemy);
        }
    }

    void InitializedEventEnemyPool()
    {
        enemyPool[(int)EnemyType.EVENTENEMY_THREEROCKS] = new List<GameObject>();
        for (int i = 0; i < countEnemyThreeRocksPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyThreeRocks);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.EVENTENEMY_THREEROCKS;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_THREEROCKS].maxHp;
            enemyPool[(int)EnemyType.EVENTENEMY_THREEROCKS].Add(_enemy);
        }

        enemyPool[(int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD] = new List<GameObject>();
        for (int i = 0; i < countEnemyKingBirdPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyKingBird);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.EVENTENEMY_KINGBIGBREAK_BIRD].Add(_enemy);
        }

        enemyPool[(int)EnemyType.EVENTENEMY_ARMYTRUNK] = new List<GameObject>();
        for (int i = 0; i < countEnemyArmyTrunkPool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyArmyTrunk);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.EVENTENEMY_ARMYTRUNK;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_ARMYTRUNK].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.EVENTENEMY_ARMYTRUNK].Add(_enemy);
        }

        enemyPool[(int)EnemyType.EVENTENEMY_BIGTURTLE] = new List<GameObject>();
        for (int i = 0; i < countEnemyBigTurtlePool; ++i)
        {
            GameObject _enemy = (GameObject)Instantiate(enemyBigTurtle);
            _enemy.SetActive(false);
            _enemy.transform.SetParent(transform);
            _enemy.GetComponent<EnemyStat>().enemyType = EnemyType.EVENTENEMY_BIGTURTLE;
            _enemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)EnemyType.EVENTENEMY_BIGTURTLE].maxHp;
            _enemy.GetComponent<Enemy>().target = followTarget;
            _enemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            enemyPool[(int)EnemyType.EVENTENEMY_BIGTURTLE].Add(_enemy);
        }
    }

    public GameObject GetEnemy(EnemyType _enemyType)
    {
        GameObject enemy = null;

        if (enemyPool[(int)_enemyType] == null)
            return null;

        for (int i = 0; i < enemyPool[(int)_enemyType].Count; ++i)
        {
            if (!enemyPool[(int)_enemyType][i].activeInHierarchy)
            {
                enemy = enemyPool[(int)_enemyType][i];
                return enemy;
            }
        }

        if (enemy == null)
            return CreatEnemy(_enemyType);

        return null;
    }

    GameObject CreatEnemy(EnemyType _enemyType)
    {
        GameObject _creatEnemy = null;

        switch (_enemyType)
        {
            case EnemyType.ENEMY_MUSHROOM:
                _creatEnemy = (GameObject)Instantiate(enemyMushroom);
                break;
            case EnemyType.ENEMY_BABY_ROCK:
                _creatEnemy = (GameObject)Instantiate(enemyBabyRock);
                break;
            case EnemyType.ENEMY_ADULT_ROCK:
                _creatEnemy = (GameObject)Instantiate(enemyAdultRock);
                break;
            case EnemyType.ENEMY_FYINGBAT:
                _creatEnemy = (GameObject)Instantiate(enemyFylingBat);
                break;
            case EnemyType.ENEMY_SLIME:
                _creatEnemy = (GameObject)Instantiate(enemySlime);
                break;
            case EnemyType.ENEMY_TRUNK:
                _creatEnemy = (GameObject)Instantiate(enemyTrunk);
                break;
            case EnemyType.ENEMY_BIGBREAK_BIRD:
                _creatEnemy = (GameObject)Instantiate(enemyBigBreakBird);
                break;
            case EnemyType.ENEMY_TURTLE:
                _creatEnemy = (GameObject)Instantiate(enemyTurtle);
                break;
        }

        if (_creatEnemy != null)
        {
            _creatEnemy.GetComponent<Enemy>().target = followTarget;
            _creatEnemy.GetComponent<Enemy>().m_enemiesDataScriptableObject = m_enemiesDataScriptableObject;
            _creatEnemy.GetComponent<EnemyStat>().enemyType = _enemyType;
            _creatEnemy.GetComponent<EnemyStat>().maxHp = m_enemiesDataScriptableObject.enemiesDatasSet[(int)_enemyType].maxHp;
            _creatEnemy.SetActive(false);
            _creatEnemy.transform.SetParent(transform);
            enemyPool[(int)_enemyType].Add(_creatEnemy);
        }

        for (int i = 0; i < enemyPool[(int)_enemyType].Count; ++i)
        {
            if (!enemyPool[(int)_enemyType][i].activeInHierarchy)
            {
                return enemyPool[(int)_enemyType][i];
            }
        }

        return null;
    }

    public void InactiveAll() //보스 등장시 , 철수
    {
        for (int i = 0; i < (int)EnemyType.ENEMYTYPE_MAX_SIZE; ++i)
        {
            for (int j = 0; j < enemyPool[i].Count; ++j)
            {
                enemyPool[i][j].SetActive(false);
            }
        }
    }

    public void ClearUnusedEnemyType(int _enemyTypeNumber)
    {
        for (int j = 0; j < enemyPool[_enemyTypeNumber].Count; ++j)
        {
            if (enemyPool[_enemyTypeNumber][j] != null)
                Destroy(enemyPool[_enemyTypeNumber][j]);
        }

        enemyPool[_enemyTypeNumber].Clear();

    }

    public GameObject GetBoss(BossType _bossType)
    {
        currentSpawnBoss = null;

        switch (_bossType)
        {
            case BossType.BOSS_BUNNY:
                currentSpawnBoss = (GameObject)Instantiate(bossBunnyPrefab);
                break;
            case BossType.BOSS_GHOST:
                currentSpawnBoss = (GameObject)Instantiate(bossGhostPrefab);
                break;
            case BossType.BOSS_NINJAFROG:
                currentSpawnBoss = (GameObject)Instantiate(bossNinjaFrogPrefab);
                break;
        }

        if (currentSpawnBoss != null)
        {
            currentSpawnBoss.GetComponent<Boss>().target = followTarget;
            currentSpawnBoss.GetComponent<Boss>().m_bossDataScriptableObject = m_bossDataScriptableObject;
            currentSpawnBoss.GetComponent<BossStat>().bossType = _bossType;
            currentSpawnBoss.GetComponent<BossStat>().maxHp = m_bossDataScriptableObject.bossDatasSet[(int)_bossType].maxHp;
            currentSpawnBoss.SetActive(false);
        }

        return currentSpawnBoss;
    }

    public GameObject GetCurrentBosstData()
    {
        if(currentSpawnBoss!=null)
            return currentSpawnBoss;
        else 
            return null;
    }

    public void DoDamageCurrentSpawnBoss()
    {
        if (currentSpawnBoss != null)
            currentSpawnBoss.GetComponent<BossStat>().AddDamage(20000f);
    }

    public void DoDamageSelectEnemy(int _enemyType, int _count)
    {
        if (enemyPool[_enemyType][_count].activeInHierarchy)
            enemyPool[_enemyType][_count].gameObject.GetComponent<EnemyStat>().AddDamage(99999999f);
    }
}