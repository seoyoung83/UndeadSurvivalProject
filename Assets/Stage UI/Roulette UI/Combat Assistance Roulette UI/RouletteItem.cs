using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RouletteItem : MonoBehaviour
{
    [SerializeField] GameObject yellowSquareLine;
    [SerializeField] GameObject secondBackground;

    [SerializeField] Image itemImage;


    public void InitializedDataForRouletteItem(Sprite _sprite)
    {
        yellowSquareLine.SetActive(false);
        secondBackground.SetActive(false);

        itemImage.sprite = _sprite;
    }

    public void DoEffect(int _type)
    {
        StartCoroutine(StartYellowSquareLineEffect(_type));
    }

    IEnumerator StartYellowSquareLineEffect(int _type)
    {
        yellowSquareLine.SetActive(true);
        AudioManager.Instance.RouletteItemAudioEvent(false);

        float timer = 0;
        if (_type == 0) //빠르게 
        {
            yellowSquareLine.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            while (timer < 0.05f)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        else //천천히 (결과: 그라데이션)
        {
            while (timer < 1)
            {
                timer += Time.unscaledDeltaTime * 4f;

                yellowSquareLine.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1 - timer);

                yield return null;
            }
        }

        yellowSquareLine.SetActive(false);
        yield return null;
    }


    public void SelectedRouletteItemEffect(bool _effect)
    {
        StopAllCoroutines();

        StartCoroutine(StartSelectionEffect(_effect));
    }

    IEnumerator StartSelectionEffect(bool _effect) //초록 배경 + 두번 바운스
    {
        yellowSquareLine.SetActive(false);

        secondBackground.SetActive(true);

        if (_effect)
        {
            AudioManager.Instance.RouletteItemAudioEvent(true);

            bool isBigger = true;

            int Count = 0;

            float timer = 1;

            while (Count < 2)
            {
                if (isBigger)
                    timer += Time.unscaledDeltaTime;
                else
                    timer -= Time.unscaledDeltaTime;

                if (timer > 1.2f)
                {
                    isBigger = false;
                }
                else if (timer < 1f)
                {
                    isBigger = true;
                    Count++;
                }

                transform.localScale = new Vector3(timer, timer, timer);

                yield return null;
            }
        }
        //_effect(false): 건너뛰기 했을시
        transform.localScale = new Vector3(1, 1, 1);
    }
}
