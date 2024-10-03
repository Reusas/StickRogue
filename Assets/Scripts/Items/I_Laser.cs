using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Laser : Item
{
    [SerializeField] float rangeIncreasePrecentage = 10;
    public override void PickUpEffect(Player p)
    {
        Debug.Log(p.attacks.attackRange * (rangeIncreasePrecentage / 100));
        p.attacks.attackRange += p.attacks.attackRange * (rangeIncreasePrecentage / 100);
    }
}
