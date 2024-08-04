using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DangerousPointFx : MonoBehaviour
{
    new ParticleSystem particleSystem;
    ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule;
    float effectCheckingTimer = 0; //√÷¥Î 4√ 
    float checkingTimer_color = 0;
    public float activeTime;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        colorOverLifetimeModule = particleSystem.colorOverLifetime;
    }

    private void OnDisable()
    {
        colorOverLifetimeModule.color = new Color(1, 1, 1, 1);
        checkingTimer_color = 0;
        effectCheckingTimer = 0;
    }

    private void Update()
    {
        effectCheckingTimer += Time.deltaTime;

        if(effectCheckingTimer < activeTime) 
        {
            checkingTimer_color += Time.deltaTime * 2;

            if (checkingTimer_color < 0.8f)
            {
                colorOverLifetimeModule.color = new Color(1, 1, 1, checkingTimer_color);
            }
            else if (checkingTimer_color > 0.8f)
            {
                checkingTimer_color = 0;
            }
        }
        else if(effectCheckingTimer > activeTime)
        {
            gameObject.SetActive(false);
        }
    }

}
