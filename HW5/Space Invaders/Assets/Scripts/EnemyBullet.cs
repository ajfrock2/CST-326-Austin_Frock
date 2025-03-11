using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    public AudioClip shootSound;

    
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 5f);
    }
    
    void Awake()
    {
        AudioSource.PlayClipAtPoint(shootSound, transform.position, 0.5f);
    }
}
