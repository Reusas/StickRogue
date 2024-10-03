using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Character,IDamageable
{
    public enum States
    {
        Patrolling,
        Moving,
        Attacking
    }
    protected States currentState;

    [SerializeField] Slider healthSlider;
    [SerializeField] int expGiven;
    [SerializeField] int moneyGiven;
    public int spawnChance;
    Player player;
    [HideInInspector] public Transform playerTransform;
    public float distanceBeforeAttack=5;
    [SerializeField] float patrolDistance = 20;
    [SerializeField] float patrolTime = 5;
    bool isPatrolling = false;
    float finalPatrolTime;
    bool moveBack = false;

    Vector3 startPosition;



    void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        playerTransform = player.GetComponent<Transform>();
        currentState = States.Patrolling;
        startPosition = transform.position;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public override void Jump()
    {
        //
    }

    public override void Move()
    {
        if(isDead)
        {
            return;
        }

        if(currentState == States.Patrolling)
        {
            Patrol();
            if(Mathf.Abs(transform.position.y - playerTransform.position.y) < 1)
            {
                currentState = States.Moving;
            }
        }

        if(currentState == States.Moving)
        {
            MoveState();
        }

        if(currentState==States.Attacking)
        {
             AttackState();
            if(playerTransform.position.x < transform.position.x)
            {
                transform.localEulerAngles = new Vector3(0,0,0);
            }
            else if(playerTransform.position.x > transform.position.x)
            {
                transform.localEulerAngles = new Vector3(0,180,0);
            }
            if(playerTransform.position.x > transform.position.x - distanceBeforeAttack|| playerTransform.position.x < transform.position.x + distanceBeforeAttack)
            {
                currentState = States.Moving;
                
            }


        }

        Rotate();


    }

    void Patrol()
    {
        // Move towards patrol point which is distance added to spawn point
        if(transform.position.x < startPosition.x + patrolDistance && !moveBack)
        {
            rb.velocity = new Vector2(speed * Time.deltaTime,rb.velocity.y);
            anim.SetBool("Walk", true);
        }
        else if (transform.position.x >= startPosition.x + patrolDistance && !moveBack)
        {
            if(!isPatrolling)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Walk", false);
                finalPatrolTime = Time.time + Random.Range(patrolTime * 0.5f, patrolTime * 2);
                isPatrolling = true;
            }

            if(Time.time >= finalPatrolTime)
            {

                isPatrolling = false;
                moveBack = true;
            }
        }

        if(transform.position.x > startPosition.x && moveBack)
        {
            rb.velocity = new Vector2(-speed * Time.deltaTime,rb.velocity.y);
            anim.SetBool("Walk", true);           
        }
        else if (transform.position.x <= startPosition.x + patrolDistance && moveBack)
        {
            if(!isPatrolling)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Walk", false);
                finalPatrolTime = Time.time + Random.Range(patrolTime * 0.5f, patrolTime * 2);
                isPatrolling = true;
            }

            if(Time.time >= finalPatrolTime)
            {
                isPatrolling = false;
                moveBack = false;
            }
        }
    }

    public abstract void MoveState();


    public abstract void AttackState();

    public void OnSpawn()
    {
        speed = Random.Range(speed * 0.9f, speed * 1.1f);
    }


    public void TakeDamage(float dmg)
    {
        health-=dmg;
        UpdateHealthSlider();
        if(health<=0)
        {
            Die();
        }
    }

    public override void Die()
    {
        isDead = true;
        anim.SetBool("Walk",false);
        rb.velocity = Vector2.zero;
        int deathAnim = Random.Range(0,deathAnimations.Length);
        anim.Play(deathAnimations[deathAnim],-1,0f);
        healthSlider.gameObject.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
        rb.isKinematic = true;
        player.earnExp(expGiven);
        player.earnMoney(moneyGiven);
        Destroy(gameObject,5f);
        this.enabled = false;
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = health;
    }
}
