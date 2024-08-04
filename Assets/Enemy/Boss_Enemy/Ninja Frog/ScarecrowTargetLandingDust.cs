using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowTargetLandingDust : MonoBehaviour
{
    Animator animator;

    float effectCheckingTimer = 0;
    float effectTime = 0.4f;

    public float damageValue { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        effectCheckingTimer = 0;
    }

    private void OnEnable()
    {
        animator.SetFloat("dustSpeed", effectTime);
    }

    void Update()
    {
        effectCheckingTimer += Time.deltaTime;

        if (effectCheckingTimer > effectTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat _playerStat = collision.gameObject.GetComponentInParent<PlayerStat>();
            if (_playerStat)
                _playerStat.AddDamage(damageValue);
        }
    }
}
