using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFx : MonoBehaviour
{
    Animator animator;

    float effectCheckingTime = 0;
    float effectTime = 0.7f;

    private void Awake()
    {
        animator=GetComponent<Animator>();
    }

    private void OnDisable()
    {
        effectCheckingTime = 0;
    }

    private void OnEnable()
    {
        animator.SetFloat("dustSpeed", 0.7f);
    }

    void Update()
    {
        effectCheckingTime += Time.deltaTime;

        if (effectCheckingTime > effectTime)
        {
            gameObject.SetActive(false);
        }
    }
}
