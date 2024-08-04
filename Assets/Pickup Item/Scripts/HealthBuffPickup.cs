using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBuffPickup : Pickup
{
    public override void OnPicked(GameObject playerObject)
    {
        PlayerStat _stat = playerObject.GetComponentInParent<PlayerStat>();
        _stat.DoImmediateHeal(PlayerStat.BuffMaxHp * 0.75f);
    }
}
