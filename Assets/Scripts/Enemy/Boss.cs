using UnityEngine;

public abstract class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] protected Animator anim;
    protected Player p;
    protected Transform playerTransform;
    public string bossName;
    public float health;
    public float maxHealth;
    public float speed;
    public int damage;
    bool isDead = false;

    


    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        health = maxHealth;
        enableBossHealth();
        p = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerTransform = p.GetComponent<Transform>();
    }

    void enableBossHealth()
    {
        UI_Manager.UISingleton.enableBossInfo(health, bossName);
    }

    public void TakeDamage(float damage)
    {
        if(isDead)
        {
            return;
        }

        health -= damage;
        UI_Manager.UISingleton.updateBossHealth(this);
        if(health <= 0 )
        {
            Die();
            isDead = true;
        }
    }

    void Die()
    {   
        Flag.bossIsAlive = false;
       OnDeath();
    }

    public abstract void OnDeath();

    public abstract void Move();

    void Update()
    {
        if(isDead)
        {
            return;
        }
        Move();
    }
}
