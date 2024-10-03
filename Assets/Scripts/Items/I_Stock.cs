using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Stock : Item
{
    public float fireRateIncrease = 5;
    public override void PickUpEffect(Player p)
    {
        Debug.Log(p.fireRate + " -" + (p.fireRate * (fireRateIncrease / 100)));
        p.fireRate -= p.fireRate * (fireRateIncrease / 100);
    }
}
