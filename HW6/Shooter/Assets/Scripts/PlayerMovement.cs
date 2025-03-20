using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    Vector3 velocity;
    
    bool isGrounded;
    bool isMoving;
    
    private CharacterController controller;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        //Move vector
        Vector3 move = transform.right * x + transform.forward * z;
        
        //Move player
        controller.Move(move * (speed * Time.deltaTime));
        
        //Jump check and set
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        //Falling down
        velocity.y += gravity * Time.deltaTime;
        
        //Jump action
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        lastPosition = transform.position;
    }
}
