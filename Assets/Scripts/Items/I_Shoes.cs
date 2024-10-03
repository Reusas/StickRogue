using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Shoes : Item
{
    [SerializeField] int speedIncreasePerStack =10;
    public override void PickUpEffect(Player p)
    {
        p.speed += p.baseSpeed * (speedIncreasePerStack/100);
    }
}
