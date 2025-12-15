using UnityEngine;

public class RaycasterGrabber : MonoBehaviour
{
    public LayerMask layer;
    public Transform viewpoint;
    public int grabDistance;
    private Camera camera;

    public bool hasObject;

    public GameObject grabbedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera=Camera.main;
    }
    public void GrabObject()
    {
        if (grabbedObject)
        {
            
        
        grabbedObject.transform.position = camera.transform.position + camera.transform.forward * grabDistance;
        grabbedObject.GetComponent<Rigidbody>().useGravity=false;
        grabbedObject.GetComponent<Rigidbody>().isKinematic=false;
       
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycast;
        if (grabbedObject && Input.GetKeyDown(KeyCode.Mouse0))
        {
            grabbedObject.GetComponent<Rigidbody>().useGravity=true;
            grabbedObject.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
            grabbedObject=null;
            
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Physics.Raycast(ray,out raycast))
            {

           if (raycast.collider.gameObject.tag=="objeto")
            {
                //print("hola");
                grabbedObject=raycast.transform.gameObject;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.white);
    }
    GrabObject();
}
}
