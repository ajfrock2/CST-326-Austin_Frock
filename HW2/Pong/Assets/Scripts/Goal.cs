using UnityEngine;
using TMPro;


public class Goal : MonoBehaviour
{
    public bool scoredOnLeft;
    public GameObject spawnLocation;
    public GameObject ballPrefab;
    public TextMeshProUGUI scoreText;
    
    private static int player1Score;
    private static int player2Score;
    private float timer;
    private bool resetting;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 5f;
        resetting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 direction;
        if (scoredOnLeft)
        {
            direction = new Vector3(0f,0f,1f);
            player2Score++;
            Debug.Log("Right player scored  " + player1Score.ToString() + " - " + player2Score.ToString());
        }
        else
        {
            direction = new Vector3(0f,0f,-1f);
            player1Score++;
            Debug.Log("Left player scored  " + player1Score.ToString() + " - " + player2Score.ToString());
        }
        
        if (player1Score >=11 || player2Score >= 11)
        {
            if (player1Score >= 11)
            {
                scoreText.text = player1Score.ToString() + " - " + player2Score.ToString() + '\n' + "Game Over" + '\n' + "Left Player Wins" ;
            }
            else
            {
                scoreText.text = player1Score.ToString() + " - " + player2Score.ToString() + '\n' + "Game Over" + '\n' + "Right Player Wins" ;
            }
            player1Score = 0;
            player2Score = 0;
            timer = 0f;
            resetting = true;
        }

        if (timer >= 5f)
        {
            GameObject newball = Instantiate(ballPrefab, spawnLocation.transform.position, Quaternion.identity);
            Rigidbody rb = newball.GetComponent<Rigidbody>();
            rb.AddForce(direction * 300, ForceMode.Acceleration);
            SetScoreText();
        }

    } 
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!resetting || !(timer >= 5f)) return;
        Vector3 direction = Random.Range(0, 2) == 1 ? new Vector3(0f,0f,-1f) : new Vector3(0f,0f, 1f);
        GameObject newball = Instantiate(ballPrefab, spawnLocation.transform.position, Quaternion.identity);
        Rigidbody rb = newball.GetComponent<Rigidbody>();
        rb.AddForce(direction * 300, ForceMode.Acceleration);
        SetScoreText();
        resetting = false;
    }
    
    private void SetScoreText()
    {
        scoreText.text = player1Score.ToString() + " - " + player2Score.ToString();
    }
}
