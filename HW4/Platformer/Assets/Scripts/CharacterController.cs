using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float acceleration;
    public float maxRunSpeed;
    public float maxFallSpeed;
    public float jumpImpulse;
    public float jumpBoostForce;
    public float stoppingSpeed;
    public HUDManager hudManager;
    public GameObject stonePrefab;
    public Transform environmentRoot;
    public Camera cam;
    public bool isGrounded;
   
    Rigidbody rb;
    
    
    private Transform camTransform;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camTransform = cam.GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
        //Get and set Horizontal movement
        float horizontalAmount = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);
        
        //Horizontal and vertical clamping
        float horiSpeed = rb.linearVelocity.x;
        float vertSpeed = rb.linearVelocity.y;
        horiSpeed = Mathf.Clamp(horiSpeed, -maxRunSpeed, maxRunSpeed);
        vertSpeed = Mathf.Clamp(vertSpeed, -maxFallSpeed, maxFallSpeed);
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horiSpeed;
        newVelocity.y = vertSpeed;
        rb.linearVelocity = newVelocity;
        
        //Testing if on ground
        Collider cylinderCollider = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = + 0.8f;
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        //Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, Color.red, 0, false);
        
        //Jump controls
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
            }
            else
            {
                if (rb.linearVelocity.y > 0)
                {
                    rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
                }
            }
        }
        
        if (horizontalAmount == 0f)
        {
            //No more ice skates
            Vector3 decayedVelocity = rb.linearVelocity;
            decayedVelocity.x *= 1f - Time.deltaTime * stoppingSpeed;
            rb.linearVelocity = decayedVelocity;
        }
        else
        {
            //Direction facing
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }

        CheckBoxCollision();
        MoveCamera();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
        animator.SetBool("In Air", !isGrounded);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            Destroy(this.gameObject);
        }
    }

    private void MoveCamera()
    {
        cam.transform.position = new Vector3(transform.position.x + 3f, cam.transform.position.y, cam.transform.position.z);
    }

    //Checks if player jumps into box
    private void CheckBoxCollision()
    {
        Vector3 startPoint = transform.position;
        Collider marioCollider = GetComponent<Collider>();
        startPoint = new Vector3(startPoint.x, startPoint.y + marioCollider.bounds.extents.y, startPoint.z);
        
        Ray headRay = new Ray(startPoint, Vector3.up);
        if (Physics.Raycast(headRay, out RaycastHit hitInfo, marioCollider.bounds.extents.y + 0.1f))
        {
            if (hitInfo.collider.CompareTag("Brick"))
            {
               // Debug.Log("Hit Brick");
                hudManager.GiveScore(100);
                Destroy(hitInfo.collider.gameObject);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            }

            if (hitInfo.collider.CompareTag("Question"))
            {
                //Debug.Log("Hit Question");
                hudManager.GiveCoins(1);
                Destroy(hitInfo.collider.gameObject);
                GameObject newObjS = Instantiate(stonePrefab, environmentRoot);
                newObjS.transform.position = hitInfo.transform.position;
            }
        }
    }
}
