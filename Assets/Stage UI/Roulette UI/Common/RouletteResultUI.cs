using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RouletteResultUI : MonoBehaviour
{
    public static RouletteResultUI Instance;

    DisplayItemDescriptionLayout m_itemDescriptionLayout;

    [SerializeField] Button rouletteEndButton; //룰렛 종료 버튼

    List<GameObject> selectedItemList = new List<GameObject>();

    [SerializeField] GameObject[] itemDescriptionLayoutPrefab;
    [SerializeField] Transform[] itemLayoutGroupTransform;

    delegate GameObject LayoutGroupTransformDelegate();
    LayoutGroupTransformDelegate CurrentItemLayoutGroupTransform;

    List<GameObject> itemDescriptionLayoutList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OpenRouletteResultUI(bool _open)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(_open);

        if (_open)
        {
            StartCoroutine(StartEffect());

            MoveToLayoutGroupTransform();
        }
        else
        {
            AudioManager.Instance.OnClickButtonAudioEvent();

            rouletteEndButton.gameObject.SetActive(false);

            CurrentItemLayoutGroupTransform().SetActive(false);

            //리스트 리셋
            DestroyAllOfList();
        }
    }

    IEnumerator StartEffect()
    {
        float alpha = 0;
        while (alpha < 0.5f)
        {
            alpha += Time.unscaledDeltaTime;
            gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
    }

    public void CreatSelectedItem(GameObject _replicatItem ,Vector3 _wantedSpawnVect)
    {
        GameObject item = Instantiate(_replicatItem);
        item.GetComponent<ObjectEffectController>().enabled = false;
        item.transform.position = _wantedSpawnVect;
        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        item.SetActive(false);
        selectedItemList.Add(item);
    }

    void MoveToLayoutGroupTransform()
    {
        for (int i = 0; i < selectedItemList.Count; ++i)
            selectedItemList[i].SetActive(true);

        StartCoroutine(StartMoveToTransform());
    }

    IEnumerator StartMoveToTransform()
    {
        CurrentItemLayoutGroupTransform().SetActive(true);

        //선택된 아이템 오브젝트 이동
        int count = 0;
        while (count < selectedItemList.Count)
        {
            Vector2 wantedVect = CalculationDescriptionLayoutTransform(count);

            RectTransform newItemRectTrans = selectedItemList[count].GetComponent<RectTransform>();
            newItemRectTrans.anchoredPosition = Vector3.Lerp(newItemRectTrans.anchoredPosition, wantedVect, Time.unscaledDeltaTime * 20f);

            float distance = (wantedVect - newItemRectTrans.anchoredPosition).magnitude;

            if (distance < 0.1f)
            {
                AudioManager.Instance.RouletteDescriptioOpen();
                selectedItemList[count].SetActive(false);
                itemDescriptionLayoutList[count].gameObject.SetActive(true);
                count++;
            }
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.3f);

        rouletteEndButton.gameObject.SetActive(true);
    }

    public void CreatDescriptionLayout(int _type, Sprite _sprite, string _itemTitle, string _itemDescription, int _starCount)
    {
        m_itemDescriptionLayout._Sprite = _sprite;
        m_itemDescriptionLayout._TiltleText = _itemTitle;
        m_itemDescriptionLayout._DescriptionText = _itemDescription;

        GameObject newObjective = Instantiate(itemDescriptionLayoutPrefab[_type]);
        newObjective.GetComponent<DescriptionLayout>().RegisterDisplayLayoutData(m_itemDescriptionLayout, _starCount);
        newObjective.SetActive(false);
        itemDescriptionLayoutList.Add(newObjective);

        int transformIndex;

        if (itemDescriptionLayoutList.Count == 1)
            transformIndex = 0;
        else if (itemDescriptionLayoutList.Count > 1 && itemDescriptionLayoutList.Count < 5 )
            transformIndex = 1;
        else
            transformIndex = 2;

        for (int i = 0; i < itemDescriptionLayoutList.Count; ++i)
            itemDescriptionLayoutList[i].transform.SetParent(itemLayoutGroupTransform[transformIndex]);

        CurrentItemLayoutGroupTransform = () => itemLayoutGroupTransform[transformIndex].gameObject;
    }

    Vector2 CalculationDescriptionLayoutTransform(int _index)
    {
        //HUD Canvas (900:1600)

        float layoutHeight = itemDescriptionLayoutList[0].GetComponent<RectTransform>().rect.height;

        if (itemDescriptionLayoutList.Count == 1)
        {
            Vector2 wantedVect = new Vector3(900 / 2, 1600 / 2);
            return wantedVect;
        }
        else if (itemDescriptionLayoutList.Count == 3)
        {
            float wantedY = 1600 / 2 + (layoutHeight * (1 - _index));
            Vector2 wantedVect = new Vector3(900 / 2, wantedY);
            return wantedVect;
        }
        else if (itemDescriptionLayoutList.Count == 5)
        {
            float wantedY = (1600 / 2 + 100) + (layoutHeight * (2 - _index)); 
            Vector2 wantedVect = new Vector3(900 / 2, wantedY);
            return wantedVect;
        }

        return Vector2.zero;
    }

    void DestroyAllOfList()
    {
        foreach (var item in itemDescriptionLayoutList)
            Destroy(item);
        itemDescriptionLayoutList.Clear();

        foreach (var item in selectedItemList)
            Destroy(item);
        selectedItemList.Clear();

    }
}
