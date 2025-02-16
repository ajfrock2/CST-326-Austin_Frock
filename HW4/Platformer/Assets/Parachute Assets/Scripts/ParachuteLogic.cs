using System.Collections;
using UnityEngine;

public class ParachuteLogic : MonoBehaviour
{
    public float openDistance;
    public float fallSpeed;
    public float chuteOpenDuration;

    private float defaultDrag;

    public GameObject parachuteObj;
    public GameObject chuteObj;
    public Transform debugSphereTransform;
    public Camera cam;
    public Transform chutePivot;

    private bool hasParachuteOpened;
    void Start()
    {
        defaultDrag = parachuteObj.GetComponent<Rigidbody>().linearDamping;
        chuteObj.SetActive(false);

        //StartCoroutine(TestCoroutine());
    }

    /*IEnumerator TestCoroutine()
    {
        Debug.Log("Testing routine started");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Testing routine resuming");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Testing routine finishing");
    }
    */
    void Update()
    {   
        Ray groundObserverRay = new(parachuteObj.transform.position, Vector3.down);
        
        if (Physics.Raycast(groundObserverRay, out RaycastHit hitInfo))
        {
            bool chuteOpen = hitInfo.distance < openDistance && hitInfo.distance > 0.01f;
            
            Color lineColor = (hitInfo.distance < openDistance) ? Color.red : Color.blue;
            Debug.DrawRay(parachuteObj.transform.position, Vector3.down, lineColor);

            
            parachuteObj.GetComponent<Rigidbody>().linearDamping = (chuteOpen) ? fallSpeed : defaultDrag;
            chuteObj.SetActive(chuteOpen);

            if (chuteOpen && !hasParachuteOpened)
            {
                StartCoroutine(AnimateParachuteOpen());
                hasParachuteOpened = true;
            }
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            
            
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);
            if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("Brick"))
            {
                debugSphereTransform.position = screenHitInfo.point;
            }
        }
    }
    IEnumerator AnimateParachuteOpen()
    {
        float timeElpased = 0f;

        while (timeElpased < chuteOpenDuration)
        {
            float percentage = timeElpased / chuteOpenDuration;
            chutePivot.localScale = new Vector3(percentage, percentage, percentage);
            //Debug.Log(percentage);
            
            yield return null;
            timeElpased += Time.deltaTime;
        }
    }
}
