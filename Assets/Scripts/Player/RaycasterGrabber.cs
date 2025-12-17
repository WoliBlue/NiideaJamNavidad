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
        if (inConversation)
        {
            DialogueBoxController.instance.SkipLine();
        }
        else
        {
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitInfo, talkDistance))
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
    void Update()
    {
                if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        RaycastHit raycast;

        // Si ya tenemos un objeto y hacemos click, lo soltamos
        if (grabbedObject && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
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

        // Raycast para agarrar objetos
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out raycast))
            {
                if (raycast.collider.gameObject.tag == "objeto")
                {
                    grabbedObject = raycast.transform.gameObject;

                    // Notificar a la figura que está siendo agarrada
                    Figuras figura = grabbedObject.GetComponent<Figuras>();
                    if (figura != null)
                    {
                        figura.SiendoAgarrada(true);
                    }
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.white);
        }

        GrabObject();
    }
}