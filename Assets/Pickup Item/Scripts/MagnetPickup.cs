using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPickup : Pickup
{
    public override void OnPicked(GameObject playerObject)
    {
        playerObject.GetComponent<PlayerStat>().DoMagnet();
    }
}
