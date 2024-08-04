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
    [SerializeField] protected Button rouletteStartButton; //�귿 ������ ��ŸƮ ��ư
    [SerializeField] protected Button tapToSkipButton; //�ǳʶٱ� ��ư (tabAnywhere)

    protected int rouletteWinningCount; //��÷ ����
    int[] startIndex;//��������

    protected List<GameObject> rouletteItemsList = new List<GameObject>(); //�귿ȸ���� ���� ��� �����۵�

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
    
    // �귿 ������ �ڽ� ����
    public abstract void CreatRouletteItemBox(int _rouletteWinningCount);
   
    // ������ �귿 ������ �ڽ� ����
    public void SetRouletteItemBox()
    {
        //���õ� �ƾ��۰� �׿� �迭 ��ġ ����
        if (rouletteWinningCount != 5)
        {
            for (int i = 0; i < rouletteWinningCount; ++i)
            {
                int randomIndex = Random.Range(rouletteWinningCount, rouletteItemsList.Count);
                int duplicateCount = selectedRouletteBoxIndexList.Count(selectedRouletteBoxIndex => selectedRouletteBoxIndex == randomIndex);

                while (duplicateCount > 0)
                {
                    // �ߺ��� �߻��� ��� �ٽ� ���� �ε��� ����
                    randomIndex = Random.Range(0, rouletteItemsList.Count);
                    duplicateCount = selectedRouletteBoxIndexList.Count(selectedRouletteBoxIndex => selectedRouletteBoxIndex == randomIndex);
                }

                GameObject temp = rouletteItemsList[i].gameObject;
                rouletteItemsList[i] = rouletteItemsList[randomIndex];
                rouletteItemsList[randomIndex] = temp;
                selectedRouletteBoxIndexList.Add(randomIndex); //���õ� ������ �迭 �ڸ��� �ֱ�
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

        // ������ ��ġ
        int transformIndex = -1;
        for (int i = 0; i < rouletteItemsList.Count; ++i)
        {
            if (i % 4 == 0 && transformIndex < itemSpawnLayoutTransform.Length)
                transformIndex++;
            rouletteItemsList[i].transform.SetParent(itemSpawnLayoutTransform[transformIndex]);
            rouletteItemsList[i].transform.localScale = Vector3.one;
        }
    }

    // �귿 ���� ������Ʈ 
    public abstract void UpdateReward();

    // �귿â Open & Close
    public void OpenRoulette(bool _open)
    {
        if (_open)
            StartCoroutine(StartRouletteReady());
        else
        {
            //���� ������Ʈ
            UpdateReward();

            //��� ������Ʈ�� ����
            foreach (var item in rouletteItemsList)
                Destroy(item);

            rouletteItemsList.Clear();
            selectedRouletteBoxIndexList.Clear();
        }
           
    }

    //�귿 ���� �غ� (��Ƽ��T)
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

        // �귿 ������ ��ŸƮ ��ư
        rouletteStartButton.gameObject.SetActive(true);
    }

    //�귿 ��ŸƮ(=����ϱ�) ��ư
    void OnClickRouletteStartButton()
    {
        AudioManager.Instance.OnClickButtonAudioEvent();

        rouletteStartButton.gameObject.SetActive(false);

        StartTextEffect(true);

        ContinueRoulette();
    }

    // LuckySkillRouletteUI ���� ������Ʈ
    public virtual void StartTextEffect(bool start) { }

    //�귿 ��ŸƮ(=����ϱ�) 
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

    //��� ����Ʈ(1�� or 3��)
    IEnumerator StartRouletteNormalTypeEffect()
    {
        //�� ��ư�� Ȱ��ȭ �Ǳ��� UI Effect
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

        //'�ƹ����� ���Ͽ� �ǳʶٱ�' Ȱ��ȭ
        tapToSkipButton.gameObject.SetActive(true);

        int spinCount = 0;

        int currentSelectCount = 0;
        //�� ��ư�� Ȱ��ȭ�� �� UI Effect
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
        //�ǳʶٱ⸦ Ŭ������ �ʾ��� ���
        if (tapToSkipButton.gameObject.activeInHierarchy)
        {
            restTime = 1.5f;
            tapToSkipButton.gameObject.SetActive(false);
        }
        else //���ʶٱ⸦ Ŭ������ ���
        {
            for (int i = currentSelectCount; i < rouletteWinningCount; ++i)
                rouletteItemsList[selectedRouletteBoxIndexList[i]].GetComponent<RouletteItem>().SelectedRouletteItemEffect(false);

            restTime = 1f;
        }

        yield return new WaitForSecondsRealtime(restTime);

        OpenRouletteResult();
    }

    //����� ����Ʈ(5��)
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

        //'�ƹ����� ���Ͽ� �ǳʶٱ�' Ȱ��ȭ
        tapToSkipButton.gameObject.SetActive(true);

        int spinCount = 0;

        int currentSelectCount = 0;

        //�� ��ư�� Ȱ��ȭ�� �� UI Effect
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
        //�ǳʶٱ⸦ Ŭ������ �ʾ��� ���
        if (tapToSkipButton.gameObject.activeInHierarchy)
        {
            restTime = 1.5f;
            tapToSkipButton.gameObject.SetActive(false);
        }  
        else //���ʶٱ⸦ Ŭ������ ���
        {
            for (int i = currentSelectCount; i < rouletteWinningCount; ++i)
                rouletteItemsList[selectedRouletteBoxIndexList[i]].GetComponent<RouletteItem>().SelectedRouletteItemEffect(false);

            restTime = 1f;
        }

        yield return new WaitForSecondsRealtime(restTime);

        OpenRouletteResult();
    }

    //�귿 �ǳʶٱ� ��ư
    void OnClickTapToSkipButton()
    {
        tapToSkipButton.gameObject.SetActive(false);

        StartTextEffect(false);
    }

    //�귿 ���â Open
    void OpenRouletteResult()
    {
        //���õ� ������Ʈ �ν��Ͻ�
        for (int i = 0; i < selectedRouletteBoxIndexList.Count; ++i)
            m_rouletteResultUI.CreatSelectedItem(rouletteItemsList[selectedRouletteBoxIndexList[i]], rouletteItemsList[selectedRouletteBoxIndexList[i]].transform.position);

        //�귿 ���â Open
        m_rouletteResultUI.OpenRouletteResultUI(true);
    }
}
