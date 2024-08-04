using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRocketTargeting : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;

    float checkingTimer = 1;

    bool isBigger = false;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnDisable()
    {
        checkingTimer = 1f;
        
        isBigger = false;

        transform.localScale = Vector3.one;
        
        m_spriteRenderer.color = new Color(0.5f, 1, 0, 0.7f);
    }

    private void Update()
    {
        if (isBigger)
        {
            checkingTimer += Time.deltaTime * 0.8f;

            m_spriteRenderer.color = new Color(0.5f, 1, 0, 1.4f - checkingTimer);
        }
        else if (!isBigger)//½ÃÀÛ
        {
            checkingTimer -= Time.deltaTime * 0.5f;
        }

        if (checkingTimer > 1.05f)
        {
            gameObject.SetActive(false);
        }
        else if(checkingTimer < 0.85f)
        {
            isBigger = true;
        }

        transform.localScale = new Vector3(checkingTimer, checkingTimer, 0);
    }
}
