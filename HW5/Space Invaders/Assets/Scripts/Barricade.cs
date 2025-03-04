using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int healthPoints;
    
    void Start()
    {
        Player.playerDied += PlayerOnplayerDied;
    }

    private void PlayerOnplayerDied()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        healthPoints--;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        
    }

    void OnDestroy()
    {
        Player.playerDied -= PlayerOnplayerDied;
    }
}
