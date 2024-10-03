using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager UISingleton;


    [SerializeField] int itemInfoTime;
    float finalItemInfoTime;
    bool showItemInfo;

    [SerializeField] GameObject itemInfoScreen;
    [SerializeField] TMP_Text itemNameTxt;
    [SerializeField] TMP_Text itemInfoTxt;
    [SerializeField] Image itemImg;
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject bossInfo;
    [SerializeField] Slider bossHealthBar;
    [SerializeField] TMP_Text bossHealthText;
    [SerializeField] TMP_Text bossName;

    [Header("PlayerStuff")]
    [SerializeField] Slider playerHealthSlider;
    [SerializeField] TMP_Text playerHealthText;
    [SerializeField] Slider playerExpSlider;
    [SerializeField] TMP_Text playerExpText;
    [SerializeField] TMP_Text playerMoney;

    void Awake()
    {
        UISingleton = this;
    }

    void Update()
    {
        if(showItemInfo)
        {
            if(Time.time > finalItemInfoTime)
            {
                showItemInfo = false;
                itemInfoScreen.SetActive(false);
            }
        }

    }


    public void UpdateItemInfo(string itemName,string itemInfo, Sprite itemImage)
    {
        itemNameTxt.text = itemName;
        itemInfoTxt.text = itemInfo;
        itemImg.sprite = itemImage;
    }

    

    public void ShowItemInfo()
    {
        itemInfoScreen.SetActive(true);
        showItemInfo = true;
        finalItemInfoTime = Time.time + itemInfoTime;
    }



    public void UpdatePlayerHealth(Player player)
    {
        playerHealthSlider.maxValue = player.maxHealth;
        playerHealthSlider.value = player.health;
        playerHealthText.text = player.health + "\\" + player.maxHealth;
    }
    public void UpdatePlayerExp(Player player)
    {
        playerExpSlider.value = player.exp;
        playerExpSlider.maxValue = player.expUntilNextLevel;
        playerExpText.text = "LVL: " + player.level;
    }

    public void UpdatePlayerExpSlider(Player player)
    {
        playerExpSlider.minValue = player.exp;
        playerExpSlider.maxValue = player.expUntilNextLevel;
        
    }

    public void UpdatePlayerMoney(Player player)
    {
        playerMoney.text = "$" + player.money;
    }


    public void CreateDamageText(Vector3 position,float damage,Color color)
    {
        GameObject dmgTxt = Instantiate(damageText,damageText.transform.position,Quaternion.identity,canvas.transform);
        dmgTxt.transform.position = Camera.main.WorldToScreenPoint(position);
        TMP_Text dmgTxtText = dmgTxt.GetComponentInChildren<TMP_Text>();
        dmgTxtText.text = damage.ToString();
        dmgTxtText.color = color;
        Destroy(dmgTxt.gameObject,2f);
    }

    public void enableBossInfo(float health, string _bossName)
    {
        bossInfo.SetActive(true);
        bossHealthBar.maxValue = health;
        bossHealthBar.value = health;
        bossName.text = _bossName;
        bossHealthText.text = health + "\\" + health;
    }

    public void disableBossInfo()
    {
        bossInfo.SetActive(false);
    }

    public void updateBossHealth(Boss boss)
    {
        bossHealthBar.value = boss.health;
        bossHealthText.text = boss.health + "\\" + boss.maxHealth;
    }

}
