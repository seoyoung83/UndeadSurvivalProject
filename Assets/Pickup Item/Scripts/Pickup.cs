using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public PickupType m_pickuptype;

    float activeTime = 300;
    float checkingTimer_activeTime = 0;

    public abstract void OnPicked(GameObject playerObject);

    protected virtual void OnPickedLuckyBox(bool _state) { }

    private void OnEnable()
    {
        checkingTimer_activeTime = 0;
        OnPickedLuckyBox(true);
    }

    private void Update()
    {
        if((int)m_pickuptype < (int)PickupType.SKILLFUEL_GREEN_SMALL)
        {
            checkingTimer_activeTime += Time.deltaTime;

            if (checkingTimer_activeTime > activeTime)
            {
                Despawn();
                checkingTimer_activeTime = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject playerObject = collision.gameObject;
            OnPicked(playerObject);

            Despawn();
        }

        if (collision.gameObject.CompareTag("MagnetArea"))
        {
            OnPickedLuckyBox(false);
        }
    }

    void Despawn()
    {
        PickupDataToDespawn data;
        data.isPickupBox = false;
        data.Type = m_pickuptype;
        data.pickupObject = gameObject;
        PickupSpawner.AddPickupToDespawn(data);
    }
}
