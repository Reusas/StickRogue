using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject playerSelected;
    [SerializeField] CharacterSelector cS;

    public string characterName;
    public string characterDescription;
    public string FirstAttackName;
    public string FirstAttackDesc;
    public string SecondAttackName;
    public string SecondAttackDesc;



    public void OnPointerDown(PointerEventData eventData)
    {
        cS.UpdateSelectedCharacter(playerSelected,transform.position,this);
    }
}
