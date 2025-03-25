using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Ammo")] 
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")] 
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")] 
    public Image lethalUI;
    public TextMeshProUGUI lethalAmontUI;
    
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmontUI;

    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject reticle; 
    public static HUDManager Instance { get; set; }
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
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
            
            ammoTypeUI.sprite = emptySlot;
            
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.lethalCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }
        if (WeaponManager.Instance.tacticalCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("M1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.AK74:
                return Resources.Load<GameObject>("AK74_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.AK74:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    public void UpdateThrowables()
    {
        lethalAmontUI.text = $"{WeaponManager.Instance.lethalCount}";
        tacticalAmontUI.text = $"{WeaponManager.Instance.tacticalCount}";
        
        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
