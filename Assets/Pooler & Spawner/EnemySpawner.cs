using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    PickupManager m_pickupManager;

    [Header("Spawn Enemy Count")]
    [SerializeField] int numberOfEnemies = 0;
    [SerializeField] int[] numberOfEachEnemies = new int[(int)EnemyType.ENEMYTYPE_MAX_SIZE];

    [Header("Despawn Enemy Count")]
    [SerializeField] int enemyKills = 0;
    [SerializeField] int[] enemyTypeKills = new int[(int)EnemyType.ENEMYTYPE_MAX_SIZE];


    private void Awake()
    {
        Instance = this;

        m_pickupManager = GameObject.FindObjectOfType<PickupManager>();
    }

    public bool SpawnEnemy(EnemyType _enemyType, Vector3 _position)
    {
        GameObject _enemy = EnemyPooler.Instance.GetEnemy(_enemyType);

        if (_enemy)
        {
            _enemy.transform.position = _position;
            _enemy.SetActive(true);
            ++numberOfEnemies;
            ++numberOfEachEnemies[(int)_enemyType];
           // AddOrientationIcon((int)_enemyType, _enemy);
            return true;
        }
        return false;
    }

    void AddOrientationIcon(int _enemyType, GameObject _enemyObject)
    {
        if(_enemyType >= (int)EnemyType.EVENTENEMY_THREEROCKS && _enemyType <= (int)EnemyType.EVENTENEMY_BIGTURTLE)
            StageUIManager.Instance.AddOrientationIconUI(true, _enemyObject);
    }

    public bool DespawnEnemy(GameObject _enemy)
    {
        if (_enemy)
        {
            m_pickupManager.SpawnRandomSkillFuel(_enemy.transform.position);

            _enemy.SetActive(false);

            if (numberOfEachEnemies[(int)_enemy.GetComponent<EnemyStat>().enemyType] > 0)
            {
                --numberOfEnemies;
                --numberOfEachEnemies[(int)_enemy.GetComponent<EnemyStat>().enemyType];

                ++enemyKills;
                ++enemyTypeKills[(int)_enemy.GetComponent<EnemyStat>().enemyType];

                ScoreManager.Instance.UpdateEnemyKillCount(enemyKills);
            }
          
            //이벤트 보스: 행운열차 Pickup 드롭
            if ((int)_enemy.GetComponent<EnemyStat>().enemyType >= (int)EnemyType.EVENTENEMY_THREEROCKS)
                AddSpawnData(false, PickupType.LUCKYBOX_SKILLUPGRADE, _enemy.transform.position);

            return true;
        }
        return false;
    }

    //Pickup Bomb 기능_  필드상의 Enemy에게 엄청난 데미지 주기(Death)
    public void DoDamageAllOfSpawnEnemy() // Deactive x Damage o 
    {
        for (int i = 0; i < (int)EnemyType.ENEMYTYPE_MAX_SIZE; ++i) //  Normal Enemy & Event Enemy
        {
            if (numberOfEachEnemies[i] != 0)
            {
                int _spawnCount = numberOfEachEnemies[i];

                for (int j = 0; j < _spawnCount; ++j)
                {
                    EnemyPooler.Instance.DoDamageSelectEnemy(i, j);
                }
            }
        }
        //보스가 활성화 되어 있으면 데미지 주기
        EnemyPooler.Instance.DoDamageCurrentSpawnBoss();
    }

    //데이터 초기화
    public void Reset()
    {
        numberOfEnemies = 0;
        enemyKills = 0;

        for (int i = 0; i < (int)EnemyType.ENEMYTYPE_MAX_SIZE; ++i)
        {
            numberOfEachEnemies[i] = 0;
            enemyTypeKills[i] = 0;
        }

        EnemyPooler.Instance.InactiveAll();
    }

    void AddSpawnData(bool _isPickupBox, PickupType _typ, Vector3 _position)
    {
        PickupDataToSpawn data;
        data.isPickupBox = _isPickupBox;
        data.Type = _typ;
        data.position = _position;
        PickupSpawner.AddPickupToSpawn(data);
    }
}
