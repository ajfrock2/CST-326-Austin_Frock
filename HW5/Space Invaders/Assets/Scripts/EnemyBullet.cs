using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 5f);
    }
}
