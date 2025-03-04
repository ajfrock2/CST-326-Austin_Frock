using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
  public GameObject bulletPrefab;
  public Transform shoottingOffset;
  public float movementSpeed;
  
  private Animator animator;
  private Rigidbody2D rb2d;
  private bool shotFired = false;
  
  public delegate void PlayerDied();
  public static event PlayerDied playerDied;
    void Start()
    {
      Bullet.shotFired += BulletOnshotFired;
      animator = GetComponent<Animator>();
    }
    
    void Update()
    {
      //Shoot functions
      if (Input.GetKeyDown(KeyCode.Space) && !(shotFired))
      {
        GameObject shot = Instantiate(bulletPrefab, shoottingOffset.position, Quaternion.identity);
        animator.SetTrigger("Shoot");
        Destroy(shot, 1.5f);
        shotFired = true;
      }
      
      
      //Movement
      float moveHorizontal = Input.GetAxis("Horizontal"); 
      Vector3 movement = new Vector3(moveHorizontal, 0f, 0f) * movementSpeed * Time.deltaTime;
      transform.Translate(movement, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      Destroy(gameObject);
      playerDied?.Invoke();
    }

    private void BulletOnshotFired()
    {
      //Bullet destroyed
      shotFired = false;
    }
    
    void OnDestroy()
    { 
      Bullet.shotFired -= BulletOnshotFired;
    }
}
