using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBoxPickup : Pickup
{
    [SerializeField] GameObject rasinbowObject;

    [SerializeField] int luckyBoxType; //0:�Ĺ�, 1:��ų

    public override void OnPicked(GameObject playerObject)
    {
        RouletteUIManager.Instance.OpenRoulette(luckyBoxType);
    }

    protected override void OnPickedLuckyBox(bool _state)
    {
        rasinbowObject.SetActive(_state);
    }
}
