using UnityEngine;

public class Abilities : MonoBehaviour
{

    public bool leftPlayer;
    public GameObject ballPrefab;
    
    private bool hasJump = false;
    private bool hasDuplicate  = false;
    private float JUMP_HEIGHT = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasJump)
        {
            //Activates jump
            if ((leftPlayer && Input.GetKeyDown(KeyCode.A)) || (!leftPlayer && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

                //Allows all possible ball duplicates to also jump
                foreach (GameObject ball in balls)
                {
                    Rigidbody rb = ball.GetComponent<Rigidbody>();
                    Vector3 velocity = rb.linearVelocity;
                    rb.linearVelocity = new Vector3(velocity.x + JUMP_HEIGHT, velocity.y, velocity.z);
                }
                hasJump = false;
            }
        }

        if (hasDuplicate)
        {
            //Creates duplicate
            if(leftPlayer && Input.GetKeyDown(KeyCode.A))
            {
                hasDuplicate = false;
                
                GameObject newball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
                Rigidbody rb = newball.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(0f, 0f, -1f) * 300, ForceMode.Acceleration);
            }
            if(!leftPlayer && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                hasDuplicate = false;
                
                GameObject newball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
                Rigidbody rb = newball.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(0f, 0f, 1f) * 300, ForceMode.Acceleration);
            }
        }
        
    }

    public void SetHasJump()
    {
        hasJump = true;
        if (leftPlayer)
        {
            Debug.Log("Left Paddle got a jump powerup");
        }
        else
        {
            Debug.Log("Right Paddle got a jump powerup");
        }
    }

    public void SetHasDuplicate()
    {
        hasDuplicate = true;
        if (leftPlayer)
        {
            Debug.Log("Left Paddle got a duplicate powerup");
        }
        else
        {
            Debug.Log("Right Paddle got a duplicate powerup");
        }
    }
}
