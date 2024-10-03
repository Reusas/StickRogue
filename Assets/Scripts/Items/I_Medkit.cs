using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Medkit : Item
{
    [SerializeField] int healthRegenIncrasePerStack = 2;
    public override void PickUpEffect(Player p)
    {
        p.healthRegenAmount +=healthRegenIncrasePerStack;
    }
}
