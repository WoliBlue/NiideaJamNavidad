using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Figuras : MonoBehaviour
{
    [Header("Configuración")]
    public float timer;
    public float TiempoLimite = 5f;
    public bool isCorrect = false;
    public List<GameObject> partes =  new List<GameObject>();
    
    [Header("Respawn")]
    public Vector3 posicionInicial;
    public Quaternion rotacionInicial;
    private bool fueVendida = false;
    private bool estaSiendoAgarrada = false;
    
    [Header("Tipos de Figura")]
    public TipoFigura miTipo;
    
    [Header("Efecto Glow Navideño")]
    public Color colorGlowVerde = new Color(0f, 1f, 0f, 1f);
    public Color colorGlowRojo = new Color(1f, 0f, 0f, 1f);
    public float velocidadParpadeo = 2f;
    private Material materialOriginal;
    private Renderer rendererFigura;
    private float tiempoGlow = 0f;
    private bool tieneMaterialGlow = false;
    public GameObject destroyPrefab;

    public GameObject render;
    
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
        
        // Obtener renderer para el glow
        rendererFigura = GetComponent<Renderer>();
        if (rendererFigura == null)
        {
            rendererFigura = GetComponentInChildren<Renderer>();
        }
        
        if (rendererFigura != null)
        {
            materialOriginal = rendererFigura.material;
        }
    }

    void Update()
    {

        // Efecto glow navideño cuando es correcta
        if (isCorrect && GameManager.instance.willBuy && rendererFigura != null)
        {
            AplicarGlowNavideño();
        }
        else if (!isCorrect && tieneMaterialGlow)
        {
            // Quitar glow si ya no es correcta
            QuitarGlow();
        }
        
        // Solo contar tiempo si no está siendo agarrada y no fue vendida
        if (!estaSiendoAgarrada && !fueVendida)
        {
            float distancia = Vector3.Distance(transform.position, posicionInicial);
            
            if (distancia > 0.5f)
            {
                timer += Time.deltaTime;
                
                if (timer >= TiempoLimite)
                {
                    VolverAlMueble();
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
    
    void AplicarGlowNavideño()
    {
        if (!tieneMaterialGlow)
        {
            // Crear material con emisión
            rendererFigura.material.EnableKeyword("_EMISSION");
            tieneMaterialGlow = true;
        }
        
        // Parpadeo alternando entre verde y rojo (colores navideños)
        tiempoGlow += Time.deltaTime * velocidadParpadeo;
        float brillo = Mathf.PingPong(tiempoGlow, 1f);
        
        // Alternar entre verde y rojo cada segundo
        Color colorActual = (Mathf.FloorToInt(tiempoGlow) % 2 == 0) ? colorGlowVerde : colorGlowRojo;
        
        // Aplicar emisión con intensidad variable
        Color emissionColor = colorActual * Mathf.LinearToGammaSpace(brillo * 2f);
        rendererFigura.material.SetColor("_EmissionColor", emissionColor);
    }
    
    void QuitarGlow()
    {
        if (rendererFigura != null && tieneMaterialGlow)
        {
            rendererFigura.material.SetColor("_EmissionColor", Color.black);
            tieneMaterialGlow = false;
            tiempoGlow = 0f;
        }
    }
    
    public void SetCorrect(bool correcto)
    {
        isCorrect = correcto;
        if (!correcto)
        {
            QuitarGlow();
        }
    }
    
    public void SiendoAgarrada(bool agarrada)
    {
        estaSiendoAgarrada = agarrada;
        if (agarrada)
        {
            timer = 0;
        }
    }
    
    public void VolverAlMueble()
    {
        Debug.Log("Figura volviendo al mueble: " + miTipo);
        timer = 0;
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    void OnCollisionEnter(Collision collision)
    {

        if (rb.linearVelocity.y > 0.01 && rb.linearVelocity.x > 0.01)
        {
            print("ouch");
            StartCoroutine(Blink());
        }
    }

public IEnumerator Blink()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<RaycasterGrabber>().DropObject();
        gameObject.GetComponent<BoxCollider>().enabled=false;
        Instantiate(destroyPrefab,transform.position,transform.rotation);
        render.SetActive(false);
        rb.isKinematic=true;
        
        yield return new WaitForSeconds(5);
        render.SetActive(true);
        rb.isKinematic=false;
        gameObject.GetComponent<BoxCollider>().enabled=true;
    }

    public void VolverAlMuebleInstantaneo()
    {
        Debug.Log("Figura tirada a la papelera: " + miTipo);
        timer = 0;
        fueVendida = false;
        estaSiendoAgarrada = false;
        
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
        }
    }
    
    public void FueVendida()
    {
        fueVendida = true;
        QuitarGlow();
    }
}