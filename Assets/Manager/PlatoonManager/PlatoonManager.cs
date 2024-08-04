using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatoonManager : MonoBehaviour
{
    public static PlatoonManager Instance;

    CameraMove m_cameraMove;

    BossManager m_bossManager;

    Transform playerTransform;
    
    bool isRush = false;

    float checkingTimer = 0;

    float currentStageTime;
    int currentRoundIndex;

    int[] eventEnemySpawnChecking = { 1, 1, 1, 1 };

    [Header("Irregular Individual Enemy Spawn Info")] //기본 생성 되는 전
    IndividualStrategy m_individualStrategy; //count 설정 가능,위치는 사각틀장소중에서 랜덤 생성
    
    [Header("Group Enemy Spawn Info")] // North, south, east and west 4방향으로 9조각 큐브 틀(Count 랜덤 3~8)
    CubeShapeStrategy m_cubeShapeStrategy;

    [Header("LineStarategy Spawn Info")] // LineStrategy(Count 랜덤 6~9)
    LineStrategy m_LineStrategy;

    [Header("SurroundStrategy Spawn Info")] // SurroundStrategy 18 Enemy 생성
    SurroundStrategy m_SurroundStrategy;

    private void Awake()
    {
        Instance = this;

        m_cameraMove = GameObject.FindObjectOfType<CameraMove>();

        playerTransform = GameObject.FindObjectOfType<PlayerMove>().transform;

        m_bossManager = GameObject.FindObjectOfType<BossManager>();

        m_individualStrategy = new IndividualStrategy(playerTransform);

        m_cubeShapeStrategy = new CubeShapeStrategy(playerTransform);

        m_LineStrategy = new LineStrategy(playerTransform);

        m_SurroundStrategy = new SurroundStrategy(playerTransform);
    }


    private void Start()
    {
        currentStageTime = GameManager.Instance.StageTime;
    }

    private void Update()
    {
        if (StageManager.playState == PlayState.NormalBattle)
        {
                if (!isRush)
                return;

            currentStageTime = GameManager.Instance.StageTime;

            int timeBasedOnCurrentRound = (currentRoundIndex + 1) * 300;
            float timeBasedOnFirstRound = currentRoundIndex == 0 ? 0.01f : timeBasedOnCurrentRound - 300;

            SpawnEnemy(0, currentStageTime >= timeBasedOnFirstRound && currentStageTime < timeBasedOnCurrentRound - 270); //0 ~ 30s
            SpawnEnemy(1, currentStageTime >= timeBasedOnCurrentRound - 270 && currentStageTime < timeBasedOnCurrentRound - 240);  //30s~1m
            SpawnEnemy(2, currentStageTime >= timeBasedOnCurrentRound - 240 && currentStageTime < timeBasedOnCurrentRound - 210); //1m~ 1m 30s
            SpawnEnemy(3, currentStageTime >= timeBasedOnCurrentRound - 210 && currentStageTime < timeBasedOnCurrentRound - 180); //1m 30s ~ 2m
            SpawnEnemy(4, currentStageTime >= timeBasedOnCurrentRound - 180 && currentStageTime < timeBasedOnCurrentRound - 150); //2m ~ 2m 30s
            SpawnEnemy(5, currentStageTime >= timeBasedOnCurrentRound - 150 && currentStageTime < timeBasedOnCurrentRound - 120);  //2m 30s ~ 3m
            SpawnEnemy(6, currentStageTime >= timeBasedOnCurrentRound - 120 && currentStageTime < timeBasedOnCurrentRound - 90);  // 3m ~ 3m 30s
            SpawnEnemy(7, currentStageTime >= timeBasedOnCurrentRound - 90 && currentStageTime < timeBasedOnCurrentRound - 60);   //3m 30s ~ 4m
            SpawnEnemy(8, currentStageTime >= timeBasedOnCurrentRound - 60 && currentStageTime < timeBasedOnCurrentRound - 2); //4m~ 4m 98s
            SpawnEnemy(9, currentStageTime >= timeBasedOnCurrentRound - 2 && currentStageTime < timeBasedOnCurrentRound);//4m 98s ~ 5m

            SpawnEventEnemy(0, currentStageTime > 120f && currentStageTime < 120.01f);  // 2분 :Three Rocks_Event
            SpawnEventEnemy(1, currentStageTime > 420f && currentStageTime < 420.01f); // 7분 :  King Big Break Bird_Event 
            SpawnEventEnemy(2, currentStageTime > 660f && currentStageTime < 660.01f); // 11분 : Army Trunk_Event 
            SpawnEventEnemy(3, currentStageTime > 780f && currentStageTime < 780.01f); // 13분 : Big Turtle_Event
        }
    }


    void SpawnEventEnemy(int eventEnemySpawnIndex, bool _spawnTime)
    {
        if (!_spawnTime)
            return;

        EnemyType eventEnemyType = EnemyType.EVENTENEMY_THREEROCKS;

        if (eventEnemySpawnChecking[eventEnemySpawnIndex] > 0)
        {
            eventEnemySpawnChecking[eventEnemySpawnIndex]--;

            m_individualStrategy.SetAttackType(eventEnemyType + eventEnemySpawnIndex, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 1);
            m_individualStrategy.Composite();
        }    
    }

    void SpawnEnemy(int _index, bool _spawnTime)
    {
        if (!_spawnTime)
            return;

        checkingTimer += Time.deltaTime;

        if (checkingTimer > 0.98f)
        {
            checkingTimer = 0;

            switch (currentRoundIndex)
            {
                case 0: //0~5m
                    SpawnEnemiesForFirstRound(_index);
                    break;
                case 1://5~10m
                    SpawnEnemiesForSecondRound(_index);
                    break;
                case 2: //10~15m
                    SpawnEnemiesForThirdRound(_index);
                    break;
            }

            if (_index == 9)
                CommandRush(false);
        }       
    }

    void SpawnEnemiesForFirstRound(int _index)
    {
        if (_index == 0) //0~0.5
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_MUSHROOM, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 1);
            m_individualStrategy.Composite();
        }
        else if (_index == 1)//0.5~1
        {
            for (int i = 0; i < 2; ++i) //ENEMY_MUSHROOM, ENEMY_SLIME
            {
                m_individualStrategy.SetAttackType(EnemyType.ENEMY_MUSHROOM + (4 * i), m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 1);
                m_individualStrategy.Composite();
            }
        }
        else if (_index == 2)//1~1.5
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_MUSHROOM, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 3);
            m_individualStrategy.Composite();
        }
        else if (_index == 3)//1.5~2
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_BIGBREAK_BIRD, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 3);
            m_individualStrategy.Composite();
        }
        else if (_index == 4)//2~2.5
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_BIGBREAK_BIRD, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 3);
            m_individualStrategy.Composite();
        }
        else if (_index == 5)//2.5~3
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_BABY_ROCK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 4);
            m_individualStrategy.Composite();
        }
        else if (_index == 6)//3~3.5
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_BABY_ROCK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 5);
            m_individualStrategy.Composite();

            int count = Random.Range(1, 5);

            m_LineStrategy.SetAttackType(AttackLineType.MAX_SIZE - count, EnemyType.ENEMY_BABY_ROCK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_LineStrategy.Composite();

        }
        else if (_index == 7)//3.5~4
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_TURTLE, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 5);
            m_individualStrategy.Composite();

            int count = Random.Range(1, 5);

            m_LineStrategy.SetAttackType(AttackLineType.MAX_SIZE - count, EnemyType.ENEMY_TURTLE, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_LineStrategy.Composite();
        }
        else if (_index == 8)//4~
        {
            int _random = Random.Range(1, 5);
            m_cubeShapeStrategy.SetAttackType(AttackDirectionType.MAX_SIZE - _random, EnemyType.ENEMY_BIGBREAK_BIRD, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_cubeShapeStrategy.Composite();
        }
    }

    void SpawnEnemiesForSecondRound(int _index)
    {
        if (_index == 0) //0~0.5
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_MUSHROOM, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 4);
            m_individualStrategy.Composite();
        }
        else if (_index == 1)//0.5~1
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_MUSHROOM, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 2);
            m_individualStrategy.Composite();

            int count = Random.Range(1, 5);

            m_LineStrategy.SetAttackType(AttackLineType.MAX_SIZE - count, EnemyType.ENEMY_TURTLE, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_LineStrategy.Composite();
        }
        else if (_index == 2 || _index == 3)//1~2
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_FYINGBAT, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 5);
            m_individualStrategy.Composite();

            int count = Random.Range(1, 5);

            m_LineStrategy.SetAttackType(AttackLineType.MAX_SIZE - count, EnemyType.ENEMY_TURTLE, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_LineStrategy.Composite();
        }
        else if (_index == 4 || _index == 5)//2~2.5
        {
            m_individualStrategy.SetAttackType(EnemyType.ENEMY_TURTLE, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 7);
            m_individualStrategy.Composite();

            int count = Random.Range(1, 5);

            m_LineStrategy.SetAttackType(AttackLineType.MAX_SIZE - count, EnemyType.ENEMY_TRUNK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_LineStrategy.Composite();
        }
        else if (_index == 6 || _index == 7)//3~4
        {
            for (int i = 0; i < 2; ++i) //ENEMY_FYINGBAT, ENEMY_SLIME
            {
                m_SurroundStrategy.SetAttackType(EnemyType.ENEMY_FYINGBAT + i, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
                m_SurroundStrategy.Composite();
            }
        }
        else if (_index == 8)//4~
        {
            for (int i = 0; i < 2; ++i) //ENEMY_MUSHROOM , ENEMY_SLIME
            {
                m_individualStrategy.SetAttackType(EnemyType.ENEMY_MUSHROOM + (4 * i), m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 8);
                m_individualStrategy.Composite();

                int _random = Random.Range(1, 5);
                m_cubeShapeStrategy.SetAttackType(AttackDirectionType.MAX_SIZE - _random, EnemyType.ENEMY_MUSHROOM + (4 * i), m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
                m_cubeShapeStrategy.Composite();
            }
        }
    }

    void SpawnEnemiesForThirdRound(int _index)
    {
        if (_index == 0 || _index == 1) //0~1
        {
            for (int i = 0; i < 2; ++i) //ENEMY_BABY_ROCK, ENEMY_FYINGBAT
            {
                m_SurroundStrategy.SetAttackType(EnemyType.ENEMY_BABY_ROCK + (2 * i), m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
                m_SurroundStrategy.Composite();
            }
        }
        else if (_index == 2 || _index == 3)//1~2
        {
            for (int i = 0; i < 2; ++i) 
            {
                m_individualStrategy.SetAttackType(EnemyType.ENEMY_FYINGBAT, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 20);
                m_individualStrategy.Composite();
            }
            m_SurroundStrategy.SetAttackType(EnemyType.ENEMY_TRUNK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_SurroundStrategy.Composite();
        }
        else if (_index == 4 || _index == 5)//2~3
        {
            for (int i = 0; i < 2; ++i)
            {
                m_individualStrategy.SetAttackType(EnemyType.ENEMY_ADULT_ROCK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 30);
                m_individualStrategy.Composite();
            }

            m_SurroundStrategy.SetAttackType(EnemyType.ENEMY_TRUNK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_SurroundStrategy.Composite();
        }
        else if (_index == 6 || _index == 7)//3~4
        {
            for (int i = 0; i < 2; ++i)
            {
                m_individualStrategy.SetAttackType(EnemyType.ENEMY_BABY_ROCK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 30);
                m_individualStrategy.Composite();
            }
            m_SurroundStrategy.SetAttackType(EnemyType.ENEMY_TRUNK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_SurroundStrategy.Composite();
        }
        else if (_index == 8)//4~
        {
            for (int i = 0; i < 2; ++i)
            {
                m_individualStrategy.SetAttackType(EnemyType.ENEMY_ADULT_ROCK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize, 30);
                m_individualStrategy.Composite();
            }

            m_SurroundStrategy.SetAttackType(EnemyType.ENEMY_TRUNK, m_cameraMove.cameraDataSet.cameraSet[(int)m_cameraMove.currentCamera].cameraSize);
            m_SurroundStrategy.Composite();
        }
    }

    public void ReleaseBoss()
    {
        StartCoroutine(StartBoss());
    }

    IEnumerator StartBoss()
    {
        Withdraw();

        Vector3 spawnVctor = playerTransform.transform.position + new Vector3(0f, 2f, 0f);

        GameObject _effect = m_bossManager.GetBossSpawnEffect(spawnVctor);
        _effect.SetActive(true);

        Transform _effectTrans = _effect.transform;

        yield return new WaitForSeconds(3f);

        //Boss Health Bar UI  
        StageUIManager.Instance.SetUIPlayModeType(1, currentRoundIndex);

        //Boss Spawn
        switch (currentRoundIndex)
        {
            case 0:
                //첫번째 보스
                m_bossManager.SpawnBoss(BossType.BOSS_GHOST, _effectTrans.position);
                break;
            case 1:
                //두번째 보스
                m_bossManager.SpawnBoss(BossType.BOSS_NINJAFROG, _effectTrans.position);
                break;
            case 2:
                //세번째 보스
                m_bossManager.SpawnBoss(BossType.BOSS_BUNNY, _effectTrans.position);
                break;
        }
        
        yield return new WaitForSeconds(0.1f);

        StageManager.SetPlayState(PlayState.BossCombat);
    }


    public void GetStageRoundIndex(int _roundIndex)
    {
        currentRoundIndex = _roundIndex;
    }

    //공격 start & stop 
    public void CommandRush(bool _isRush = true) 
    {
        isRush = _isRush;
    }

    //철수
    public void Withdraw() 
    {
        isRush = false;

        EnemySpawner.Instance.Reset();

       // PickupSpawner.Instance.Reset();
    }
}
