using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupBox : Pickup
{
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite openBoxSprite; 
    [SerializeField] Sprite closedBoxSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_pickuptype = PickupType.ONECOIN; //Default
    }

    private void OnEnable()
    {
        SetBoxState(true);
    }

    public void SetBoxState(bool _packing)
    {
        if (_packing)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            spriteRenderer.sprite = openBoxSprite;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            spriteRenderer.sprite = closedBoxSprite;

            //box오픈 - Pickup 즉시스폰 
            PickupDataToDespawn data;
            data.isPickupBox = true;
            data.Type = m_pickuptype;
            data.pickupObject = gameObject;
            PickupSpawner.Instance.DespawnPickup(data, true);
        }
    }

    public override void OnPicked(GameObject playerObject)
    {
        SetBoxState(false);
    }
}
