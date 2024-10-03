using UnityEngine;
using System.Linq;
using System;

public class I_Syringe : Item, IOnCriticalHit
{
    Player player;
    public int healingStrength;

    public void OnCriticalHit()
    {
        player.health += healingStrength * stacks;
        if(player.health >=player.maxHealth)
        {
            player.health = player.maxHealth;
        }
        UI_Manager.UISingleton.UpdatePlayerHealth(player);
        UI_Manager.UISingleton.CreateDamageText(player.transform.position,(int)healingStrength,Color.green);
    }

    public override void PickUpEffect(Player p)
    {
        player = p;
       
        ItemManager.iM.items.Add(this);
    }

    

}
