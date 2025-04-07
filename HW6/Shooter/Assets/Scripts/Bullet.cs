using UnityEngine;

public class Bullet : MonoBehaviour
{
    internal int bulletDamage;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            //Debug.Log("hit" + collision.gameObject.name);
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("hit" + collision.gameObject.name);
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
            
        }
        
        if (collision.gameObject.CompareTag("Beer"))
        {
            //Debug.Log("hit" + collision.gameObject.name);
            collision.gameObject.GetComponent<Bottle>().Explode();
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
           // Debug.Log("hit" + collision.gameObject.name);

            if (collision.gameObject.GetComponent<Enemy>().isDead == false)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
                CreateBloodSprayEffect(collision);
            }
            
            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject bloodSprayPrefab = Instantiate(GlobalReferences.Instance.bloodSprayEffect, contact.point, Quaternion.LookRotation(contact.normal));
        
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        
        hole.transform.SetParent(objectWeHit.gameObject.transform);

    }
}
