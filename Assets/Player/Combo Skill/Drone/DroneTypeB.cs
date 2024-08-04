using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTypeB : Drone
{
    public override void Awake()
    {
        thisSkillType = PlayerSkillCategory.ACTIVE_DRONE_B;

        curruntActiveSkillTypeNumber = (int)thisSkillType;

        activeSkillType = SkillType.SKILL_TYPE_B_DRONE;
        bulletType = PlayerBulletType.DRONEB_ROCKET_NORMAL;
    }
}
