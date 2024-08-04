using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEffect : MonoBehaviour
{
    float checkingTimer_active;
    bool activeFx = false;

    float checkingTimer_bounce;
    int count = 0;


    void Update()
    {
        if (!activeFx)
        {
            //ÀÌÆåÆ® ÄðÅ¸ÀÓ
            checkingTimer_active += Time.deltaTime;

            if (checkingTimer_active > 2f)
            {
                checkingTimer_active = 0f;
                activeFx = true;
            }
        }
        else
        {
            EffectBounce();
        }
    }

    void EffectBounce()
    {
        if (count < 4)
        {
            checkingTimer_bounce += Time.deltaTime * 2f;
            if (checkingTimer_bounce >= 1f)
            {
                checkingTimer_bounce = 0f;
                count++;
            }
            transform.localScale = new Vector3(checkingTimer_bounce, checkingTimer_bounce, 0);
        }
        else if (count >= 4)
        {
            activeFx = false;
            count = 0;
        }
    }
}
