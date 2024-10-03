using UnityEngine;

public class I_Teddy : Item
{
    public override void PickUpEffect(Player p)
    {
        p.dodgeStacks++;
    }
}
