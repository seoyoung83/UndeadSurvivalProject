using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AppearEffectWithSpriteFx : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;

    [Header("Set Scal")]
    [SerializeField] float setValue_minScal;
    [SerializeField] float setValue_MaxScal;

    [Header("Set Alpha")]
    [SerializeField] float setValue_minAlpha;
    [SerializeField] float setValue_MaxAlpha;

    [Header("Set Speed")]
    [SerializeField] float setValue_speed;

    [Header("Set Blink Count")]
    [SerializeField] int setValue_blinkCount;

    float checkingTime_scale;
    float checkingTime_alpha;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        checkingTime_scale = setValue_minScal;
        checkingTime_alpha = setValue_minAlpha;

        transform.localScale = new Vector3(setValue_minScal, setValue_minScal, 1);
        m_spriteRenderer.color = new Color(1, 1, 1, setValue_minAlpha);
    }

    void OnEnable()
    {
        checkingTime_scale = setValue_minScal;
        checkingTime_alpha = setValue_minAlpha;

        transform.localScale = new Vector3(setValue_minScal, setValue_minScal, 1);
        m_spriteRenderer.color = new Color(1, 1, 1, setValue_minAlpha);

        StartCoroutine(GetEffect());
    }

    void GetSettingInfo(float _minScal, float _maxScal, float _minAlpha, float _maxAlpha, float _Speed, int _blinkCount)
    {
        //setting
        setValue_minScal = _minScal;
        setValue_MaxScal = _maxScal;

        setValue_minAlpha = _minAlpha;
        setValue_MaxAlpha = _maxAlpha;

        setValue_speed = _Speed;

        setValue_blinkCount = _blinkCount;

        //reset
        checkingTime_scale = setValue_minScal;
        checkingTime_alpha = setValue_minAlpha;

        transform.localScale = new Vector3(setValue_minScal, setValue_minScal, 1);
        m_spriteRenderer.color = new Color(1, 1, 1, setValue_minAlpha);
    }

    IEnumerator GetEffect()
    {
        bool targetScal = false;

        while (!targetScal)
        {
            if (checkingTime_scale < setValue_MaxScal)
            {
                checkingTime_scale += Time.deltaTime * setValue_speed;
                transform.localScale = new Vector3(checkingTime_scale, checkingTime_scale, 1);
            }
            else if (checkingTime_scale > setValue_MaxScal)
            {
                checkingTime_scale = setValue_MaxScal;
                targetScal = true;
            }

            yield return null;
        }

        int count = 0;

        while (count < setValue_blinkCount)
        {
            checkingTime_alpha += Time.deltaTime * setValue_speed;

            if (checkingTime_alpha < setValue_MaxAlpha)
            {
                m_spriteRenderer.color = new Color(1, 1, 1, checkingTime_alpha);
            }
            else if (checkingTime_alpha >= setValue_MaxAlpha)
            {
                checkingTime_alpha = setValue_minAlpha;
                count++;
            }
            yield return null;
        }
        
        gameObject.SetActive(false);
    }

    void GettingBigger()
    {
        if (checkingTime_scale < setValue_MaxScal)
        {
            checkingTime_scale += Time.deltaTime * setValue_speed;
            transform.localScale = new Vector3(checkingTime_scale, checkingTime_scale, checkingTime_scale);
        }
        else if (checkingTime_scale > setValue_MaxScal)
        {
            checkingTime_scale = setValue_MaxScal;
        }

    }

    void Blinking()
    {
        checkingTime_alpha += Time.deltaTime * setValue_speed;

        if (checkingTime_alpha < 0.5f)
        {
            m_spriteRenderer.color = new Color(1, 0, 0, checkingTime_alpha);
        }
        else
        {
            checkingTime_alpha = 0f;
        }
    }
}
