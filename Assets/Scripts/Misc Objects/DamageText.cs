using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DamageTeext : MonoBehaviour
{
    [SerializeField] GameObject damageTxt;
    GameObject dmgTxt;


    public void CreateeDamageText(Vector3 position,float damage)
    {
        dmgTxt = Instantiate(damageTxt,damageTxt.transform.position,Quaternion.identity,this.gameObject.transform);

        dmgTxt.transform.position = Camera.main.WorldToScreenPoint(position);
        dmgTxt.GetComponentInChildren<TMP_Text>().text = damage.ToString();
        Destroy(dmgTxt.gameObject,2f);

    }

    public void setCriticalDamageText()
    {
        dmgTxt.GetComponentInChildren<TMP_Text>().color = new Color(255,0,0);
    }

    public void setHealingText()
    {
        dmgTxt.GetComponentInChildren<TMP_Text>().color = new Color(0,255,0);
    }
}
