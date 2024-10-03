using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Which character threw the grenade
    public Character characterThrew;
    [SerializeField] int detonationTime;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionFX;
    [SerializeField] LayerMask explosionLayers;
    [SerializeField] AudioClip explosionSfx;

    public float damage;
    public bool isCritical = false;
    bool exploded = false;
    void Start()
    {
        StartCoroutine(explodeAfterTime());
        
    }

    IEnumerator explodeAfterTime()
    {
        yield return new WaitForSeconds(detonationTime);
        explode();
        
    }

    void explode()
    {
        if(exploded)
        {
            return;
        }
        exploded = true;
        GameObject fx =Instantiate(explosionFX,transform.position,quaternion.identity);
        List<Collider2D> results = Physics2D.OverlapCircleAll(transform.position,explosionRadius,explosionLayers).ToList();
        
        foreach(Collider2D result in results)
        {
            result.GetComponent<IDamageable>().TakeDamage(damage);
        }
        AudioManager.aMSingleton.PlayNormalSound(explosionSfx);
        if(results.Count != 0)
        {
            
            if(isCritical)
            {
                UI_Manager.UISingleton.CreateDamageText(transform.position,damage,Color.red);
            }
            else
            {
                UI_Manager.UISingleton.CreateDamageText(transform.position,damage,Color.white);
            }
        }

        Destroy(fx,5f);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Enemy")
        {
            explode();
        }
    }
    // Drawing radius for testing
/*     void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,explosionRadius);
    } */
}
