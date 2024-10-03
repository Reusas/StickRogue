using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreacherAttack : CharacterAttack
{
    public int pelletCount =6;
    [SerializeField] float spread;
    [SerializeField] AudioClip gunShotAudio;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask shootableLayers;

    public override bool Attack()
    {
        if(!canUsePrimaryAttack)
        {
            return false;
        }

        ShotGunShot();



        return true;
    }

    void ShotGunShot()
    {
        bool hasHit = false;
        float finalDamage = calculateDamage(player.damage);
        for(int i=0; i<pelletCount; i++)
        { 
            Vector3 offset = new Vector3(0,Random.Range(-spread,spread),0);
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position,-transform.right + offset,attackRange,shootableLayers);

            if(hit)
            {
                hasHit = true;
                
                hit.transform.GetComponent<IDamageable>()?.TakeDamage(finalDamage);
                

                GameObject fx = Instantiate(onHitFX,hit.point,onHitFX.transform.rotation);
                if(isCritical)
                {
                    if(i==pelletCount -1)
                    {
                        isCritical = false;
                    }
                    
                    UI_Manager.UISingleton.CreateDamageText(hit.point,finalDamage,Color.red);
                }
                else
                {
                    UI_Manager.UISingleton.CreateDamageText(hit.point,finalDamage,Color.white);
                }
                Destroy(fx,1f);
            }

            if(hasHit)
            {
                ItemManager.iM.OnHit(hit);
                hasHit = false;
            }

            Debug.DrawLine(firePoint.position,firePoint.position + (-transform.right + offset) *10,Color.red,5);

        }
        attackFX.Play();
        AudioManager.aMSingleton.PlayPitchedSound(gunShotAudio);
    }

    public override bool secondaryAttack()
    {
        if(!canUseSecondaryAttack)
        {
            return false;
        }



        canUseSecondaryAttack = false;
        StartCoroutine(shotGunSpam());
        StartCoroutine(attackCoolDown(1,secondaryAttackCoolDown));




        return true;
    }


    IEnumerator shotGunSpam()
    {
        for(int i=0; i<3; i++)
        { 
            ShotGunShot();
            yield return new WaitForSeconds(.18f);
        }



    }
}
