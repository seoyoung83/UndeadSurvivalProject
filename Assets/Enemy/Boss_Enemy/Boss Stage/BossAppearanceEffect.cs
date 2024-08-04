using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAppearanceEffect : MonoBehaviour
{
    [SerializeField] float effectTime; //3f

    [Header("Red Icon")]
    [SerializeField] GameObject circleLine; //사이즈 &t색상?  사이즈 0.5~1
    float checkingTime_circleLine = 0.2f;
    [SerializeField] GameObject raidIcon;// 2~2.5
    float checkingTime_laidIcon = 2f;
    bool isBigger = true;

    [Header("Red Fence")]
    [SerializeField] SpriteRenderer redFence;
    float checkingTime_redFence = 0f;
    bool isFade = true;

    private void OnDisable()
    {
        circleLine.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        raidIcon.transform.localScale = new Vector3(2, 2, 0);
        redFence.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnEnable()
    {
        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        float checkingTimer = 0;

        while (checkingTimer < effectTime)
        {
            checkingTimer += Time.deltaTime;

            RedIconEffect();

            RedFenceEffect();

            yield return null;
        }

        gameObject.SetActive(false);

        yield return null;
    }

    void RedIconEffect()
    {
        //써클라인
        checkingTime_circleLine += Time.deltaTime *0.7f;

        if (checkingTime_circleLine > 1)
            checkingTime_circleLine = 0.2f;

        circleLine.transform.localScale = new Vector3(checkingTime_circleLine, checkingTime_circleLine, 0);

        //아이콘
        if (checkingTime_laidIcon >= 2.5f)
        {
            isBigger = false;
        }
        else if (checkingTime_laidIcon <= 2)
        {
            isBigger = true;
        }

        if (isBigger)
        {
            checkingTime_laidIcon += Time.deltaTime * 0.5f;
        }
        else
        {
            checkingTime_laidIcon -= Time.deltaTime * 0.5f;
        }

        raidIcon.transform.localScale = new Vector3(checkingTime_laidIcon, checkingTime_laidIcon, 0);
    }

    void RedFenceEffect()
    {
        if (isFade)
        {
            checkingTime_redFence -= Time.deltaTime * 0.5f;
        }
        else
        {
            checkingTime_redFence += Time.deltaTime * 0.5f;
        }

        if (checkingTime_redFence > 0.5f)
        {
            isFade = true;
        }
        else if(checkingTime_redFence <= 0.1f)
        {
            isFade = false;
        }
        redFence.color = new Color(1, 1, 1, checkingTime_redFence);
    }
}
