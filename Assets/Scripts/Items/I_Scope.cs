using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Scope : Item
{
    [SerializeField] float critIncreasePerStack;

    public override void PickUpEffect(Player p)
    {
        p.critChance +=critIncreasePerStack;
    }
}
