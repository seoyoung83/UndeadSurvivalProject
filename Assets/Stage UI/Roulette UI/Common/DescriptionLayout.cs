using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct DisplayItemDescriptionLayout
{
    public Sprite _Sprite;
    public string _TiltleText;
    public string _DescriptionText;
}
public class DescriptionLayout : MonoBehaviour
{
    [SerializeField] GameObject yellowSqaureLine;

    [SerializeField] Image itemImage;

    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;

    [SerializeField] GameObject[] starObject;
    int starCount;

    public void RegisterDisplayLayoutData(DisplayItemDescriptionLayout _display, int _starCount)
    {
        itemImage.sprite = _display._Sprite;

        itemNameText.text = "" + _display._TiltleText;

        itemDescriptionText.text = "" + _display._DescriptionText;

        starCount = _starCount;
    }

    private void OnEnable()
    {
        StartCoroutine(StartEffect());

        for (int i = 0; i < starCount; ++i)
            starObject[i].SetActive(true);
    }

    IEnumerator StartEffect()
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.unscaledDeltaTime * 0.5f;
            yellowSqaureLine.GetComponent<Image>().color = new Color(1, 1, 1, 1 - alpha);
            yield return null;
        }
        yellowSqaureLine.SetActive(false);
    }
}
