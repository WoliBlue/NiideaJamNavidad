using UnityEngine;

public class MantelVenta : MonoBehaviour
{
    public GameObject figura;
    
    [Header("Audio")]
    public AudioClip sonidoVenta;
    public AudioClip sonidoError; 
    private AudioSource audioSource;

    void Start()
    {
        // Añadir AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Sonido 3D
    }

    void OnTriggerEnter(Collider other)
    {
        Figuras figuraComponent = other.GetComponent<Figuras>();
        
        if (figuraComponent != null)
        {
            // VALIDACIÓN: Solo se puede vender si isCorrect es true
            if (figuraComponent.isCorrect)
            {
                // ✓ FIGURA CORRECTA - VENDER
                Debug.Log("¡Venta exitosa! Figura: " + figuraComponent.miTipo);
                
                // Marcar como vendida
                figuraComponent.FueVendida();
                GameManager.instance.VenderFigura();
                
                // Reproducir sonido de venta
                if (sonidoVenta != null)
                {
                    audioSource.PlayOneShot(sonidoVenta, 0.5f);
                }
                
                // Destruir la figura
                Destroy(other.gameObject);
                
                // TODO: Notificar al GameManager/DataManager
                // GameManager.instance.RegistrarVenta(figuraComponent.miTipo);
            }
            else
            {
                // ✗ FIGURA INCORRECTA - RECHAZAR
                Debug.Log("¡Error! Esta no es la figura correcta. Tipo: " + figuraComponent.miTipo);
                
                // Reproducir sonido de error
                if (sonidoError != null)
                {
                    audioSource.PlayOneShot(sonidoError, 0.5f);
                }
                
                // Hacer que vuelva a su posición original
                figuraComponent.VolverAlMueble();
            }
        }
    }
}