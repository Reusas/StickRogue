using UnityEngine;
using UnityEngine.UI;

public abstract class SpecialItem : Item
{
    protected Player player;
    Button specialButton;
    public int coolDown;

    void Start()
    {
        isSpecial = true;
        specialButton = GameObject.FindWithTag("SpecialButton").GetComponent<Button>();
    }

    public override void PickUpEffect(Player p)
    {
        UpdateSpecialButton();
        ItemManager.iM.addSpecialItem(this);
        ItemManager.iM.specialAttackCoolDown = coolDown;
        player = p;
    }

    public abstract void OnSpecialUse();

    protected void UpdateSpecialButton()
    {
        
        specialButton.interactable = true;
        Image i = specialButton.transform.GetChild(0).GetComponent<Image>();
        i.sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        i.color = new Color32(255,255,255,255);

    }
}
