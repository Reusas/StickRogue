using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chest : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] GameObject[] itemsDropped;
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text chestInfoTxt;
    bool showInfo;
    bool canOpen = false;
    bool isOpen = false;
    public int price;
    Player player;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Player" && !isOpen)
        {
            player = col.GetComponent<Player>();
            canOpen = true;
            ChestInfo();
            
        }
    }

    void ChestInfo()
    {
        showInfo = !showInfo;
        chestInfoTxt.gameObject.SetActive(showInfo);
        chestInfoTxt.text = "Tap to open ($" + price + ")";
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag=="Ground")
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform.tag == "Player")
        {
            if(!isOpen)
            {
                canOpen = false;
                ChestInfo();
            }

            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(canOpen && !isOpen)
        {
            if(canBuy(player))
            {
                isOpen = true;
                anim.Play("ChestOpen",-1,0f);
                chestInfoTxt.gameObject.SetActive(false);
                OnOpen();
                Destroy(this.gameObject,10f);
                this.enabled = false;
                GameObject itemDropped = itemsDropped[Random.Range(0,itemsDropped.Length)];
                Instantiate(itemDropped,transform.position + new Vector3(0,1,0),Quaternion.identity);
            }

        }
    }

    bool canBuy(Player p)
    {
        if(p.money >= price)
        {
            p.earnMoney(-price);
            return true;
        }
        return false;
    }


    void OnOpen()
    {
        Debug.Log("Chest Opened!");
    }
}
