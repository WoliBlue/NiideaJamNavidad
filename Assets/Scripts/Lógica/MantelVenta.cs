using UnityEngine;

public class MantelVenta : MonoBehaviour
{
    public GameObject figura;

    [Header("Audio (opcional)")]
    public AudioClip sonidoVenta;
    private AudioSource audioSource;

    void Start()
    {
        // Sonido de Venta
        if (sonidoVenta != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.playOnAwake = false;
        }
    }

    // Detectar colisiones con figuras
    void OnTriggerEnter(Collider other)
    {
        Figuras figuraComponent = other.GetComponent<Figuras>();

        if (figuraComponent != null)
        {
            // Marcar como vendida
            figuraComponent.FueVendida();

            // Reproducir sonido de venta si existe
            if (sonidoVenta != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoVenta);
            }

            // Destruir la figura
            Destroy(other.gameObject);

            // Notificar al GameManager y DataManager
            Debug.Log("Figura vendida: " + figuraComponent.miTipo);
            GameManager.instance.VenderFigura();
            DataManager.instance.IncrementarFigurasVendidas();
        }
    }
}