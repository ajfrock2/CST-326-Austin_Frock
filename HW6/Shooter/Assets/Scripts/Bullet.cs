using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("hit" + collision.gameObject.name);
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("hit" + collision.gameObject.name);
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
            
        }
        
        if (collision.gameObject.CompareTag("Beer"))
        {
            Debug.Log("hit" + collision.gameObject.name);
            collision.gameObject.GetComponent<Bottle>().Explode();
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        
        hole.transform.SetParent(objectWeHit.gameObject.transform);

    }
}
