using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrurningScarecroeTarget : MonoBehaviour
{
    public float damageValue { get; set; }

    float checkingTimer_damage = 0;
    float checkingTimer =0;
    float activeTime = 5f;

    private void OnEnable()
    {
        checkingTimer = 0;
    }

    private void Update()
    {
        checkingTimer += Time.deltaTime;
        if (checkingTimer > activeTime)
        {
            checkingTimer = 0;
            transform.parent.transform.gameObject.SetActive(false);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            checkingTimer_damage += Time.deltaTime;
            if (checkingTimer_damage > 1)
            {
                PlayerStat _playerStat = collision.gameObject.GetComponent<PlayerStat>();
                if (_playerStat)
                    _playerStat.AddDamage(damageValue / 3);
            }
        }
    }
}
