using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class I_MissileBullets : Item, IOnHit
{
    Player player;
    [Header("Attributes")]
    public int chanceToShoot = 10;
    [SerializeField] GameObject explosionFX;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask explosionLayers;
    [SerializeField] AudioClip explosionSfx;


    public void OnHit(RaycastHit2D hit)
    {
        int c = Random.Range(0,101);
        if(chanceToShoot * stacks > c)
        {
            Explode(hit);
        }
    }

    void Explode(RaycastHit2D hit)
    {
        Debug.Log("Boom");
        int damage = player.damage;

        GameObject fx =Instantiate(explosionFX,hit.point,Quaternion.identity);

        List<Collider2D> results = Physics2D.OverlapCircleAll(hit.point,explosionRadius,explosionLayers).ToList();
        
        foreach(Collider2D result in results)
        {
            result.GetComponent<IDamageable>().TakeDamage(damage);
            UI_Manager.UISingleton.CreateDamageText(hit.point,damage,Color.yellow);
        }

        AudioManager.aMSingleton.PlayNormalSound(explosionSfx);
        Destroy(fx,5f);
    }

    public override void PickUpEffect(Player p)
    {
        player = p;
        
        ItemManager.iM.items.Add(this);
    }
}
