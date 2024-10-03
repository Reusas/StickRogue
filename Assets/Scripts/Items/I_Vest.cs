using UnityEngine;

public class I_Vest : Item
{
    [SerializeField] int maxHealthIncreasePerStack = 30;
    public override void PickUpEffect(Player p)
    {
        p.maxHealth += maxHealthIncreasePerStack;
        UI_Manager.UISingleton.UpdatePlayerHealth(p);
    }


}
