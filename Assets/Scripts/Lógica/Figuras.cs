using UnityEngine;

public class Figuras : MonoBehaviour
{
    [Header("Configuración")]
    public float timer;
    public float TiempoLimite = 5f; // Tiempo en segundos antes de respawnear
    public bool isCorrect;
    
    [Header("Respawn")]
    public Vector3 posicionInicial;
    public Quaternion rotacionInicial;
    private bool fueVendida = false;
    private bool estaSiendoAgarrada = false;
    
    [Header("Tipos de Figura")]
    public TipoFigura miTipo;
    
    private Rigidbody rb;
    
    public enum TipoFigura
    {
        Caganer,
        Jesus,
        Maria,
        Jose,
        Melchor,
        Gaspar,
        Baltasar,
        Buey,
        Mula,
        Aldeano
    }

    void Start()
    {
        // Guardar posición inicial
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Volver a su posición inicial después de un tiempo si no es agarrada
        // Solo contar tiempo si no está siendo agarrada y no fue vendida
        if (!estaSiendoAgarrada && !fueVendida)
        {
            // Verificar si la figura está lejos de su posición inicial
            float distancia = Vector3.Distance(transform.position, posicionInicial);
            
            if (distancia > 0.5f) // Si está a más de 0.5 unidades de la posición inicial
            {
                timer += Time.deltaTime;
                
                if (timer >= TiempoLimite)
                {
                    VolverAlMueble();
                }
            }
            else
            {
                // Resetear el timer si está cerca de su posición
                timer = 0;
            }
        }
        
        if (isCorrect)
        {
            // FALTA POR AÑADIR QUE BRILLE CUANDO ES LA CORRECTA
        }
    }
    
    // Llamar esto cuando el RaycasterGrabber agarra la figura
    public void SiendoAgarrada(bool agarrada)
    {
        estaSiendoAgarrada = agarrada;
        if (agarrada)
        {
            timer = 0; // Resetear timer cuando se agarra
        }
    }
    
    // Volver al mueble después del tiempo límite
    public void VolverAlMueble()
    {
        Debug.Log("Figura volviendo al mueble: " + miTipo);
        timer = 0;
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        
        // Resetear física
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    
    // Volver al mueble instantáneamente (para la papelera)
    public void VolverAlMuebleInstantaneo()
    {
        Debug.Log("Figura tirada a la papelera: " + miTipo);
        timer = 0;
        fueVendida = false;
        estaSiendoAgarrada = false;
        
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        
        // Resetear física
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
        }
    }
    
    // Llamar esto cuando la figura es vendida
    public void FueVendida()
    {
        fueVendida = true;
        // La figura será destruida por MantelVenta
    }
}