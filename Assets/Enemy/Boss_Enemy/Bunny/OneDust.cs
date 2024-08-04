using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDust : MonoBehaviour
{
    Animator animator;

    float effectCheckingTime = 0;
    float effectTime = 0.7f;

    public float damageValue { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            effectCheckingTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);
        }
    }
}
