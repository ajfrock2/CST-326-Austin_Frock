using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    private static readonly int Recoil = Animator.StringToHash("Recoil");
    private static readonly int IsAds = Animator.StringToHash("isADS");

    public bool isActiveWeapon;
    public int weaponDamage;
    
    [Header("Bullet")]
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float bulletLifeTime = 3f;
    //Flash
    public GameObject muzzleEffect;
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
    
    [Header("Shoot")]
    //Shooting
    public bool isShooting, readyToShoot;
    public float shootingDelay = 2f;
    bool allowReset = true;
    
    [Header("Burst")]
    //Burst
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;
    
    [Header("Spread")]
    //Spread
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;
    

    internal Animator animator;
    
    [Header("Reload")]
    //Reloading
    public float reloadTime = 2f;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private bool isADS;
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();
        
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
    }
    
    void Update()
    {
        if (isActiveWeapon)
        {
            //Camera clipping, TODO issue with pistol, not children of chidren are on layer causing clip issues
            gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }
            
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
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0 && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }


            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletPerBurst;
                FireWeapon();
            }
            
        }
        else
        {
            //Camera clipping 
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.reticle.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    
    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.reticle.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {
        
        bulletsLeft--;
        
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionsAndSpread().normalized;
        //Instantiate, shoot, and destroy bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;
        
        
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));

        if (isADS)
        {
            animator.SetTrigger("ADS Recoil");
        }
        else
        {
            animator.SetTrigger(Recoil);
        }
        
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
        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
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
           if (hit.distance > 2f)
           {
               targetPoint = hit.point;
           }
           //Dealing with ADS bug
           else
           {
               targetPoint = ray.GetPoint(10f);
           }
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
