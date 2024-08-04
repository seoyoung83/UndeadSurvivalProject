using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteUIManager : MonoBehaviour
{
    public static RouletteUIManager Instance;

    GameManager m_gameManager;
    RouletteResultUI m_rouletteResultUI;

    [SerializeField] GameObject m_CombatAssistanceRoulette;
    [SerializeField] GameObject m_LuckySkillRoulette;

    [SerializeField] Button rouletteEndButton; //�귿 ���� ��ư 

    delegate GameObject CurrentRouletteType();
    CurrentRouletteType CurrentRoulette;

    [SerializeField]int rouletteWinningCount;

    private void Awake()
    {
        Instance = this;

        m_gameManager = GameObject.FindObjectOfType<GameManager>();
        m_rouletteResultUI = GameObject.FindObjectOfType<RouletteResultUI>();
    }

    private void Start()
    {
        rouletteEndButton.onClick.AddListener(OnClickRouletteEndButton);
        
        m_CombatAssistanceRoulette.SetActive(false);
        m_LuckySkillRoulette.SetActive(false);

        //WinningCount ���� �̱�
        int[] winningCountWeightedValue = { 20, 75, 5 };
        rouletteWinningCount = (int)RandomWeightedRouletteItem.GetRouletteWinningCount(winningCountWeightedValue);
    }

    // �귿(& ���â) ����
    public void OpenRoulette(int _type) 
    {
        //�ð� stop
        m_gameManager.PauseTime();

        CurrentRoulette = (_type == 0) ? () => m_CombatAssistanceRoulette.gameObject : () => m_LuckySkillRoulette.gameObject;

        CurrentRoulette().SetActive(true);

        CurrentRoulette().GetComponent<Roulette>().CreatRouletteItemBox(rouletteWinningCount);
        
        CurrentRoulette().GetComponent<Roulette>().OpenRoulette(true);

        AudioManager.Instance.RouletteUIOpen();
    }

    // �귿(& ���â) �ݱ�
    void OnClickRouletteEndButton()
    {
        // �ش� Roulette UI ��Ȱ��ȭ & ����
        CurrentRoulette().SetActive(false);
        CurrentRoulette().GetComponent<Roulette>().OpenRoulette(false);

        // Roulette ���â ��Ȱ��ȭ
        m_rouletteResultUI.OpenRouletteResultUI(false);

        // �ð� FLOW
        m_gameManager.ResumeTime();

        //WinningCount ���� �̱�
        int[] winningCountWeightedValue = { 76, 20, 4 };
        rouletteWinningCount = (int)RandomWeightedRouletteItem.GetRouletteWinningCount(winningCountWeightedValue);
    }

    public void ResetWinningCount() 
    {
        rouletteWinningCount = 1; // ***�Ŀ� ���� ����
        CurrentRoulette().GetComponent<Roulette>().CreatRouletteItemBox(rouletteWinningCount);
    }
}
