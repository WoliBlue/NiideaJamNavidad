using UnityEngine;

public class RaycasterGrabber : MonoBehaviour
{
    public LayerMask layer;
    public Transform viewpoint;
    public int grabDistance = 3;
    private Camera camera;
    public DialogueBoxController dialogue;
    public bool hasObject;
    public GameObject grabbedObject;

    void Start()
    {
        camera = Camera.main;
    }
    [SerializeField] float talkDistance = 2;
    bool inConversation;



    void Interact()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (inConversation)
        {
            //DialogueBoxControllerMulti.instance.StartDialogue(Person.dialogueAsset.dialogue, npc.StartPosition, npc.npcName);
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, talkDistance))
            {
                //if (hitInfo.collider.gameObject.TryGetComponent(out NPC npc))
                //{
                //  DialogueBoxController.instance.StartDialogue(npc.dialogueAsset.dialogue, npc.StartPosition, npc.npcName);
                //}
            }
        }
    }
    public void GrabObject()
    {
        if (grabbedObject)
        {
            grabbedObject.transform.position = camera.transform.position + camera.transform.forward * grabDistance;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void DropObject()
    {
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (!rb) return;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;

        // Notificar a la figura que ya no está siendo agarrada
        Figuras figura = grabbedObject.GetComponent<Figuras>();
        if (figura != null)
        {
            figura.SiendoAgarrada(false);
        }
        grabbedObject = null;
        return;
    }
    private GameObject hoveredObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        // Raycast para detección de objetos (Hover Highlight)
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycast;

        if (Physics.Raycast(ray, out raycast, grabDistance))
        {
            GameObject hitObj = raycast.collider.gameObject;
            if (hitObj.CompareTag("objeto"))
            {
                if (hoveredObject != hitObj)
                {
                    ClearHover();
                    hoveredObject = hitObj;
                    Figuras f = hoveredObject.GetComponent<Figuras>();
                    if (f != null) f.ToggleSelectionHighlight(true);
                }
            }
            else
            {
                ClearHover();
            }
        }
        else
        {
            ClearHover();
        }

        // AGARRAR / SOLTAR
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (grabbedObject)
            {
                DropObject();
            }
            else if (hoveredObject != null)
            {
                grabbedObject = hoveredObject;
                ClearHover(); // Dejar de resaltar como 'hover' porque ahora está agarrado

                // Notificar a la figura que está siendo agarrada
                Figuras figura = grabbedObject.GetComponent<Figuras>();
                if (figura != null)
                {
                    figura.SiendoAgarrada(true);
                    figura.ToggleSelectionHighlight(false); // Quitar highlight de selección al agarrar
                }
            }
        }

        if (grabbedObject)
        {
            GrabObject();
        }

        Debug.DrawRay(ray.origin, ray.direction * grabDistance, Color.white);
    }

    private void ClearHover()
    {
        if (hoveredObject != null)
        {
            Figuras f = hoveredObject.GetComponent<Figuras>();
            if (f != null) f.ToggleSelectionHighlight(false);
            hoveredObject = null;
        }
    }
}