using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDied(int score);
    public static event EnemyDied OnEnemyDied;

    public bool isSpaceship;
    public int score;
    public GameObject bulletPrefab;
    private float projectileChance = 0.02f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        //Debug.Log("Ouch!");
        if (isSpaceship)
        {
           OnEnemyDied?.Invoke((Random.Range(0, 4) * 50) + 50); 
        }
        else
        {
            OnEnemyDied?.Invoke(score);
        }

        Destroy(gameObject);
    }

    void Start()
    {
        Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
        Player.playerDied += PlayerOnplayerDied;
        if (isSpaceship)
        {
            Destroy(gameObject, 10f);
        }
    }

    private void PlayerOnplayerDied()
    {
        if (isSpaceship)
        {
            Destroy(gameObject);
        }
    }

    private void EnemyOnOnEnemyDied(int i)
    {
        if (Random.Range(0f, 1.0f) < projectileChance)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        float spaceshipSpeed = 2f;
        if (isSpaceship)
        {
            transform.position += Vector3.right * (Time.deltaTime * spaceshipSpeed);
        }
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= EnemyOnOnEnemyDied;
        Player.playerDied -= PlayerOnplayerDied;
    }
}
