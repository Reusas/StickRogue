using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Rucksack : Item
{
    [SerializeField] float cooldownDecrease = 10;

    public override void PickUpEffect(Player p)
    {
        p.attacks.primaryAttackCoolDown -= p.attacks.baseCoolDown1 * (cooldownDecrease/100);
        p.attacks.secondaryAttackCoolDown -= p.attacks.baseCoolDown2 * (cooldownDecrease/100);
    }
}
