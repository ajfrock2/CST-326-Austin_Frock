using System;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 20f;
    [SerializeField] private float explosionForce = 1200f;

    private float countdown;
    
    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade
    }
    
    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0 && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Grenade:
                Smoke_GrenadeEffect();
                break;
        }
    }

    private void Smoke_GrenadeEffect()
    {
        //Visuals
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);
        
        //Sounds
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            //Physical effect
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Apply blindness
            }
        }
    }

    private void GrenadeEffect()
    {
        //Visuals
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        //Sounds
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            //Physical effect
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            
            //FUTURE Apply damage
        }
    }
}
