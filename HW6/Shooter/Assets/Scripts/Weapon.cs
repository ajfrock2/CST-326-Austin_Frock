using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    private static readonly int Recoil = Animator.StringToHash("Recoil");

    public bool isActiveWeapon;
    
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float bulletLifeTime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponModel
    {
        M1911,
        AK74
    }

    public ShootingMode currentShootingMode;
    public WeaponModel thisWeaponModel;
    
    //Shooting
    public bool isShooting, readyToShoot;
    public float shootingDelay = 2f;
    bool allowReset = true;
    
    //Burst
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;
    
    //Spread
    public float spreadIntensity;
    
    //Flash
    public GameObject muzzleEffect;

    internal Animator animator;
    
    //Reloading
    public float reloadTime = 2f;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();
        
        bulletsLeft = magazineSize;
    }
    
    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagSoundM1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //Hold down shoot
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            //Reloading
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                Reload();
            }


            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletPerBurst;
                FireWeapon();
            }
            
        }
    }

    private void FireWeapon()
    {
        
        bulletsLeft--;
        
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger(Recoil);
        
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionsAndSpread().normalized;
        //Instantiate, shoot, and destroy bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));

        //Check if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        
        animator.SetTrigger("Reload");
        
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    
    public Vector3 CalculateDirectionsAndSpread()
    {
       Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
       RaycastHit hit;

       Vector3 targetPoint;
       if (Physics.Raycast(ray, out hit))
       {
           //Hit something
           targetPoint = hit.point;
       }
       else
       {
           //Shooting air
           targetPoint = ray.GetPoint(10f);
       }
       Vector3 direction = targetPoint - bulletSpawn.position;
       
       float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
       float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
       
       //Returning shooting direction with spread
       return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(bullet);
    }
}
