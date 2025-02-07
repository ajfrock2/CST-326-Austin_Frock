using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject ballPrefab;

    private Transform spawnLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 direction;
        if (Random.Range(0, 2) == 1)
        {
            spawnLocation = spawn1.transform;
            direction =  new Vector3(0f,0f,-1f);
        }
        else
        {
            spawnLocation = spawn2.transform;
            direction =  new Vector3(0f,0f, 1f);
        }
        
        GameObject newball = Instantiate(ballPrefab, spawnLocation.position, Quaternion.identity);
        Rigidbody rb = newball.GetComponent<Rigidbody>();
        rb.AddForce(direction * 200, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
