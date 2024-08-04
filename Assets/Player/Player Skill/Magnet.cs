using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magnet : MonoBehaviour //Skill Magnet Function
{
    Rigidbody2D rigid;
    bool isPulledIn = false;

    float checkingTimer = 0f;
    float bounceTime = 0.2f;

    float speed = 5.5f;

    Transform playerTrans;
    Vector3 bounceDirect;

    private void Start()
    {
        playerTrans = GameObject.FindObjectOfType<PlayerMove>().transform;

        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        checkingTimer = 0;

        bounceDirect = Vector3.zero; 
        
        isPulledIn = false;
    }

    private void Update()
    {
        //Skill Magnet Function
        if (!isPulledIn)
            return;
        Move();
    }

    private void Move()
    {
        if (checkingTimer < bounceTime)
        {
            checkingTimer += Time.deltaTime;
            rigid.AddForce(bounceDirect * speed, ForceMode2D.Force);
        }
        else
        {
            rigid.velocity = Vector3.zero;
            transform.position = Vector3.Lerp(transform.position, playerTrans.position, Time.deltaTime * (speed * 1.5f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        if (collision.gameObject.CompareTag("MagnetArea"))
        {
            if (!collision.gameObject.GetComponentInParent<PlayerMove>())
                return;

            Vector3 triggerPlayerVect = collision.gameObject.GetComponentInParent<PlayerMove>().transform.position;

            bounceDirect = (transform.position - triggerPlayerVect).normalized;

            isPulledIn = true;
        }
    }
}
