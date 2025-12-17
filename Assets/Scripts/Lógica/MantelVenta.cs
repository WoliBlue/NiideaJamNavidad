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

    void OnTriggerStay(Collider other)
    {
        Figuras figuraComponent = other.GetComponent<Figuras>();

        // Solo procesamos si hay una figura y NO está siendo agarrada por el jugador
        if (figuraComponent != null && !figuraComponent.EstaSiendoAgarrada)
        {
            // EXCEPCIÓN: Solo se permite en el mantel si ES la figura correcta Y está resaltada
            if (figuraComponent.EsFiguraObjetivo)
            {
                // ✓ VENTA EXITOSA
                Debug.Log("¡Venta exitosa! Figura: " + figuraComponent.miTipo);

                figuraComponent.FueVendida();
                GameManager.instance.VenderFigura();

                if (sonidoVenta != null)
                    audioSource.PlayOneShot(sonidoVenta, 0.5f);

                Destroy(other.gameObject);
                if (GameManager.instance.figuras.Contains(other.gameObject))
                    GameManager.instance.figuras.Remove(other.gameObject);

                Person currentClient = FindFirstObjectByType<PersonManager>().CurrentBuyer;
                if (currentClient != null) currentClient.ClientReceivedFigure();
            }
            else
            {
                // ✗ CUALQUIER OTRO CASO: Devolver al mueble
                // (Ya sea porque es la figura incorrecta o porque el cliente aún no está listo)
                Debug.Log("Figura devuelta al mueble: " + figuraComponent.miTipo);

                // Solo sonar el error si había una intención de compra activa
                if (sonidoError != null && GameManager.instance != null && GameManager.instance.willBuy)
                    audioSource.PlayOneShot(sonidoError, 0.3f);

                figuraComponent.VolverAlMueble();
            }
        }
    }
}