using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState
{
    Ready, //ù ���� �غ�, ����� �غ�(+������ ������ ����) , �� Round ������ ����
    NormalBattle,// Active LapTime & Spawn
    BossCombat, // Deactive LapTime & Spawn
    End,  
}
// 1. ������ �������� 
// 1-1 �������� ��������(< maxRound): StartCoroutine(m_currentRound) ���� , 
// 1-2. �������� ��������(= maxRound, �� ���� Ŭ����): ������ �÷��̾� ������ ���ƾ���, Result UI + Jason, Score

// 2. �׾����� :��� Player������ ���ƾ���, ReStart UI, ���� Pause
// [����] 

// ü��, ����, �ڼ�, ��ź => Stage �ð��� �����ϰ� ������ Spawn(Not Boss����)

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
    [SerializeField] int currentRound = 0; //�ν�����â���� ������ ���� ��

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

        //�÷��̾� ������ ��� ����
        m_playerMove.SetPlayerMovementType(PlayerMovementState.Moveable);

        //Enemy ���� �� ����Index �Ѱ��ֱ�
        m_platoonManager.GetStageRoundIndex(_roundIndex);

        //Pickup ���� �� ����Index �Ѱ��ֱ�
        m_pickupManager.GetStageRoundIndex(_roundIndex);

        //Timer���� �� ����Index �Ѱ��ֱ�
        m_gameManager.GetStageRoundIndex(_roundIndex);

        if (_roundIndex == 0)
        {
            m_playerWeaponController.SetPlayerWeaponData();

            m_cameraControl.SetCameraType(CameraType.Default_cameraSize_5);

            m_gameManager.StageReset();//time & type ����

            m_pickupManager.InitialSupportSpawn();
        }

        ++m_currentRound;
        currentRound = m_currentRound;

        yield return new WaitForSeconds(1.0f);

        SetPlayState(PlayState.NormalBattle);

        //Enemy ����
        m_platoonManager.CommandRush(true); 

        //Pickup Item ����
        m_pickupManager.CommandRush(true);
    }

    public void StartBossBattle() // 5��/10��/15�� �� �Ǿ����� (In Update)
    {
        SetPlayState(PlayState.Ready);

        m_platoonManager.ReleaseBoss();
    }

    public void OnStageRoundCleared() //����óġ�� 
    {
        if (m_currentRound < m_maxRound)
        {
            StartCoroutine(StartStage(m_currentRound)); //���� Round 
            return;
        }

        GameWin(); 
    }

    void GameWin()//������ ������ óġ�Ѱ��
    {
        SetPlayState(PlayState.End);

        //GameResultUIManager

        //ScoreManager

        Debug.Log("YOU Wine!");

        //joystic GamObject Deactive
    }

    void GameFail() //�״°�� 
    {
        SetPlayState(PlayState.End);

        Debug.Log("YOU LOST");
        m_playerMove.SetPlayerMovementType(PlayerMovementState.NonMoveable);

        //�׾����� : ReStart UI(�״�� ��Ȱ & ó������ ���� �ٽ� ����) , ���� Pause
    }

    void OnRestartGame() //�ٽ� ���� ��ư�� ������ ��� & �״� ���
    {
        //m_currentRound ����
        //�ð� ����
        //uiī��Ʈ ����
        //�÷��̾� ����
        //ī�޶� ����
        //Platton����
    }

    public static void SetPlayState(PlayState _playState)
    {
        //������ ������ ���� ���� || ������¿��� ������ ����
        bool settingTime_Boss = (playState == PlayState.NormalBattle && _playState == PlayState.Ready) ||
        (playState == PlayState.Ready && _playState == PlayState.BossCombat);

        //����� ���� �� ���� ���� ||���� ���¿��� ����� ����
        bool settingTime_NomalBattle = (playState == PlayState.BossCombat && _playState == PlayState.Ready) ||
            (playState == PlayState.Ready && _playState == PlayState.NormalBattle);

        playState = _playState;

        if (settingTime_Boss || settingTime_NomalBattle)
        {
            //������ ������ ���� ���� ||����� ���� �� ���� ����
            if (playState == PlayState.Ready)
                ComboSkillManager.Instance.CheckStagePlayState(true);

            //������¿��� ������ ���� ||���� ���¿��� ����� ����
            if (playState == PlayState.BossCombat || playState == PlayState.NormalBattle)
                ComboSkillManager.Instance.CheckStagePlayState(false);
        }
    }
}
