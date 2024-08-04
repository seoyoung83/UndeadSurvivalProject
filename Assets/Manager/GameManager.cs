using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;

    CameraMove m_cameraMove;

    StageManager m_stageManager;

    StageUIManager m_stageUIManager;

    ObjectOptimizationManager m_objectOptimizationManager;

    bool isPaused = false;
    float savedTimeScale;

    [SerializeField] float stageTime = 0f;
    int currentRoundIndex;

    private void Awake()
    {
        Instance = this;

        m_stageManager = GameObject.FindObjectOfType<StageManager>();

        m_cameraMove = GameObject.FindObjectOfType<CameraMove>();

        m_stageUIManager = GameObject.FindObjectOfType<StageUIManager>();

        m_objectOptimizationManager = GameObject.FindObjectOfType<ObjectOptimizationManager>();
    }

    public float StageTime 
    {
        get
        {
            return stageTime;
        }
    }

    private void Update() 
    {
        if(StageManager.playState == PlayState.NormalBattle)
        {
            stageTime += Time.deltaTime;

            switch (currentRoundIndex)
            {
                case 0: // 1 ROUND 
                    if (stageTime > 179.9f - 5f && stageTime < 180.01f - 5f) // 3�� - 5�� [  Display message  "Warning" ] 
                    {
                        m_stageUIManager.DisplayWarninngMessage(0);
                    }
                    else if (stageTime > 179.9f && stageTime < 180.01f) // 3�� [ Warning ]
                    {
                        m_cameraMove.SetCameraType(CameraType.Middle_cameraSize_6);
                    }
                    else if (stageTime > 299.9f - 5f && stageTime < 300.01f - 5f) // 5��- 5�� [  Display message  ]
                    {
                        m_stageUIManager.DisplayWarninngMessage(1);
                    }
                    else if (stageTime > 299.9f && stageTime < 300.01f) // 5�� [ Boss ]
                    {
                        StartBossCombat();
                        stageTime =300; 
                    }
                    break;
                case 1: // 2 ROUND 
                    if (stageTime > 479.9f - 5f && stageTime < 480.01f - 5f) // 8�� - 5��  [  Display message  "Warning" ] 
                    {
                        m_stageUIManager.DisplayWarninngMessage(0);
                    }
                    else if (stageTime > 479.9f && stageTime < 480.01f) 
                    {
                        m_cameraMove.SetCameraType(CameraType.Last_cameraSize_7);
                    }
                    else if (stageTime > 599.9f - 5f && stageTime < 600.01f - 5f) // 10�� - 5��  [  Display message  ]
                    {
                        m_stageUIManager.DisplayWarninngMessage(1);
                    }
                    else if (stageTime > 599.9f && stageTime < 600.01f) // 10�� [ Boss ]
                    {
                        StartBossCombat();
                        stageTime = 600;
                    }
                    break;
                case 2: // 3 ROUND 
                    if (stageTime > 899.9f - 5f && stageTime < 900.01f - 5f) // 15�� - 5��  [  Display message  ]
                    {
                        m_stageUIManager.DisplayWarninngMessage(1);
                    }
                    else if (stageTime > 899.9f && stageTime < 900.01f) // 15�� [ Boss ]
                    {
                        StartBossCombat();
                        stageTime = 900;
                    }
                    break;
            }
        }   
    }

    public void StageReset() //ù �����Ҷ�, ������Ҷ� �ʱ�ȭ
    {
        stageTime = 0f;
    }

    public void GetStageRoundIndex(int _index) //Round �޶����� �� �޾ƿ���
    {
        currentRoundIndex = _index;

        //UIȰ��ȭ(�Ϲ��÷���&����)
        m_stageUIManager.SetUIPlayModeType(0, currentRoundIndex);
    }

    public  void PauseTime()
    {
        if (!isPaused)
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void ResumeTime()
    {
        if (isPaused)
        {
            Time.timeScale = savedTimeScale;
            isPaused = false;
        }
    }

    void StartBossCombat()
    {
        m_stageManager.StartBossBattle();

        m_objectOptimizationManager.DeleteUnusedObjects(currentRoundIndex);
    }
}
