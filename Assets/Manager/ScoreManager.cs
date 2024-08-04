using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Kill Enemy")]
    int currentEnemyKillCount = 0;

    [Header("Get Coin")]
    int currentCoinCount = 0;
    float buffValue_Coin = 0f;

    [Header("Skill Fuel")]
    [SerializeField] int currentSkillFuelGaugeBarLevel = 0; // 현재 Fuel 레벨
    [SerializeField] float currentSkillFuelGaugeBarValue; //현재  fuel 값
    [SerializeField] float cumulativeSkillFuelValue; //누적 값
    float buffValue_skillFuel = 0f;

    [SerializeField] List<float> maxFuelValueByBarLevelDataList= new List<float>(); //각 레벨별 Max Count 기준 값

    Queue<int> getCoinValueQueue = new Queue<int>(); //얻은 코인value

    Queue<int> upgradeLevelQueue = new Queue<int>(); //스킬바 레벨업에 따른 스킬 레벨업 카운트
    Queue<float> getSkillFuelValueQueue = new Queue<float>(); //얻은 skill fuel Value

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        currentEnemyKillCount = 0;
        currentCoinCount = 0;

        int maxValuebyLevel = 0;

        for (int i = 0; i <= 100; ++i) //미리 만들어두기
        {
            if (i < 5)
                maxValuebyLevel += 5;
            else if (i >= 5 && i < 10)
                maxValuebyLevel += 15;
            else if (i >= 10 && i < 30)
                maxValuebyLevel += 30;
            else if (i >= 30 && i < 60)
                maxValuebyLevel += 50;
            else
                maxValuebyLevel += 80;

            maxFuelValueByBarLevelDataList.Add(maxValuebyLevel);
        }
    }


    private void Update()
    {
        if (getSkillFuelValueQueue.Count > 0)
        {
            float fuelValue = getSkillFuelValueQueue.Dequeue();

            currentSkillFuelGaugeBarValue += fuelValue;

            if (currentSkillFuelGaugeBarValue >= maxFuelValueByBarLevelDataList[currentSkillFuelGaugeBarLevel])
                GetSkIllFuelLevelData();
        }

        if (upgradeLevelQueue.Count > 0)
        {
            StartCoroutine(StartLevelupSkillBar());
        }

        if (getCoinValueQueue.Count > 0)
        {
            int coinValue = getCoinValueQueue.Dequeue();

            currentCoinCount += coinValue;

            StageUIManager.Instance.UpdateCoinCountText(currentCoinCount);
        }
    }

    void GetSkIllFuelLevelData()
    {
        for (int i = currentSkillFuelGaugeBarLevel; i <= maxFuelValueByBarLevelDataList.Count - 1; ++i)
        {
            if (currentSkillFuelGaugeBarValue >= maxFuelValueByBarLevelDataList[currentSkillFuelGaugeBarLevel])
            {
                float tempValue = currentSkillFuelGaugeBarValue - maxFuelValueByBarLevelDataList[i];

                currentSkillFuelGaugeBarValue = tempValue;

                currentSkillFuelGaugeBarLevel++;

                upgradeLevelQueue.Enqueue(currentSkillFuelGaugeBarLevel);
            }
        }
    }

    IEnumerator StartLevelupSkillBar()
    {
        while (upgradeLevelQueue.Count > 0)
        {
            int level = upgradeLevelQueue.Dequeue();
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(StageUIManager.Instance.StartOpenSkillSelectUI(level));
        }
    }



    public float GetSkillFuelGauge()
    {
        return currentSkillFuelGaugeBarValue / maxFuelValueByBarLevelDataList[currentSkillFuelGaugeBarLevel];
    }

    public float SkillFuelGauge
    {
        get { return currentSkillFuelGaugeBarValue / maxFuelValueByBarLevelDataList[currentSkillFuelGaugeBarLevel]; }
    }


    public void UpdateEnemyKillCount(int _count)
    {
        currentEnemyKillCount = _count;

        StageUIManager.Instance.UpdateKillCountText(currentEnemyKillCount);
    }
   
    public void UpdateCoinCount(int _value)
    {
        int coin_buff = (int)Math.Round(_value + (_value * buffValue_Coin / 100));

        getCoinValueQueue.Enqueue(coin_buff);
    }

    public void UpdateSkillFuel(float _value)
    {
        float skillFuel_buff = _value + (_value * buffValue_skillFuel / 100);

        getSkillFuelValueQueue.Enqueue(skillFuel_buff);
    }

    public void DoPassiveBuff(PlayerSkillCategory _type, float _value)
    {
        switch (_type)
        {
            case PlayerSkillCategory.PASSIVE_NINJASCROLL: // Exp (보석 연료) 획득 +N%
                buffValue_skillFuel = _value;
                break;
            case PlayerSkillCategory.PASSIVE_OILBONDS: // 골드획득 +N%
                buffValue_Coin = _value;
                break;
        }
    }

    
}
