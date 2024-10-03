using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class CharacterAttack : MonoBehaviour
{
    [HideInInspector] public TMP_Text attackButtonText;
    [HideInInspector] public TMP_Text attackButtonText2;
    [HideInInspector] public TMP_Text attackButtonText3;

    protected Player player;
    [SerializeField] protected GameObject onHitFX;
    [SerializeField] protected ParticleSystem attackFX;
    protected bool canUsePrimaryAttack = true;
    protected bool canUseSecondaryAttack = true;
    protected bool canUseSpecialAttack = true;
    public float attackRange;

    public float primaryAttackCoolDown;
    public float secondaryAttackCoolDown;
    [HideInInspector] public float baseCoolDown1;
    [HideInInspector] public float baseCoolDown2;

    protected bool isCritical=false;


    void Start()
    {
        player = GetComponent<Player>();
        baseCoolDown1 = primaryAttackCoolDown;
        baseCoolDown2 = secondaryAttackCoolDown;
    }

    public abstract bool Attack();

    public abstract bool secondaryAttack();

    public  bool thirdAttack()
    {
        if(!canUseSpecialAttack)
        {
            return false;
        }
        canUseSpecialAttack = false;
        StartCoroutine(attackCoolDown(2,ItemManager.iM.specialAttackCoolDown));
        specialAttack();
        return true;
    }

    public void specialAttack()
    {
        ItemManager.iM.OnSpecialUse();
    }

    public float calculateDamage(float damage)
    {
        float finalDamage = damage;

        int critRoll = Random.Range(1,101);
        if(player.critChance > critRoll)
        {
            finalDamage = damage * 2;
            isCritical = true;
        }

        return finalDamage;
    }

    // Attack index is used to know if the cooldown is being set on the primary or secondary attack. 0 is primary, 1 is secondary.
    protected IEnumerator attackCoolDown(int attackIndex, float coolDownTime)
    {   
        StartCoroutine(showCooldownTime(attackIndex,coolDownTime));
        yield return new WaitForSeconds(coolDownTime);
        switch(attackIndex)
        {
            case 0:
            canUsePrimaryAttack = true;
            break;
            case 1:
            canUseSecondaryAttack = true;
            break;
            case 2:
            canUseSpecialAttack = true;
            break;
        }
        Debug.Log("Attack Reset");
    }

    IEnumerator showCooldownTime(int _attackIndex, float _coolDownTime)
    {
        
        // Sec attack
        if(_attackIndex == 1)
        {
            attackButtonText2.text = _coolDownTime.ToString();
            if(_coolDownTime > 0)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(showCooldownTime(_attackIndex,_coolDownTime -1));
            }
            else
            {
                attackButtonText2.text ="";
            }

        }
        else if(_attackIndex == 2)
        {
            attackButtonText3.text = _coolDownTime.ToString();
            if(_coolDownTime > 0)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(showCooldownTime(_attackIndex,_coolDownTime -1));
            }
            else
            {
                attackButtonText3.text ="";
            }

        }
    }



}
