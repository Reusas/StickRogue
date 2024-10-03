using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    bool exploded;
    public bool playersMine;
    public float damage = 50;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionFX;
    [SerializeField] AudioClip explosionSfx;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;

    
    void Start()
    {
        StartCoroutine(Dissapear());
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(20f);
        I_MineBelt.currentMines--;
        Destroy(this.gameObject);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(playersMine)
        {
            if(col.transform.tag=="Enemy")
            {
                explode(enemyLayer);
            }
        }
        else
        {
            if(col.transform.tag=="Player")
            {
                explode(playerLayer);
            }
        }
    }

    void explode(LayerMask lM)
    {
        if(exploded)
        {
            return;
        }
        exploded = true;
        GameObject fx =Instantiate(explosionFX,transform.position,Quaternion.identity);
        List<Collider2D> results = Physics2D.OverlapCircleAll(transform.position,explosionRadius,lM).ToList();
        
        foreach(Collider2D result in results)
        {
            result.GetComponent<IDamageable>().TakeDamage(damage);
        }

        AudioManager.aMSingleton.PlayNormalSound(explosionSfx);      
        UI_Manager.UISingleton.CreateDamageText(transform.position,damage,Color.white);
        
        I_MineBelt.currentMines--;
        Destroy(fx,5f);
        Destroy(gameObject);

    }

        // Drawing radius for testing
/*     void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,explosionRadius);
    }  */
}
