using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIManager : MonoBehaviour
{
    public static StageUIManager Instance;

    ScoreManager m_scoreManager;
    GameManager m_gameManager;

    [Header("StageTime UI")]
    [SerializeField] TextMeshProUGUI stageTimeText;
    float currentStageTime;

    [Header("Gauge Bar_Player Skill Update Bar UI")]
    [SerializeField] GameObject playerSkillUpgradeGaugeObject;
    [SerializeField] Image fillmountPlayerSkillUpgradeBar;
    //이펙트용
    //업그레이드 단계 Text

    [Header("Gauge Bar_Boss HP UI & Boss Name")]
    [SerializeField] GameObject bossHpGaugeObject;
    [SerializeField] Image fillmountBossHpBar;
    string[] bossNames = { "고스트", "닌자 개구리", "돌격 토끼" };
    [SerializeField] TextMeshProUGUI bossName;

    [Header("Enemy Kill Count UI")]
    [SerializeField] TextMeshProUGUI enemyKillCountText;

    [Header("Coins Count UI")]
    [SerializeField] TextMeshProUGUI coinCountText;

    [Header("Bomb Effect Image UI")]
    [SerializeField] Image bombEffectImage;

    [Header("Warnning Display Message")]
    [SerializeField] GameObject displayMessageUIGameObject;
    [SerializeField] GameObject displayWarnningIcon;
    [SerializeField] RectTransform textGroup;
    [SerializeField] RectTransform displayTextBackground;
    [SerializeField] TextMeshProUGUI displayText;
    string[] displayMessage = { "좀비가 몰려옵니다!", "보스 습격!" };

    [Header("Skill Fuel")]
    [SerializeField] Image fillmountSkillUpGaugeBar;
    [SerializeField] TextMeshProUGUI currentSkillFuelGaugeBarLevel;
    [SerializeField] Image fillmountSkillUpGaugeBar_Effect;
    Color[] levelupEffectColor = { new Color(0, 1, 1), new Color(0, 0.5f, 1), new Color(0, 0, 1), new Color(0.5f, 0.5f, 1), new Color(0.5f, 0, 1), new Color(1, 0, 1), new Color(1, 0.5f, 1) };
    bool isFuelGaugeBatEffectActive = false;

    [Header("Skill Select UI")]
    int openSkillSelectUICount;

    [Header("Orientation Icon UI")]
    [SerializeField] Transform orientationIconUITrans;
    [SerializeField] GameObject[] orientaionPrefab;

    private void Awake()
    {
        Instance = this;

        currentStageTime = GameManager.Instance.StageTime;
    }

    private void Start()
    {
        m_gameManager = GameObject.FindObjectOfType<GameManager>();

        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        currentSkillFuelGaugeBarLevel.text = "0";

        fillmountSkillUpGaugeBar_Effect.gameObject.SetActive(false);

        displayMessageUIGameObject.SetActive(false);
    }

    private void Update()
    {
        //게임 타이머 업데이트
        UpdateStageTimer();

        //스킬바 업데이트
        UpdateSkillFuelGaugeBarUI();
    }

    //게임 타이머 업데이트
    private void UpdateStageTimer()
    {
        currentStageTime = GameManager.Instance.StageTime;
        stageTimeText.text = GetTimeString(currentStageTime);
    }

    string GetTimeString(float _time)
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(_time); //TimeSpan:시간표현,DateTime(년,월,일,시간 등)
        return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }

    // UI 타입 (보스 모드 & 노멀 플레이 모드)
    public void SetUIPlayModeType(int _number, int roundIndex)
    {
        switch (_number)
        {
            case 0:
                playerSkillUpgradeGaugeObject.SetActive(true);
                bossHpGaugeObject.SetActive(false);

                stageTimeText.gameObject.SetActive(true);
                bossName.gameObject.SetActive(false);
                break;
            case 1://Boss Mode
                playerSkillUpgradeGaugeObject.SetActive(false);
                bossHpGaugeObject.SetActive(true);

                stageTimeText.gameObject.SetActive(false);
                bossName.gameObject.SetActive(true);
                bossName.text = bossNames[roundIndex];
                break;
        }
    }

    //보스 Hp 바 업데이트
    public void UpdateBossHealthBar(float _changeValue, float _maxHp)
    {
        fillmountBossHpBar.fillAmount = _changeValue / _maxHp;
    }

    //처치한 적 카운트 업데이트
    public void UpdateKillCountText(int _killCount)
    {
        enemyKillCountText.text = "" + _killCount.ToString();
    }

    //코인 카운트 업데이트 & 효과
    public void UpdateCoinCountText(int _coindCount)
    {
        int fontSize = 25;
        coinCountText.text = "" + _coindCount.ToString();

        StartCoroutine(StartCoinTextEffect(fontSize));
    }

    IEnumerator StartCoinTextEffect(int _fontSize)
    {
        float basicSize = _fontSize;

        while (basicSize < _fontSize + 7)
        {
            basicSize += Time.deltaTime * 40;

            coinCountText.fontSize = basicSize;

            yield return null;
        }

        float maxSize = basicSize + 7;

        while (maxSize > _fontSize)
        {
            maxSize -= Time.deltaTime * 30;

            coinCountText.fontSize = maxSize;

            yield return null;
        }

        coinCountText.fontSize = _fontSize;
    }

    //폭탄 이펙트
    public void GetPickupBombEffect()
    {
        StartCoroutine(StartBombEffect());
    }

    IEnumerator StartBombEffect()
    {
        float timer_plus = 0;
        while (timer_plus < 0.9f)
        {
            timer_plus += Time.deltaTime * 3f;
            bombEffectImage.color = new Color(1f, 1f, 1f, timer_plus);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        float timer_minus = 0;
        while (timer_minus > 0)
        {
            timer_minus -= Time.deltaTime * 0.01f;
            bombEffectImage.color = new Color(1f, 1f, 1f, timer_minus);
            yield return null;
        }

        bombEffectImage.color = new Color(1f, 1f, 1f, 0);
    }

    //경고창 디스플레이
    public void DisplayWarninngMessage(int _messageNumber)
    {
        textGroup.localScale = Vector3.one;

        displayTextBackground.localPosition = Vector3.one;
        displayTextBackground.GetComponent<Image>().color = Color.red;

        displayText.fontSize = 47;
        displayText.text = "" + displayMessage[_messageNumber].ToString();
        displayText.color = Color.white;

        displayWarnningIcon.GetComponent<RectTransform>().localScale = Vector3.one;
        displayWarnningIcon.GetComponent<Image>().color = Color.white;

        StartCoroutine(StartDisplayWarninngMessage());
    }

    IEnumerator StartDisplayWarninngMessage()
    {
        displayMessageUIGameObject.SetActive(true);

        AudioManager.Instance.WarnningDisplayUIOpen();

        //등장
        float firstTimer = 0;
        while (firstTimer < 1)
        {
            firstTimer += Time.deltaTime * 7;

            displayWarnningIcon.GetComponent<RectTransform>().localScale = new Vector3(2f - firstTimer, 2f - firstTimer, 0);
            displayWarnningIcon.GetComponent<Image>().color = new Color(1f, 1f, 1f, firstTimer);

            displayText.fontSize = 47 * firstTimer;

            displayTextBackground.localPosition = new Vector3(-400f + (firstTimer * 400f), 0, 0);
            yield return null;
        }

        displayText.fontSize = 47;
        displayTextBackground.localPosition = Vector3.one;
        displayWarnningIcon.GetComponent<RectTransform>().localScale = Vector3.one;
        displayWarnningIcon.GetComponent<Image>().color = Color.white;

        //Bounce
        float secondTimer = 0;

        bool isBigger = true;
        float scaleTimer = 0;

        while (secondTimer < 4f)
        {
            secondTimer += Time.deltaTime;

            if (isBigger)
                scaleTimer += Time.deltaTime * 0.8f;
            else
                scaleTimer -= Time.deltaTime * 0.8f;

            if (scaleTimer >= 0.1f)
                isBigger = false;
            else if (scaleTimer <= 0)
                isBigger = true;

            textGroup.localScale = new Vector3(1 + scaleTimer, 1 + scaleTimer, 0);

            yield return null;
        }

        textGroup.localScale = Vector3.one;

        //퇴장
        float thirdTimer = 0;
        while (thirdTimer < 1f)
        {
            thirdTimer += Time.deltaTime * 8;

            textGroup.localScale = new Vector3(1 + thirdTimer, 1 + thirdTimer, 0);

            displayTextBackground.GetComponent<Image>().color = new Color(1, 0, 0, 1 - thirdTimer);

            displayText.color = new Color(1, 1, 1, 1 - thirdTimer);

            displayWarnningIcon.GetComponent<RectTransform>().localScale = new Vector3(1 + thirdTimer, 1 + thirdTimer, 0);
            displayWarnningIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1 - thirdTimer);
            yield return null;
        }

        displayMessageUIGameObject.SetActive(false);
    }

    //스킬바 업데이트
    void UpdateSkillFuelGaugeBarUI()
    {
        //  float value = m_scoreManager.currentSkillFuelGaugeBarValue / m_scoreManager.currentSkillFuelGaugeBarLevelMaxValue;
        fillmountSkillUpGaugeBar.fillAmount = Mathf.Lerp(fillmountSkillUpGaugeBar.fillAmount, m_scoreManager.SkillFuelGauge, 0.05f);
    }

    //스킬 선택창 비활성
    public void CloseSkillSelectUI()
    {
        isFuelGaugeBatEffectActive = false;
    }

    //스킬 선택창 활성화 _스킬바 레벨업 시, 이펙트
    public IEnumerator StartOpenSkillSelectUI(int upgradLevel)
    {
        m_gameManager.PauseTime();
     
        isFuelGaugeBatEffectActive = true;
        
        //스킬 선택창 Active
        SkillSelectUIMenuManager.Instance.OpenSkillSelectUI(true);

        //스킬 게이지 바 현재 레벨_텍스트 효과
        StartCoroutine(EffectSkillFuelLevelText(upgradLevel));

        fillmountSkillUpGaugeBar_Effect.fillAmount = fillmountSkillUpGaugeBar.fillAmount;
        fillmountSkillUpGaugeBar_Effect.gameObject.SetActive(true);

        //게이지바 색상 핑퐁 효과
        float pingpongCheckingTimer = 0;
        while (isFuelGaugeBatEffectActive)
        {
            pingpongCheckingTimer += Time.unscaledDeltaTime;

            if (fillmountSkillUpGaugeBar_Effect.fillAmount < 1)
                fillmountSkillUpGaugeBar_Effect.fillAmount = pingpongCheckingTimer * 5;

            if (pingpongCheckingTimer > 1)
                pingpongCheckingTimer = 0;

            float _value = Mathf.PingPong(pingpongCheckingTimer * 7f, levelupEffectColor.Length);
            fillmountSkillUpGaugeBar_Effect.color = levelupEffectColor[(int)_value];


            yield return null;
        }

        fillmountSkillUpGaugeBar_Effect.gameObject.SetActive(false);

        //스킬 선택창 Deactive
        SkillSelectUIMenuManager.Instance.OpenSkillSelectUI(false);

        m_gameManager.ResumeTime();

        yield return null;
    }

    IEnumerator EffectSkillFuelLevelText(int upgradLevel)
    {
        currentSkillFuelGaugeBarLevel.text = "" + upgradLevel.ToString();
     
        float basicSize = 50;

        while (basicSize < 85)
        {
            basicSize += Time.unscaledDeltaTime * 80;

            currentSkillFuelGaugeBarLevel.fontSize = basicSize;

            yield return null;
        }

        float maxSize = 85;

        while (maxSize > 50)
        {
            maxSize -= Time.unscaledDeltaTime * 70;

            currentSkillFuelGaugeBarLevel.fontSize = maxSize;

            yield return null;
        }

        currentSkillFuelGaugeBarLevel.fontSize = 50;
    }

    public void AddOrientationIconUI(bool isEnemyOrientationIcon, GameObject _thisObject)
    {
        GameObject icon;
        if (isEnemyOrientationIcon)
            icon = GameObject.Instantiate(orientaionPrefab[0]);
        else
            icon = GameObject.Instantiate(orientaionPrefab[1]);

        icon.GetComponent<OrientationIcon>().SetInfo(_thisObject, isEnemyOrientationIcon);
        icon.transform.SetParent(orientationIconUITrans);
        icon.SetActive(true);
    }
}