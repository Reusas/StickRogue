using UnityEngine;

public class B_LowRider : Boss
{
    public enum BossStates
    {
        Spawning,
        Attacking,
        Driving,
    }

    public BossStates currentState;

    [SerializeField] float positionToDriveOnSpawn;
    [SerializeField] LowRiderShooter shooter;
    [SerializeField] LowRiderShooter shooter2;
    [SerializeField] ParticleSystem explosionEffect;
    bool hasLastPlayerPos;
    float lastPlayerX;
    public float drivingSpeed;
    bool facingLeft = false;

    void Awake()
    {
        positionToDriveOnSpawn = transform.position.x + positionToDriveOnSpawn;
    }

    public override void Move()
    {
        if(currentState == BossStates.Spawning)
        {
            if(transform.position.x < positionToDriveOnSpawn)
            {
                rb.velocity = new Vector2(speed * Time.deltaTime,0);
            }
            else
            {
                currentState = BossStates.Attacking;
            }
        }

        if(currentState == BossStates.Attacking)
        {
            rb.velocity = Vector2.zero;
            if(shooter.finishedShooting == false)
            {
                shooter.StartBlasting();
            }
            else if(shooter.finishedShooting)
            {
                if(shooter2.finishedShooting == false)
                {
                    shooter2.StartBlasting();
                }
                else if(shooter2.finishedShooting)
                {
                    currentState = BossStates.Driving;
                    shooter.finishedShooting=false;
                    shooter2.finishedShooting=false;
                }

            }
        }

        if(currentState == BossStates.Driving)
        {
            if(!hasLastPlayerPos)
            {
                hasLastPlayerPos = true;
                lastPlayerX = playerTransform.position.x;

                if(lastPlayerX < transform.position.x)
                {
                    transform.rotation = new Quaternion(0,0,0,0);
                    facingLeft = true;
                }
                else if(lastPlayerX > transform.position.x)
                {
                    transform.rotation = new Quaternion(0,180,0,0);
                    facingLeft = false;
                }
            }

            if(!facingLeft)
            {
                if(transform.position.x < lastPlayerX)
                {
                    rb.velocity = new Vector2(drivingSpeed* Time.deltaTime, rb.velocity.y);
                }
                else if(transform.position.x > lastPlayerX)
                {
                    hasLastPlayerPos = false;
                    currentState= BossStates.Attacking;
                }
            }
            else if(facingLeft)
            {
                if(transform.position.x > lastPlayerX)
                {
                    rb.velocity = new Vector2(-drivingSpeed* Time.deltaTime, rb.velocity.y);
                }
                else if(transform.position.x < lastPlayerX)
                {
                    hasLastPlayerPos = false;
                    currentState= BossStates.Attacking;
                }
            }

        }
    }

    public override void OnDeath()
    {
        anim.enabled = true;
        anim.Play("LowRiderDie",-1,0f);
        explosionEffect.Play();
        Destroy(this.gameObject,5f);
        UI_Manager.UISingleton.disableBossInfo();

        

    }
}
