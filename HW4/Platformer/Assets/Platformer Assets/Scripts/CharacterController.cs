using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float jumpImpulse;
    public float jumpBoostForce;
    Rigidbody rb;
    
    public bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontalAmount = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);
        
        float horiSpeed = rb.linearVelocity.x;
        horiSpeed = Mathf.Clamp(horiSpeed, -maxSpeed, maxSpeed);
        
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horiSpeed;
        rb.linearVelocity = newVelocity;
        
        //Todo clamp vertical velocity
        
        Collider collider = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = collider.bounds.extents.y + 0.01f;
        
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, Color.red, 0, false);


        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
            }
        }
        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
        }

        if (horizontalAmount == 0f)
        {
             
            Vector3 decayedVelocity = rb.linearVelocity;
            newVelocity.x *= 1f - Time.deltaTime * 4f;
            rb.linearVelocity = decayedVelocity;
        }
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }

    }
}
