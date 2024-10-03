using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Character,IDamageable
{
    public bool PCControlls;
    BoxCollider2D pCollider;
    public int level =1;
    public int exp = 0;
    public int expUntilNextLevel = 100;
    public int money;
    public float moneyEarnedMultiplier =1;
    public float dodgeStacks = 0;
    public float dodgeChance = 0.1f;

    [SerializeField] float climbSpeed;
    bool isClimbing;
    bool canClimb;
    float climbDir = 0;
    MobileJoyStick joyStick;
    public CharacterAttack attacks;
    bool isAttacking = false;

    public float moveDir =0;

    float nextAttackTime;
    
    public void earnExp(int expEarned)
    {
        exp += expEarned;
        if(exp>= expUntilNextLevel)
        {
            level++;
            LevelUp();
            // Make current exp the min value for the slider.
            
        }
        UI_Manager.UISingleton.UpdatePlayerExp(this);
    }

    public void earnMoney(int moneyEarned)
    {
        money +=Mathf.RoundToInt(moneyEarned * moneyEarnedMultiplier);
        UI_Manager.UISingleton.UpdatePlayerMoney(this);
    }

    void LevelUp()
    {
        expUntilNextLevel = expUntilNextLevel * 2;
        expUntilNextLevel += expUntilNextLevel;
        UI_Manager.UISingleton.UpdatePlayerExpSlider(this);
        // Increase max health and dmg by 10%
        maxHealth += baseMaxHealth * 0.1f;
        damage += Mathf.RoundToInt(baseDamage * 0.1f);
        UI_Manager.UISingleton.UpdatePlayerHealth(this);
    }


    void Awake()
    {
        joyStick = FindObjectOfType<MobileJoyStick>();
        pCollider = GetComponent<BoxCollider2D>();
        SetMobileControls();
        Application.targetFrameRate = 60;
        UI_Manager.UISingleton.UpdatePlayerHealth(this);
    }
    
    // Sets the functions to the on screen buttons on spawn using eventtriggers.
    void SetMobileControls()
    {
        EventTrigger trigger = GameObject.Find("JumpButton").GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) =>{Jump(); } );
        trigger.triggers.Add(entry);

        trigger = GameObject.Find("Button2").GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) =>{autoAttack(); } );
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) =>{stopAutoAttack(); } );
        trigger.triggers.Add(entry);
       // attacks.attackButtonText = trigger.GetComponentInChildren<TMP_Text>();

        trigger = GameObject.Find("Button3").GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) =>{secondaryAttack(); } );
        trigger.triggers.Add(entry);
        attacks.attackButtonText2 = trigger.transform.Find("CoolDownText").GetComponent<TMP_Text>();



         trigger = GameObject.Find("Button4").GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) =>{thirdAttack(); } );
        trigger.triggers.Add(entry); 
        attacks.attackButtonText3 = trigger.transform.Find("CoolDownText").GetComponent<TMP_Text>();

    }

    public override void Move()
    {
        AttemptDodge();
        // Temporary movement. Should be controled by a joystick later.
        // The 15 makes the default speed of 100 be good without it being 2000 in the editor.
        moveDir =joyStick.verticalAxis * speed  * Time.deltaTime;  
        if(PCControlls)
        {
            moveDir = Input.GetAxis("Horizontal") * speed  * Time.deltaTime;  
        } 


        if(moveDir !=0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk",false);
            
        }

        if(!isClimbing)
        {
            rb.velocity = new Vector2(moveDir,rb.velocity.y);
 
        }
        else
        {

            rb.velocity = new Vector2(0,climbDir);
        }
        Rotate();

        Climb();
    }

    public override void Jump()
    {
        if(isClimbing)
        {
            isClimbing = false;
            pCollider.isTrigger = false;
            return;
        }
                 
        if(currentJumps < maxJumps)
        {
            currentJumps++;
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            anim.SetBool("Jump",true);
            ItemManager.iM.OnJump();
  
        }   

        
    
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag!= "Climbable")
        {
            return;
        }
        canClimb = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform.tag!= "Climbable")
        {
            return;
        }
        canClimb = false;
        isClimbing = false;
        pCollider.isTrigger = false;
        currentJumps =0;
    }

    void Climb()
    {
        if(canClimb && !isClimbing)
        {
            if(joyStick.horizontalAxis > 0 || joyStick.horizontalAxis < 0 || Input.GetAxis("Vertical") !=0)
            {
                Debug.Log("Climbing started");
                isClimbing = true;
                pCollider.isTrigger = true;
            }

        }

        if(isClimbing)
        {

            climbDir = joyStick.horizontalAxis * climbSpeed  * Time.deltaTime;
            if(PCControlls)
            {
                climbDir = Input.GetAxis("Vertical") * climbSpeed  * Time.deltaTime;
            }
            
        }
    }

    void Update()
    {
        if(isAttacking)
        {
            Attack();
        }
        RegenHealth();

            if(Input.GetKeyDown(KeyCode.Space) && PCControlls)
            {
                Jump();
            }
    }

    public void autoAttack()
    {
        isAttacking = true;
    }

    public void stopAutoAttack()
    {
        isAttacking = false;
    }

    public void Attack()
    {
        //if(canAttack)
       // {
            if(Time.time <nextAttackTime)
            {
                return;
            }

            bool attack = attacks.Attack();
            if(attack)
            {
                anim.Play("SoldierShoot",-1,0f);
            }
           // canAttack = false;
            //StartCoroutine(resetAttack(fireRate));          
            nextAttackTime = Time.time+fireRate;
       // }


    }


    public void secondaryAttack()
    {
        bool attack = attacks.secondaryAttack();
        if(attack)
        {
            anim.Play("SoldierShoot",-1,0f);
        }

    }

    public void thirdAttack()
    {
        bool attack = attacks.thirdAttack();
        if(attack)
        {
           
        }
    }

    public void TakeDamage(float damage)
    {
        if(AttemptDodge())
        {
            Debug.Log("Dodged!");
            return;
        }
        health-=damage;
        UI_Manager.UISingleton.UpdatePlayerHealth(this);
        if(health<=0)
        {
            Die();
        }
    }

    public bool AttemptDodge()
    {
        float chance = (dodgeChance * dodgeStacks / (dodgeChance * dodgeStacks + 1)) * 100;
        int roll = Random.Range(0,101);
        if(chance > roll)
        {
            //anim.Play("Flash",-1,0f);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        int deathAnim = Random.Range(0,deathAnimations.Length);
        anim.Play(deathAnimations[deathAnim],-1,0f);
    }

    void RegenHealth()
    {
        
        if(health< maxHealth && Time.time >= regenTimer)
        {
            regenTimer = Time.time + healthRegenTime;
            health += healthRegenAmount;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
            UI_Manager.UISingleton.UpdatePlayerHealth(this);
            UI_Manager.UISingleton.CreateDamageText(transform.position,(int)healthRegenAmount,Color.green);
        }
    }


}
