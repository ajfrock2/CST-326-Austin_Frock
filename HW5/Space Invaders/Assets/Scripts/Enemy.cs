using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private static readonly int Died = Animator.StringToHash("Died");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Idle = Animator.StringToHash("Idle");

    public delegate void EnemyDied(int score);
    public static event EnemyDied OnEnemyDied;

    public bool isSpaceship;
    public int score;
    public GameObject bulletPrefab;
    public AudioClip deathNoise;
    
    private float projectileChance = 0.02f;
    private Animator animator;
    private float animationSpeed = 1f;
    private bool alive = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            Destroy(collision.gameObject);
            if (isSpaceship)
            {
                OnEnemyDied?.Invoke((Random.Range(0, 4) * 50) + 50);
            }
            else
            {
                OnEnemyDied?.Invoke(score);
            }

            animator.SetTrigger(Died);
            Destroy(gameObject, 0.5f); //Based on animation speed
            alive = false;
            AudioSource.PlayClipAtPoint(deathNoise, transform.position);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animationSpeed = 1f;
        Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
        Player.playerDied += PlayerOnplayerDied;
        objectManager.speedIncreased += ObjectManagerOnspeedIncreased;
        
        if (isSpaceship)
        {
            Destroy(gameObject, 10f);
        }
    }

    private void ObjectManagerOnspeedIncreased()
    {
        animationSpeed *= 1.2f;
        animator.speed = animationSpeed;
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
        //Shooting
        if (Random.Range(0f, 1.0f) < projectileChance && !isSpaceship)
        {
            Invoke(nameof(SpawnBullet), 1f);
            animator.SetTrigger(Shoot);
        }
    }

    void SpawnBullet()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        //Sync movements
        if (!isSpaceship)
        {
            animator.SetFloat(Idle, Mathf.Repeat(Time.time, 1f));
        }
        
        //Spaceship movement
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
        objectManager.speedIncreased -= ObjectManagerOnspeedIncreased;
    }
}
