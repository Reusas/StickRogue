using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelector : MonoBehaviour
{
    [SerializeField] GameObject startingPlayer;
    public static GameObject selectedPlayer;
    [SerializeField] Transform selectOutline;

    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text characterDescription;
    [SerializeField] TMP_Text FirstAttackName;
    [SerializeField] TMP_Text FirstAttackDesc;
    [SerializeField] TMP_Text SecondAttackName;
    [SerializeField] TMP_Text SecondAttackDesc;
    [SerializeField] TMP_Text health;
    
    void Start()
    {
        selectedPlayer = startingPlayer;
    }

    public void UpdateSelectedCharacter(GameObject characterSelected, Vector3 outLinePos,CharacterSelectButton info)
    {
        selectedPlayer = characterSelected;
        selectOutline.transform.position = new Vector3(outLinePos.x+0.45f,outLinePos.y -1.05f,0);
        UpdateDescription(info);
    }

    public void UpdateDescription(CharacterSelectButton info)
    {
        characterName.text = info.characterName;
        characterDescription.text = info.characterDescription;
        FirstAttackName.text = info.FirstAttackName +":";
        FirstAttackDesc.text = info.FirstAttackDesc;
        SecondAttackName.text = info.SecondAttackName+":";
        SecondAttackDesc.text = info.SecondAttackDesc;
        health.text = "Health:" + info.playerSelected.GetComponent<Player>().health;

    }

}
