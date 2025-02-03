using UnityEngine;

public class BallCollision : MonoBehaviour
{

    public float initalSpeed;
    public float angularRange;
    
    private float speed;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        speed = initalSpeed;
    }
   
    void OnCollisionEnter(Collision other)
    {
           
        if (other.gameObject.CompareTag("Right Paddle") || other.gameObject.CompareTag("Left Paddle"))
        {
            speed *= 1.1f;
            Vector3 direction = rb.linearVelocity;
            
            float paddleCenter =  other.gameObject.transform.position.x;
            float ballLocation = GetComponent<Transform>().position.x;
            float paddleTop = paddleCenter + 1.25f;
            float percent = (ballLocation - paddleCenter) / (paddleTop - paddleCenter);
            direction.x = percent * angularRange;
            
            direction.z = speed;
            rb.linearVelocity = direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
