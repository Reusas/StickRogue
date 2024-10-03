using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Bandage : SpecialItem
{
    public override void OnSpecialUse()
    {
        Debug.Log("test");
        player.health = 95;
    }
}
