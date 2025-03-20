using UnityEngine;

public class MouseMovement : MonoBehaviour
{
   public float sensitivity = 500f;
   public float topClamp = -90f;
   public float bottomClamp = 90f;
   
   float xRotation = 0f;
   float yRotation = 0f;
    void Start()
    {
        //Invisible cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        //Mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        
        //Rotations
        xRotation -= mouseY;
        yRotation += mouseX;
        
        //Clamping
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        
        //Apply
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
