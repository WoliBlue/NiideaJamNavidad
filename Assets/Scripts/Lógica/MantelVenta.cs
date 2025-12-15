using UnityEngine;

public class MantelVenta : MonoBehaviour
{
    public GameObject figura;
    // El mantel de venta es a donde se debe arrastrar una figura para venderla. 
    void Start()
    {
        
    }


    void Update()
    {
        
    }
        void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Figuras>())
        {
            Destroy(other.gameObject);
        }

    }
}
