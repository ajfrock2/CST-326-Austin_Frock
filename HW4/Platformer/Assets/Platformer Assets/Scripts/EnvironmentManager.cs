using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Camera cam;
    public HUDManager hudManager;
    public GameObject questionBlock;

    
    private float questionBlockoffset;
    private float timeSinceQuestionBlock;
    void Start()
    {
        questionBlockoffset = 0;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHit = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);
            
            if (rayHit && screenHitInfo.transform.gameObject.CompareTag("Question"))
            {
                hudManager.GiveCoins(1);
            }
            
            if (rayHit && screenHitInfo.transform.gameObject.CompareTag("Brick"))
            {
                Destroy(screenHitInfo.transform.gameObject);
                hudManager.GiveScore(50);
            }
        }

        //Question block animation
        timeSinceQuestionBlock += Time.deltaTime;
        if (timeSinceQuestionBlock >= 0.8)
        {
            questionBlockoffset += 0.2f;
            
            questionBlock.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0f, questionBlockoffset));

            timeSinceQuestionBlock = 0;
        }
        
    }
}
