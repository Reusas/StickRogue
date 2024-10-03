using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Jetpack : Item
{
    [SerializeField] int maxJumpIncrease = 1;
    public override void PickUpEffect(Player p)
    {
        p.maxJumps +=maxJumpIncrease;
    }
}
