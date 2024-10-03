
using UnityEngine;

public class I_Clip : Item
{
    [SerializeField] float damagePrecentageIncrease = 5;
    public override void PickUpEffect(Player p)
    {
        p.damage += Mathf.RoundToInt(p.baseDamage * (damagePrecentageIncrease/100));
    }
}
