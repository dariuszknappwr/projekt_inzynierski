using RPG.Combat;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MeleeWeapon
{
    public override void ActionOnStomp(GameObject owner, GameObject stomped)
    {
        owner.GetComponent<PlayerMover>().ForceJump();
    }
}
