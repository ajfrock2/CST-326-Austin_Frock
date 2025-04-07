using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;
    
    public float spawnDelay = 0.5f;
    
    public int currentWave = 0;
    public float waveCooldown = 10f;

    public bool inCooldown;
    public float cooldownCounter;

    public List<Enemy> currentZombiesAlive;
    
    public GameObject zombiePrefab;

    public TextMeshProUGUI WaveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;
        
        GlobalReferences.Instance.waveNumber = currentWave;
        
        currentWaveUI.text = "Wave: " + currentWave.ToString();
        
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            //Random spawn offset
            Vector3 spawnOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            Vector3 spawnPosition = transform.position + spawnOffset;
            
            //Instantiate
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();
            
            currentZombiesAlive.Add(enemyScript);
            
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        //Get dead zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        
        //Removing them
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        
        zombiesToRemove.Clear();
        
        //Start cooldown when all zombies are dead
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
        cooldownCounterUI.text = cooldownCounter.ToString("F0");
        
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        WaveOverUI.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(waveCooldown);
        
        inCooldown = false;
        WaveOverUI.gameObject.SetActive(false);

        currentZombiesPerWave *= 2;
        StartNextWave();
    }
}
