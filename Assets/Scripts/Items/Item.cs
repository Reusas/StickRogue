using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Item : MonoBehaviour
{
    [SerializeField] bool shouldStay = false;
    protected bool isSpecial;
    public int stacks = 1;

    Rigidbody2D rb;
    [SerializeField] protected string itemName;
    [SerializeField] protected string info;
    


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(50,200));
    }

    public void OnPickUp(Player p)
    {
        UI_Manager.UISingleton.UpdateItemInfo(itemName,info,GetComponentInChildren<SpriteRenderer>().sprite);
        UI_Manager.UISingleton.ShowItemInfo();
        if(!shouldStay)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(transform.GetComponent<Rigidbody2D>());
            Destroy(transform.GetComponent<BoxCollider2D>());
            
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if(isSpecial)
            {
                PickUpEffect(p);
                return;
            }

            Type type = this.GetType();
            
            bool exists = ItemManager.iM.items.Any(t => t.GetType() == type);
            
            

            if(exists)
            {
                Destroy(this.gameObject);
                // If the item already exists simply increase its stack by one.
                int index = ItemManager.iM.items.FindIndex(t=> t.GetType() == type);
                ItemManager.iM.items[index].stacks++;
                return;
            }


        }
        PickUpEffect(p);

    }

    

    public abstract void PickUpEffect(Player p);


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag=="Ground")
        {
            rb.velocity = Vector2.zero;
            transform.gameObject.layer = 0;
            rb.isKinematic = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag=="Player")
        {
            OnPickUp(col.GetComponent<Player>());

        }
    }
    
}
