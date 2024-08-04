using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteStat : MonoBehaviour //허수아비 
{
    SpriteRenderer spriteRenderer;
    Color[] damagedColor = { Color.white, new Color(1f, 0.5f, 0.5f) };

    float maxHp = 9999999;
    float currentHp;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        currentHp = maxHp;
    }

    public void AddDamage(float _damage) // 미사일 무기 제외 
    {
        if (!gameObject.activeInHierarchy)
            return;

        currentHp -= _damage;

        if (currentHp <= 0f)
        {
            currentHp = maxHp;
        }
        
        StartCoroutine(StartDamageEffect());

        GameObject _damageFx = EffectPooler.Instance.GetEffect(EffectType.EFFECTTYPE_DAMAGETEXT);
        if (_damageFx != null)
        {
            _damageFx.SetActive(true);
            _damageFx.GetComponent<WarldspaceDamageTextUI>().DisplayDamageTextEffect(transform, _damage);
        }
    }

    IEnumerator StartDamageEffect() 
    {
        float pingpongCheckingTime = 0;

        int count = 0;
        while (count < 2)
        {
            //PingPong Alhpa
            pingpongCheckingTime += Time.deltaTime * 1.8f;

            if (pingpongCheckingTime > 1)
            {
                pingpongCheckingTime = 0;
                count++;
            }
            float _value;
            _value = Mathf.PingPong(pingpongCheckingTime * 10f, damagedColor.Length);
            spriteRenderer.material.SetColor("_Color", damagedColor[(int)_value]);

            yield return null;
        }

        spriteRenderer.material.SetColor("_Color", damagedColor[0]);
    }
}
