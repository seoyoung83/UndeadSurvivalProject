using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarldspaceDamageTextUI : MonoBehaviour //½Ã°£ Áö³ª¸é ºñÈ°¼ºÈ­
{
   [SerializeField] TextMeshProUGUI damageText;

    Color[] setColor = { Color.white, new Color(1,0.75f,0), new Color(1, 0.55f, 0), new Color(1, 0.35f, 0), new Color(1, 0.15f, 0) }; //  Èò, ³ë¶û, ÁÖÈ², ´ÙÈ«, »¡°­

    float fadeSpeed = 1.5f;
    float fadeAlphaTime = 1f;
    float checkingTime_alpga = 0;

    int m_damaged;

    private void Start()
    {
        damageText.fontSize = 0.25f;
    }

    public void DisplayDamageTextEffect(Transform _trans, float _damageValue)
    {
        if (_damageValue <= 1)
        {
            m_damaged = 1;
        }
        else if (_damageValue > 1)
        {
            m_damaged = (int)Mathf.Round(_damageValue);
        }

        SetDamageTextColor(_damageValue);

        damageText.text = m_damaged.ToString();

        StartCoroutine(StartDamageEffect(_trans));
    }

    IEnumerator StartDamageEffect(Transform _trans)
    {
        bool haveStop = false;
        Vector3 dir = new Vector3(Random.Range(-0.15f, 0.15f), Random.Range(0, 0.25f),0 );

        while (_trans && !haveStop)
        {
            Vector3 tartgetPos = new Vector3(_trans.position.x, _trans.position.y + 0.1f, 0)+ dir;

            gameObject.transform.position = tartgetPos;

             checkingTime_alpga += Time.deltaTime * fadeSpeed;

            if (checkingTime_alpga < fadeAlphaTime)
            {
                damageText.alpha = 1 - checkingTime_alpga;
            }
            else if (checkingTime_alpga > fadeAlphaTime)
            {
                haveStop = true;
            }
            yield return null;
        }

        ResetTextEffect();
    }

    void ResetTextEffect()
    {
        gameObject.SetActive(false);

        damageText.alpha = 1;
        checkingTime_alpga = 0;
        damageText.color = setColor[0];
    }

    void SetDamageTextColor(float _damage)
    {
        float palyerApk = PlayerStat.BuffOffense;

        float[] standard = { palyerApk * 2, palyerApk * 5, palyerApk * 10, palyerApk * 20 };

        if (_damage < standard[0])
        {
            damageText.color = setColor[0];
        }
        else if (_damage >= standard[0] && _damage < standard[1])
        {
            damageText.color = setColor[1];
        }
        else if (_damage >= standard[1] && _damage < standard[2])
        {
            damageText.color = setColor[2];
        }
        else if (_damage >= standard[2] && _damage < standard[3])
        {
            damageText.color = setColor[3];
        }
        else if (_damage >= standard[3])
        {
            damageText.color = setColor[4];
        }
    }

}
