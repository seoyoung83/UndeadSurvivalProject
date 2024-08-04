using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffectController : MonoBehaviour 
{
    [SerializeField] float effectSpeed;

    private void OnEnable()
    {
        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        float size = 0f;
     
        while (size < 1)
        {
            size += Time.unscaledDeltaTime * effectSpeed;
            transform.localScale = new Vector3(size, size, size);
            yield return null;
        }

        transform.localScale = new Vector3(1, 1, 1);

    }
}
