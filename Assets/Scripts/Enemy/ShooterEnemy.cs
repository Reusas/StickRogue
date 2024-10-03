using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    float attackTimer;

    [SerializeField] string AttackAnim;
    [SerializeField] Transform RayPos;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float range;
    [SerializeField] protected GameObject onHitFX;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] ParticleSystem shootFX;
    bool isAiming = false;

    public override void AttackState()
    {
        if(!isAiming)
        {
            anim.Play("Pistol_Aim",-1,0f);
            attackTimer = Time.time + fireRate * 2;
            isAiming = true;
            
        }

        if(isAiming)
        {
            if(Time.time > attackTimer)
            {
                Shoot();
            }
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

    void Shoot()
    {
        anim.Play(AttackAnim,-1,0f);
        attackTimer = Time.time + fireRate;
        AudioManager.aMSingleton.PlayPitchedSound(hitSfx);
        shootFX.Play();

        RaycastHit2D hit = Physics2D.Raycast(RayPos.position,-transform.right,range,attackLayer);
        Debug.DrawLine(RayPos.position,RayPos.position + -transform.right * range,Color.red,5);

        if(hit)
        {

            hit.transform.GetComponent<IDamageable>().TakeDamage(damage);
            GameObject fx = Instantiate(onHitFX,hit.point,onHitFX.transform.rotation);

            Destroy(fx,1f);
        }
    }


}
