using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_MineBelt : Item, IOnJump
{
    Player player;
    [SerializeField] GameObject mine;
    public static int currentMines = 0;
    int maxMines = 3;

    public void OnJump()
    {
        Debug.Log("JUMPED");
        if(currentMines<maxMines)
        {
            GameObject m = Instantiate(mine,player.transform.position,Quaternion.identity);
            m.GetComponent<LandMine>().playersMine = true;
            m.GetComponent<LandMine>().damage = m.GetComponent<LandMine>().damage * stacks;
            currentMines++;
            
        }
        

    }

    public override void PickUpEffect(Player p)
    {
        player =p;
        ItemManager.iM.items.Add(this);
        
    }
}
