using UnityEngine;

public class LowRiderShooter : MonoBehaviour
{
    [SerializeField] B_LowRider boss;
    [SerializeField] string AttackAnim;
    [SerializeField] Animator anim;
    [SerializeField] Transform firePoint;
    [SerializeField] float range;
    [SerializeField] protected GameObject onHitFX;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] ParticleSystem shootFX;
    [SerializeField] float fireRate;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] int minBurstSize,maxBurstSize;
    [SerializeField] int fireDelay;
    int shotsFired = 0;
    float attackTimer;
    bool shouldShoot;
    public bool finishedShooting = false;

    void Update()
    {
        if(!shouldShoot)
        {
            return;
        }

        if(Time.time > attackTimer)
        {
            Shoot();
        }
    }

    public void StartBlasting()
    {
        if(!shouldShoot)
        {
            shouldShoot = true;
            shotsFired = Random.Range(minBurstSize,maxBurstSize);
        }

    }

    void Shoot()
    {
        if(shotsFired == 0)
        {
            finishedShooting = true;
            shouldShoot = false;
            return;
        }

        anim.Play(AttackAnim,-1,0f);
        attackTimer = Time.time + fireRate;
        AudioManager.aMSingleton.PlayPitchedSound(hitSfx);
        shootFX.Play();

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position,-transform.right,range,attackLayer);
        Debug.DrawLine(firePoint.position,firePoint.position + -transform.right * range,Color.red,5);

        if(hit)
        {

            hit.transform.GetComponent<IDamageable>().TakeDamage(boss.damage);
            GameObject fx = Instantiate(onHitFX,hit.point,onHitFX.transform.rotation);

            Destroy(fx,1f);
        }
        shotsFired--;
    }
}
