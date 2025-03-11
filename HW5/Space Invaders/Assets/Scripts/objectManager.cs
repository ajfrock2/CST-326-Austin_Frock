using System;
using Unity.VisualScripting;
using UnityEngine;

public class objectManager : MonoBehaviour
{
    public GameObject alien1Prefab;
    public GameObject alien2Prefab;
    public GameObject alien3Prefab;
    public GameObject spaceshipPrefab;
    public GameObject playerPrefab;
    public GameObject barricadePrefab;
    public int enemiesPerRow;
    public Transform topLeftSpawn;
    public Transform leftBarricadeSpawn;
    public float minimumSpeed;

    private float horizontalSpacing = 1f;
    private float verticalSpacing = 1f;
    private float moveDistance = 0.2f;
    private float timeSinceMove = 0f;
    private bool gameStarted = false;
    private float moveInterval = 0;
    private int stepsTaken = 0;
    private bool movingRight = true;
    private int enemiesKilled = 0;
    
    public delegate void SpeedIncreased();
    public static event SpeedIncreased speedIncreased;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInterval = minimumSpeed;
        Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
        HUDManager.onGameStart += HUDManagerOnonGameStart;
        Player.playerDied += PlayerOnplayerDied;
    }

    private void PlayerOnplayerDied()
    {
        //Destroy enverything and reset stats
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Invoke("HUDManagerOnonGameStart", 3f);
        ResetStats();
    }

    void SpawnRow(float yPos, GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = yPos;
        for (int i = 0; i < enemiesPerRow; i++)
        {
            Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
            spawnPosition.x += horizontalSpacing;
        }
    }

    void SpawnEnemies()
    {
        float initialY = transform.position.y;
        SpawnRow(initialY, alien3Prefab);
        SpawnRow(initialY - verticalSpacing, alien2Prefab);
        SpawnRow(initialY - (verticalSpacing * 2), alien2Prefab);
        SpawnRow(initialY - (verticalSpacing * 3), alien1Prefab);
        SpawnRow(initialY - (verticalSpacing * 4), alien1Prefab);
        gameStarted = true;
    }
    private void HUDManagerOnonGameStart()
    {
        SpawnPlayer();
        SpawnEnemies();
        SpawnBarricades();
    }

    private void SpawnBarricades()
    {
        float distanceOffset = 4f;
        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPosition = leftBarricadeSpawn.position;
            spawnPosition.x += distanceOffset * i;
            Instantiate(barricadePrefab, spawnPosition, barricadePrefab.transform.rotation);
            spawnPosition.x += 0.75f;
            Instantiate(barricadePrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
    }

    private void EnemyOnOnEnemyDied(int score)
    {
        if (score > 49) return; //Handling spaceship
        enemiesKilled += 1;
        //Increase speed
        if (enemiesKilled % 10 == 0)
        {
            moveInterval *= 0.80f;
            speedIncreased?.Invoke();
        }
        //Spawn spaceship
        if (enemiesKilled % 20 == 0)
        {
            SpawnSpaceship();
        }
    }

    private void SpawnSpaceship()
    {
        //To make it spawn off-screen and move in 
        Vector3 adjustedSpawnPosition = topLeftSpawn.position;
        adjustedSpawnPosition.x -= 2f;
        Instantiate(spaceshipPrefab, adjustedSpawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceMove += Time.deltaTime;

        if (gameStarted)
        {
            //Right movement
            if (movingRight && timeSinceMove >= moveInterval)
            {
                //Vertical
                if (stepsTaken == 16)
                {
                    Vector3 newLocation = transform.position;
                    newLocation.y -= verticalSpacing * 0.5f;
                    transform.position = newLocation;
                    movingRight = false;
                    timeSinceMove = 0f;
                }
                //Horizontal
                else
                {
                    Vector3 newLocation = transform.position;
                    newLocation.x += moveDistance;
                    transform.position = newLocation;
                    timeSinceMove = 0f;
                    stepsTaken += 1;
                }
            }
            //Left movement
            if (!movingRight && timeSinceMove >= moveInterval)
            {
                //Vertical
                if (stepsTaken == -16)
                {
                    Vector3 newLocation = transform.position;
                    newLocation.y -= verticalSpacing * 0.5f;
                    transform.position = newLocation;
                    movingRight = true;
                    timeSinceMove = 0f;
                }
                //Horizontal
                else
                {
                    Vector3 newLocation = transform.position;
                    newLocation.x -= moveDistance;
                    transform.position = newLocation;
                    timeSinceMove = 0f;
                    stepsTaken -= 1;
                }
            }
            
            
        }

        //All enemies are dead
        if (enemiesKilled == 55)
        {
            minimumSpeed *= 0.9f;
            ResetStats();
            SpawnEnemies();
        }
    }

    private void ResetStats()
    {
        moveInterval = minimumSpeed;
        enemiesKilled = 0;
        stepsTaken = 0;
        transform.position = topLeftSpawn.position;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDied -= EnemyOnOnEnemyDied;
        HUDManager.onGameStart -= HUDManagerOnonGameStart;
        Player.playerDied -= PlayerOnplayerDied;
    }
}

