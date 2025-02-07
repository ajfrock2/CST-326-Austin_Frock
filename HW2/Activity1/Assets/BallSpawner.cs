using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject ballPrefab;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPos = spawnLocation.position;
            
            GameObject newBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        }

    }
}
