using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    
    // Components
    [SerializeField] LayerMask jumpLayers;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;

    // Stats
    [SerializeField] protected string charName;
    [HideInInspector] public float baseSpeed;
    public float speed;
    public float health;
    [HideInInspector] public float baseMaxHealth;
    [SerializeField] public float maxHealth;
    public float jumpForce;
    [HideInInspector] public float baseJumpForce;
    protected int currentJumps = 0;
    public int maxJumps;
    protected int moveDirection;
    protected bool isDead = false;

    [Header("Stats")]
    public float fireRate =1;
    [HideInInspector] public float baseDamage;
    public int damage;
    public float critChance;
    public float healthRegenAmount;
    public float healthRegenTime;
    [HideInInspector] public float regenTimer =0;

    [SerializeField] protected string[] deathAnimations;


    
    public abstract void Move();
    
    public  abstract void Jump();


    protected void Rotate()
    {
        if(rb.velocity.x < 0)
        {
            transform.localEulerAngles = new Vector3(0,0,0);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localEulerAngles = new Vector3(0,180,0);
        }
    }



    public abstract void Die();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseMaxHealth = maxHealth;
        baseDamage = damage;
        baseSpeed = speed;
        baseJumpForce = jumpForce;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Ground")
        {
            

            Vector3 feetPos = transform.position - new Vector3(0.05f,1f,0);
            RaycastHit2D hit = Physics2D.Raycast(feetPos,-transform.up,1f,jumpLayers);
            
            Debug.DrawLine(feetPos,feetPos + -transform.up *1f,Color.red,5);

            if(!hit)
            {
                return;
            }

            if(hit.transform.tag=="Ground")
            {
                
                currentJumps = 0;
                anim.SetBool("Jump",false);
            }
           
        }
    }
}
