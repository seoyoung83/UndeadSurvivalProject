using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState
{
    Ready, //첫 시작 준비, 재시작 준비(+보스전 시작전 레디) , 각 Round 데이터 갱신
    NormalBattle,// Active LapTime & Spawn
    BossCombat, // Deactive LapTime & Spawn
    End,  
}
// 1. 보스전 끝났을때 
// 1-1 보스전이 끝났을때(< maxRound): StartCoroutine(m_currentRound) 시작 , 
// 1-2. 보스전이 끝났을때(= maxRound, 즉 게임 클리어): 몇초후 플레이어 움직임 막아야함, Result UI + Jason, Score

// 2. 죽었을때 :즉시 Player움직임 막아야함, ReStart UI, 게임 Pause
// [공통] 

// 체력, 코인, 자석, 폭탄 => Stage 시간과 무관하게 일정히 Spawn(Not Boss전투)

public class StageManager : MonoBehaviour
{
    static public PlayState playState = PlayState.Ready;

    DataManager m_dataManager;

    PlayerMove m_playerMove;

    PlayerWeaponController m_playerWeaponController;

    PlatoonManager m_platoonManager;

    PickupManager m_pickupManager;

    CameraMove m_cameraControl;

    GameManager m_gameManager;

    static public int m_currentRound = 0;
    [SerializeField] int currentRound = 0; //인스펙터창으로 보려고 만든 거

    int m_maxRound = 3;

    private void Awake()
    {
        m_dataManager = GameObject.FindObjectOfType<DataManager>();
        m_dataManager.LoadData();
    }

    private void Start()
    {
        m_playerMove = GameObject.FindObjectOfType<PlayerMove>();

        m_playerWeaponController = GameObject.FindObjectOfType<PlayerWeaponController>();

        m_gameManager = GameObject.FindObjectOfType<GameManager>();
        
        m_platoonManager = GameObject.FindObjectOfType<PlatoonManager>();

        m_pickupManager = GameObject.FindObjectOfType<PickupManager>();

        m_cameraControl = GameObject.FindObjectOfType<CameraMove>();

        m_dataManager.OnDataLoadComplete += OnDataLoadComplete;
    }

    void OnDataLoadComplete()
    {
        StartCoroutine(StartStage(0));
    }

    IEnumerator StartStage(int _roundIndex)
    {
        SetPlayState(PlayState.Ready);

        //플레이어 움직임 모드 세팅
        m_playerMove.SetPlayerMovementType(PlayerMovementState.Moveable);

        //Enemy 스폰 전 라운드Index 넘겨주기
        m_platoonManager.GetStageRoundIndex(_roundIndex);

        //Pickup 스폰 전 라운드Index 넘겨주기
        m_pickupManager.GetStageRoundIndex(_roundIndex);

        //Timer시작 전 라운드Index 넘겨주기
        m_gameManager.GetStageRoundIndex(_roundIndex);

        if (_roundIndex == 0)
        {
            m_playerWeaponController.SetPlayerWeaponData();

            m_cameraControl.SetCameraType(CameraType.Default_cameraSize_5);

            m_gameManager.StageReset();//time & type 리셋

            m_pickupManager.InitialSupportSpawn();
        }

        ++m_currentRound;
        currentRound = m_currentRound;

        yield return new WaitForSeconds(1.0f);

        SetPlayState(PlayState.NormalBattle);

        //Enemy 스폰
        m_platoonManager.CommandRush(true); 

        //Pickup Item 스폰
        m_pickupManager.CommandRush(true);
    }

    public void StartBossBattle() // 5분/10분/15분 이 되었을때 (In Update)
    {
        SetPlayState(PlayState.Ready);

        m_platoonManager.ReleaseBoss();
    }

    public void OnStageRoundCleared() //보스처치후 
    {
        if (m_currentRound < m_maxRound)
        {
            StartCoroutine(StartStage(m_currentRound)); //다음 Round 
            return;
        }

        GameWin(); 
    }

    void GameWin()//마지막 보스를 처치한경우
    {
        SetPlayState(PlayState.End);

        //GameResultUIManager

        //ScoreManager

        Debug.Log("YOU Wine!");

        //joystic GamObject Deactive
    }

    void GameFail() //죽는경우 
    {
        SetPlayState(PlayState.End);

        Debug.Log("YOU LOST");
        m_playerMove.SetPlayerMovementType(PlayerMovementState.NonMoveable);

        //죽었을때 : ReStart UI(그대로 부활 & 처음부터 게임 다시 시작) , 게임 Pause
    }

    void OnRestartGame() //다시 시작 버튼을 누르는 경우 & 죽는 경우
    {
        //m_currentRound 리셋
        //시간 리셋
        //ui카운트 리셋
        //플레이어 리셋
        //카메라 리셋
        //Platton리셋
    }

    public static void SetPlayState(PlayState _playState)
    {
        //보스전 시작전 레디 돌입 || 레디상태에서 보스전 돌입
        bool settingTime_Boss = (playState == PlayState.NormalBattle && _playState == PlayState.Ready) ||
        (playState == PlayState.Ready && _playState == PlayState.BossCombat);

        //노멀전 시작 전 레디 돌입 ||레디 상태에서 노멀전 돌입
        bool settingTime_NomalBattle = (playState == PlayState.BossCombat && _playState == PlayState.Ready) ||
            (playState == PlayState.Ready && _playState == PlayState.NormalBattle);

        playState = _playState;

        if (settingTime_Boss || settingTime_NomalBattle)
        {
            //보스전 시작전 레디 돌입 ||노멀전 시작 전 레디 돌입
            if (playState == PlayState.Ready)
                ComboSkillManager.Instance.CheckStagePlayState(true);

            //레디상태에서 보스전 돌입 ||레디 상태에서 노멀전 돌입
            if (playState == PlayState.BossCombat || playState == PlayState.NormalBattle)
                ComboSkillManager.Instance.CheckStagePlayState(false);
        }
    }
}
