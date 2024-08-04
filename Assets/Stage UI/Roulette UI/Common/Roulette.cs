using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class Roulette : MonoBehaviour
{
    public IReadOnlyDictionary<int, JSkillUIData> m_SkillUIDataDictionary { get; set; }

    RouletteResultUI m_rouletteResultUI;

    [SerializeField] protected Transform[] itemSpawnLayoutTransform;

    [Header("About Button")]
    [SerializeField] protected Button rouletteStartButton; //룰렛 돌리기 스타트 버튼
    [SerializeField] protected Button tapToSkipButton; //건너뛰기 버튼 (tabAnywhere)

    protected int rouletteWinningCount; //당첨 갯수
    int[] startIndex;//시작지점

    protected List<GameObject> rouletteItemsList = new List<GameObject>(); //룰렛회전에 들어가는 모든 아이템들

    protected List<int> selectedRouletteBoxIndexList = new List<int>();

    private void Awake()
    {
        m_SkillUIDataDictionary = DataManager.Instance.SkillUIDataDictionary;

        m_rouletteResultUI = GameObject.FindObjectOfType<RouletteResultUI>();
    }

    private void Start()
    {
        rouletteStartButton.gameObject.SetActive(false);
        tapToSkipButton.gameObject.SetActive(false);

        rouletteStartButton.onClick.AddListener(OnClickRouletteStartButton);
        tapToSkipButton.onClick.AddListener(OnClickTapToSkipButton);
    }
    
    // 룰렛 아이템 박스 생성
    public abstract void CreatRouletteItemBox(int _rouletteWinningCount);
   
    // 생성한 룰렛 아이템 박스 세팅
    public void SetRouletteItemBox()
    {
        //선택된 아아템과 그외 배열 배치 섞기
        if (rouletteWinningCount != 5)
        {
            for (int i = 0; i < rouletteWinningCount; ++i)
            {
                int randomIndex = Random.Range(rouletteWinningCount, rouletteItemsList.Count);
                int duplicateCount = selectedRouletteBoxIndexList.Count(selectedRouletteBoxIndex => selectedRouletteBoxIndex == randomIndex);

                while (duplicateCount > 0)
                {
                    // 중복이 발생할 경우 다시 랜덤 인덱스 생성
                    randomIndex = Random.Range(0, rouletteItemsList.Count);
                    duplicateCount = selectedRouletteBoxIndexList.Count(selectedRouletteBoxIndex => selectedRouletteBoxIndex == randomIndex);
                }

                GameObject temp = rouletteItemsList[i].gameObject;
                rouletteItemsList[i] = rouletteItemsList[randomIndex];
                rouletteItemsList[randomIndex] = temp;
                selectedRouletteBoxIndexList.Add(randomIndex); //선택된 아이템 배열 자릿값 넣기
            }
        }
        else
        {
            int randomIndex = Random.Range(rouletteWinningCount, rouletteItemsList.Count); 

            for (int i = 0; i < rouletteWinningCount; ++i)
            {
                GameObject temp = rouletteItemsList[i].gameObject;
                rouletteItemsList[i] = rouletteItemsList[randomIndex];
                rouletteItemsList[randomIndex] = temp;
                selectedRouletteBoxIndexList.Add(randomIndex);

                randomIndex++;
                if (randomIndex >= rouletteItemsList.Count)
                    randomIndex = 0;
            }

        }

        // 아이템 배치
        int transformIndex = -1;
        for (int i = 0; i < rouletteItemsList.Count; ++i)
        {
            if (i % 4 == 0 && transformIndex < itemSpawnLayoutTransform.Length)
                transformIndex++;
            rouletteItemsList[i].transform.SetParent(itemSpawnLayoutTransform[transformIndex]);
            rouletteItemsList[i].transform.localScale = Vector3.one;
        }
    }

    // 룰렛 보상 업데이트 
    public abstract void UpdateReward();

    // 룰렛창 Open & Close
    public void OpenRoulette(bool _open)
    {
        if (_open)
            StartCoroutine(StartRouletteReady());
        else
        {
            //보상 업데이트
            UpdateReward();

            //모든 오브젝트들 삭제
            foreach (var item in rouletteItemsList)
                Destroy(item);

            rouletteItemsList.Clear();
            selectedRouletteBoxIndexList.Clear();
        }
           
    }

    //룰렛 시작 준비 (액티브T)
    IEnumerator StartRouletteReady()
    {
        int count = 0;

        while (count < 8)
        {
            rouletteItemsList[count].SetActive(true);

            yield return new WaitForSecondsRealtime(0.05f);

            rouletteItemsList[count + 8].SetActive(true);

            yield return new WaitForSecondsRealtime(0.05f);
            count++;

            if (count == 4)
                yield return new WaitForSecondsRealtime(0.05f);
        }

        // 룰렛 돌리기 스타트 버튼
        rouletteStartButton.gameObject.SetActive(true);
    }

    //룰렛 스타트(=계속하기) 버튼
    void OnClickRouletteStartButton()
    {
        AudioManager.Instance.OnClickButtonAudioEvent();

        rouletteStartButton.gameObject.SetActive(false);

        StartTextEffect(true);

        ContinueRoulette();
    }

    // LuckySkillRouletteUI 코인 업데이트
    public virtual void StartTextEffect(bool start) { }

    //룰렛 스타트(=계속하기) 
    void ContinueRoulette()
    {
        AudioManager.Instance.RouletteStartSpin();

        if (rouletteWinningCount != 5)
        {
            startIndex = new int[3];

            for (int i = 0; i < 3; ++i)
                startIndex[i] = i * 5;

            StartCoroutine(StartRouletteNormalTypeEffect());
        }
        else
        {
            startIndex = new int[2];
            for (int i = 0; i < 2; ++i)
                startIndex[i] = 10;

            StartCoroutine(StartRouletteSpecialTypeEffect());
        }
    }

    //노멀 이펙트(1개 or 3개)
    IEnumerator StartRouletteNormalTypeEffect()
    {
        //탭 버튼이 활성화 되기전 UI Effect
        float timer_effectBeforeStart = 0;

        while (timer_effectBeforeStart < 0.13f)
        {
            timer_effectBeforeStart += Time.unscaledDeltaTime;

            foreach (int index in startIndex)
                rouletteItemsList[index].GetComponent<RouletteItem>().DoEffect(0);

            for (int i = 0; i < startIndex.Length; i++)
            {
                startIndex[i]++;

                if (startIndex[i] >= rouletteItemsList.Count)
                    startIndex[i] = 0;
            }

            yield return new WaitForSecondsRealtime(0.05f);
        }

        //'아무데나 탬하여 건너뛰기' 활성화
        tapToSkipButton.gameObject.SetActive(true);

        int spinCount = 0;

        int currentSelectCount = 0;
        //탭 버튼이 활성화된 후 UI Effect
        while (currentSelectCount < rouletteWinningCount && tapToSkipButton.gameObject.activeInHierarchy)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            rouletteItemsList[startIndex[0]].GetComponent<RouletteItem>().DoEffect(1);

            startIndex[0]++;

            if (startIndex[0] >= rouletteItemsList.Count)
                startIndex[0] = 0;

            if (selectedRouletteBoxIndexList[currentSelectCount] == startIndex[0])
            {
                spinCount++;

                if (spinCount > 2)
                {
                    rouletteItemsList[selectedRouletteBoxIndexList[currentSelectCount]].GetComponent<RouletteItem>().SelectedRouletteItemEffect(true);

                    spinCount = 0;

                    currentSelectCount++;

                    yield return new WaitForSecondsRealtime(0.5f);
                }
            }   
        }

        float restTime;
        //건너뛰기를 클릭하지 않았을 경우
        if (tapToSkipButton.gameObject.activeInHierarchy)
        {
            restTime = 1.5f;
            tapToSkipButton.gameObject.SetActive(false);
        }
        else //컨너뛰기를 클릭했을 경우
        {
            for (int i = currentSelectCount; i < rouletteWinningCount; ++i)
                rouletteItemsList[selectedRouletteBoxIndexList[i]].GetComponent<RouletteItem>().SelectedRouletteItemEffect(false);

            restTime = 1f;
        }

        yield return new WaitForSecondsRealtime(restTime);

        OpenRouletteResult();
    }

    //스페셜 이펙트(5개)
    IEnumerator StartRouletteSpecialTypeEffect()
    {
        int count = 0;
        while (count < 4)
        {
            foreach (int index in startIndex)
                rouletteItemsList[index].GetComponent<RouletteItem>().DoEffect(0);

            for (int i = 0; i < startIndex.Length; i++)
            {
                startIndex[i] += (i * 2) - 1;

                if (startIndex[i] >= rouletteItemsList.Count)
                    startIndex[i] = 0;

                if (startIndex[i] == 2)
                {
                    count++;

                    for (int j = 0; j < startIndex.Length; ++j)
                        startIndex[j] = 10;
                }
            }
            yield return new WaitForSecondsRealtime(0.065f);
        }

        //'아무데나 탬하여 건너뛰기' 활성화
        tapToSkipButton.gameObject.SetActive(true);

        int spinCount = 0;

        int currentSelectCount = 0;

        //탭 버튼이 활성화된 후 UI Effect
        while (currentSelectCount < rouletteWinningCount && tapToSkipButton.gameObject.activeInHierarchy)
        {
            yield return new WaitForSecondsRealtime(0.15f);

            rouletteItemsList[startIndex[0]].GetComponent<RouletteItem>().DoEffect(1);

            startIndex[0]++;

            if (startIndex[0] >= rouletteItemsList.Count)
                startIndex[0] = 0;

            if (selectedRouletteBoxIndexList[currentSelectCount] == startIndex[0])
            {
                spinCount++;

                if (spinCount > 1)
                {
                    for (int i = 0; i < rouletteWinningCount; ++i)
                    {
                        if (tapToSkipButton.gameObject.activeInHierarchy)
                        {
                            rouletteItemsList[selectedRouletteBoxIndexList[currentSelectCount]].GetComponent<RouletteItem>().SelectedRouletteItemEffect(true);
                            currentSelectCount++;
                            yield return new WaitForSecondsRealtime(0.8f);
                        }
                    }
                    spinCount = 0;
                }
            }
        }

        float restTime;
        //건너뛰기를 클릭하지 않았을 경우
        if (tapToSkipButton.gameObject.activeInHierarchy)
        {
            restTime = 1.5f;
            tapToSkipButton.gameObject.SetActive(false);
        }  
        else //컨너뛰기를 클릭했을 경우
        {
            for (int i = currentSelectCount; i < rouletteWinningCount; ++i)
                rouletteItemsList[selectedRouletteBoxIndexList[i]].GetComponent<RouletteItem>().SelectedRouletteItemEffect(false);

            restTime = 1f;
        }

        yield return new WaitForSecondsRealtime(restTime);

        OpenRouletteResult();
    }

    //룰렛 건너뛰기 버튼
    void OnClickTapToSkipButton()
    {
        tapToSkipButton.gameObject.SetActive(false);

        StartTextEffect(false);
    }

    //룰렛 결과창 Open
    void OpenRouletteResult()
    {
        //선택된 오브젝트 인스턴스
        for (int i = 0; i < selectedRouletteBoxIndexList.Count; ++i)
            m_rouletteResultUI.CreatSelectedItem(rouletteItemsList[selectedRouletteBoxIndexList[i]], rouletteItemsList[selectedRouletteBoxIndexList[i]].transform.position);

        //룰렛 결과창 Open
        m_rouletteResultUI.OpenRouletteResultUI(true);
    }
}
