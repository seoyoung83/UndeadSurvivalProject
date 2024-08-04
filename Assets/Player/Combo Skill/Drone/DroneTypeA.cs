using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTypeA : Drone
{
    public override void Awake()
    {
        thisSkillType = PlayerSkillCategory.ACTIVE_DRONE_A;

        curruntActiveSkillTypeNumber = (int)thisSkillType;

        activeSkillType = SkillType.SKILL_TYPE_A_DRONE;
        bulletType = PlayerBulletType.DRONEA_ROCKET_NORMAL;
    }
}

