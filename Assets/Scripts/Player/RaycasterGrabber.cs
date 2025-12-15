using UnityEngine;

public class RaycasterGrabber : MonoBehaviour
{
    public LayerMask layer;
    public Transform viewpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit ray;
        if(Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(transform.position,Vector3.forward,out ray, layer))
        {
            print("Hola");
        }
    }
}
