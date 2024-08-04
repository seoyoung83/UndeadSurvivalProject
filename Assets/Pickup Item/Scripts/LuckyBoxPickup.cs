using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBoxPickup : Pickup
{
    [SerializeField] GameObject rasinbowObject;

    [SerializeField] int luckyBoxType; //0:ÄÄ¹î, 1:½ºÅ³

    public override void OnPicked(GameObject playerObject)
    {
        RouletteUIManager.Instance.OpenRoulette(luckyBoxType);
    }

    protected override void OnPickedLuckyBox(bool _state)
    {
        rasinbowObject.SetActive(_state);
    }
}
