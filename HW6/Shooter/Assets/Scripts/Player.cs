using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
   public int HP = 100;
   public GameObject bloodyScreen;
   
   public TextMeshProUGUI playerHealthUI;
   public GameObject gameOverUI;

   public bool isDead;
   
   public CinemachineCamera deathCamera;
   private void Start()
   {
      playerHealthUI.text = $"Health: {HP}";   
   }

   public void TakeDamage(int damageAmount)
   {
      HP -= damageAmount;

      if (HP <= 0)
      {
         Debug.Log("Player Dead");
         PlayerDead();
         isDead = true;
      }
      else
      {
         Debug.Log("Player Hit");
         StartCoroutine(BloodyScreenEffect());
         playerHealthUI.text = $"Health: {HP}";  
         SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
      }
   }

   private void PlayerDead()
   {
      //Switch cams
      deathCamera.Priority = 50;
      
      SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);
      GetComponent<MouseMovement>().enabled = false;
      GetComponent<PlayerMovement>().enabled = false;

      //Dying animation
      GetComponentInChildren<Animator>().enabled = true;
      playerHealthUI.gameObject.SetActive(false);
      GetComponent<ScreenBlackout>().StartFade();
      StartCoroutine(ShowGameOverUI());
   }

   private IEnumerator ShowGameOverUI()
   {
      yield return new WaitForSeconds(1f);
      gameOverUI.SetActive(true);

      int waveSurvived = GlobalReferences.Instance.waveNumber;
      
      if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
      {
         SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
      }
      
      StartCoroutine(ReturnToMainMenu());
   }

   private IEnumerator ReturnToMainMenu()
   {
      yield return new WaitForSeconds(10f);
      
      SceneManager.LoadScene("MainMenu");
   }

   private IEnumerator BloodyScreenEffect()
   {
      if (bloodyScreen.activeInHierarchy == false)
      {
         bloodyScreen.SetActive(true);
      }
      
      var image = bloodyScreen.GetComponentInChildren<Image>();

      // Set the initial alpha value to 1 (fully visible).
      Color startColor = image.color;
      startColor.a = 1f;
      image.color = startColor;

      float duration = 2f;
      float elapsedTime = 0f;

      while (elapsedTime < duration)
      {
         // Calculate the new alpha value using Lerp.
         float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

         // Update the color with the new alpha value.
         Color newColor = image.color;
         newColor.a = alpha;
         image.color = newColor;

         // Increment the elapsed time.
         elapsedTime += Time.deltaTime;

         yield return null; ; // Wait for the next frame.
      }
      
      if (bloodyScreen.activeInHierarchy)
      {
         bloodyScreen.SetActive(false);
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("ZombieHand"))
      {
         if (isDead == false)
         {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
         }
      }
   }
}
