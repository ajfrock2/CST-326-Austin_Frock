using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Player.playerDied += PlayerOnplayerDied;
    }

    private void PlayerOnplayerDied()
    {
        Invoke(nameof(LoadCreditScene), 2.5f);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadCreditScene()
    {
        SceneManager.LoadScene("Credits");
        Invoke(nameof(LoadMenuScene), 10.0f);
    }
    
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    void OnDestroy()
    {
        Player.playerDied -= PlayerOnplayerDied;
    }
}
