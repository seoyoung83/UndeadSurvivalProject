using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPickup : Pickup
{
    public override void OnPicked(GameObject playerObject)
    {
        StageUIManager.Instance.GetPickupBombEffect();

        EnemySpawner.Instance.DoDamageAllOfSpawnEnemy();

        PickupSpawner.Instance.DespawnAllOfPickupBox();
    }
}
