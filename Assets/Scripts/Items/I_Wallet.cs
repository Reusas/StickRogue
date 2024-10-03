using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Wallet : Item
{
    [SerializeField] float moneyEarnedMultiplierPrecentage = 10;
    public override void PickUpEffect(Player p)
    {
        p.moneyEarnedMultiplier += moneyEarnedMultiplierPrecentage/100;
    }

}
