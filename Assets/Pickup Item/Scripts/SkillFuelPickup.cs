using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFuelPickup : Pickup
{
    [SerializeField] float skillFuelValue;

    public override void OnPicked(GameObject playerObject)
    {
        ScoreManager.Instance.UpdateSkillFuel(skillFuelValue);
    }
}
