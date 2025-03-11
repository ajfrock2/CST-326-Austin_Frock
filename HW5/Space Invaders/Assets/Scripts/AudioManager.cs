using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private AudioSource audioSource;

    void Start()
    {
        Player.playerDied += PlayerOnplayerDied;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void PlayerOnplayerDied()
    {
        audioSource.Stop();
    }
    

    void Update()
    {
        
    }

    void OnDestroy()
    {
        Player.playerDied -= PlayerOnplayerDied;
    }
}
