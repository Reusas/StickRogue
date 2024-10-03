using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Boots : Item
{
    public int jumpForceIncrease = 10;
    public override void PickUpEffect(Player p)
    {
        p.jumpForce = p.baseJumpForce * (jumpForceIncrease / 100);
    }
}
