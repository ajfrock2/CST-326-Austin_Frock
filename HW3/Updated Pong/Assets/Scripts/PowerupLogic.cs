using UnityEngine;

public class PowerupLogic : MonoBehaviour
{
    public GameObject prefab;

    public Sprite[] images;
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnPowerup(float direction, float chance)
    {
        if (Random.Range(0f, 1f) < chance)
        {
            Vector3 randSpawn = new Vector3(Random.Range(-4f, 4f), 0f, 0f);
            GameObject newPowerup = Instantiate(prefab, transform.position + randSpawn, prefab.transform.rotation);
            SpriteRenderer spriteRenderer = newPowerup.GetComponent<SpriteRenderer>();
            
            int powerupIndex = Random.Range(0, images.Length);
            newPowerup.GetComponent<PowerupType>().SetIndex(powerupIndex);
            spriteRenderer.sprite = images[powerupIndex];
            
            Rigidbody rb = newPowerup.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0f, 0f, direction) * 90, ForceMode.Acceleration);
        }
    }
}
