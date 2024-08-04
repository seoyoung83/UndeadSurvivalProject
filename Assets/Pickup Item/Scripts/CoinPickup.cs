using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Pickup
{
    [SerializeField] int coinValue;

    public override void OnPicked(GameObject playerObject)
    {
        ScoreManager.Instance.UpdateCoinCount(coinValue);
    }
}
