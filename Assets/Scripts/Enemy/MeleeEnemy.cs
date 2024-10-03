using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    float attackTimer;
    [SerializeField] Transform meleeRayPos;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float range;
    [SerializeField] protected GameObject onHitFX;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] string AttackAnim;

    public override void AttackState()
    {
        if(Time.time > attackTimer)
        {
            anim.Play(AttackAnim,-1,0f);
            attackTimer = Time.time + fireRate;
        }
       
    }

    public override void MoveState()
    {
        if(Mathf.Abs(transform.position.y - playerTransform.position.y) >= 1)
        {
            currentState = States.Patrolling;
            Debug.Log("Patrolling");
        }
        if(playerTransform.position.x < transform.position.x - distanceBeforeAttack)
        {
            rb.velocity = new Vector2(-speed * Time.deltaTime,rb.velocity.y);
            anim.SetBool("Walk", true);
        }
        else if(playerTransform.position.x > transform.position.x + distanceBeforeAttack)
        {
            rb.velocity = new Vector2(speed * Time.deltaTime,rb.velocity.y);
            anim.SetBool("Walk", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            currentState = States.Attacking;
            anim.SetBool("Walk", false);
            
        }
    }

    public void StabAttack()
    {

        RaycastHit2D hit = Physics2D.Raycast(meleeRayPos.position,-transform.right,range,attackLayer);
        Debug.DrawLine(meleeRayPos.position,meleeRayPos.position + -transform.right * range,Color.red,5);

        if(hit)
        {

            hit.transform.GetComponent<IDamageable>().TakeDamage(damage);
            GameObject fx = Instantiate(onHitFX,hit.point,onHitFX.transform.rotation);
            AudioManager.aMSingleton.PlayPitchedSound(hitSfx);
            Destroy(fx,1f);
        }


        
    }


}
