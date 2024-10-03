using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SoldierAttack : CharacterAttack
{
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask shootableLayers;

    [SerializeField] GameObject grenadePrefab;

    [SerializeField] Vector2 grenadeForce;



    [SerializeField] AudioClip gunShotAudio;
    [SerializeField] AudioClip grenadeThompAudio;

    
  

    public override bool Attack()
    {
        if(!canUsePrimaryAttack)
        {
            return false;
            
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position,-transform.right,attackRange,shootableLayers);

        if(hit)
        {
            float finalDamage = calculateDamage(player.damage);
            hit.transform.GetComponent<IDamageable>()?.TakeDamage(finalDamage);
            // This is the blood effect
            GameObject fx = Instantiate(onHitFX,hit.point,onHitFX.transform.rotation);
            // If the hit is critical create a critical damage text
            if(isCritical)
            {
                isCritical = false;
                UI_Manager.UISingleton.CreateDamageText(hit.point,finalDamage,Color.red);
                ItemManager.iM.OnCriticalHit();
            }
            // Otherwise create a normal damage text
            else
            {
                UI_Manager.UISingleton.CreateDamageText(hit.point,finalDamage,Color.white);
            }
            Destroy(fx,1f);

            ItemManager.iM.OnHit(hit);
        }
        attackFX.Play();
        AudioManager.aMSingleton.PlayPitchedSound(gunShotAudio);

        
        return true;

    }



    public override bool secondaryAttack()
    {
        if(!canUseSecondaryAttack)
        {
            return false;
        }
        canUseSecondaryAttack = false;
        
        float finalDamage = calculateDamage(player.damage * 5);


        GameObject grenade = Instantiate(grenadePrefab,firePoint.transform.position,Quaternion.identity);
        Grenade gr = grenade.GetComponent<Grenade>();
        gr.damage = finalDamage;
        if(isCritical)
        {
            isCritical = false;
            gr.isCritical = true;
        }
        
        Rigidbody2D grenadeRB = grenade.GetComponent<Rigidbody2D>();
        // The firepoint . z will be -1 or 1 depending which direction is faced thus the grenade will always fly forwards
        grenadeRB.AddForce(new Vector2(grenadeForce.x * firePoint.forward.z ,grenadeForce.y));
        
        AudioManager.aMSingleton.PlayNormalSound(grenadeThompAudio);

        StartCoroutine(attackCoolDown(1,secondaryAttackCoolDown));
        return true;
        
    }

}
