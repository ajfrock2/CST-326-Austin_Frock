using Unity.VisualScripting;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    
    public static InteractionManager Instance { get; set; }
    
    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objHitByRaycast = hit.transform.gameObject;

            //Guns
            if (objHitByRaycast.GetComponent<Weapon>() && objHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoveredWeapon = objHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            //AmmoBox
            if (objHitByRaycast.GetComponent<AmmoBox>())
            {
                hoveredAmmoBox = objHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
            
            //Throwables
            if (objHitByRaycast.GetComponent<Throwable>())
            {
                hoveredThrowable = objHitByRaycast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                }
            }
            else
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
