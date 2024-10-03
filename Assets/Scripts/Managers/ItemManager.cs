using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemManager : MonoBehaviour
{


    public static ItemManager iM;

    public List<Item> items = new List<Item>();

    public SpecialItem specialItem;

    public int specialAttackCoolDown;

    

    void Awake()
    {
        iM = this;
    }


    // OPTIMIZATION IDEA
    // Maybe when i get componnets i should add them to another list of just on hit items and then next time loop through that. Then if an item is picked up 
    // Loop through the full list again and refresh the new smaller list. IDK maybe would be good
    public void OnHit(RaycastHit2D hit)
    {
        foreach(Item i in items)
        {
            i.GetComponent<IOnHit>()?.OnHit(hit);
        }
    }

    public void OnCriticalHit()
    {
        
        foreach(Item i in items)
        {
            i.GetComponent<IOnCriticalHit>()?.OnCriticalHit();
        }
    }

    public void OnJump()
    {
        foreach(Item i in items)
        {
            i.GetComponent<IOnJump>()?.OnJump();
        }
    }

    public void addSpecialItem(SpecialItem i)
    {
        if(specialItem != null)
        {
            Destroy(specialItem.gameObject);
            
        }
        specialItem = i;
        
    }

    public void OnSpecialUse()
    {
        specialItem.OnSpecialUse();
    }
}
