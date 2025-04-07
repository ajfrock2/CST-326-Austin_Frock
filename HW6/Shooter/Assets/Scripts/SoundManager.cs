using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioClip shootingSoundM1911; 
    public AudioClip reloadSoundM1911;

    public AudioClip shootingSoundAK74;
    public AudioClip reloadSoundAK74;
    
    public AudioSource emptyMagSoundM1911; 
    public AudioSource shootingChannel;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;
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

    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.M1911:
                shootingChannel.PlayOneShot(shootingSoundM1911);
                break;
            case Weapon.WeaponModel.AK74:
                shootingChannel.PlayOneShot(shootingSoundAK74);
                break;
        }
    }
    
    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.M1911:
                shootingChannel.PlayOneShot(reloadSoundM1911);
                break;
            case Weapon.WeaponModel.AK74:
                shootingChannel.PlayOneShot(reloadSoundAK74);
                break;
        }
    }
}
