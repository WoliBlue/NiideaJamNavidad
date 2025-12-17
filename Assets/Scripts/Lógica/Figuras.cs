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
    public List<GameObject> partes = new List<GameObject>();

    [Header("Respawn")]
    public Vector3 posicionInicial;
    public Quaternion rotacionInicial;
    private bool fueVendida = false;
    private bool estaSiendoAgarrada = false;
    public bool EstaSiendoAgarrada => estaSiendoAgarrada;
    public bool EsFiguraObjetivo => isCorrect && GameManager.instance != null && GameManager.instance.willBuy;

    [Header("Tipos de Figura")]
    public TipoFigura miTipo;

    [Header("Efecto Glow Navideño")]
    public Color colorGlowVerde = new Color(0f, 1f, 0f, 1f);
    public Color colorGlowRojo = new Color(1f, 0f, 0f, 1f);
    public float velocidadParpadeo = 2f;
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

    private List<Renderer> allRenderers = new List<Renderer>();
    private Vector3 originalScale;

    void Start()
    {
        // Guardar posición inicial
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Obtener TODOS los renderers para el glow (algunas figuras tienen partes)
        allRenderers.AddRange(GetComponentsInChildren<Renderer>());

        // Si hay una lista de partes asignada manualmente, asegurarnos de que estén incluidas
        foreach (GameObject parte in partes)
        {
            if (parte != null)
            {
                Renderer r = parte.GetComponent<Renderer>();
                if (r != null && !allRenderers.Contains(r))
                    allRenderers.Add(r);
            }
        }
    }

    void Update()
    {
        // Efecto glow navideño cuando es correcta y el cliente está listo para comprar
        if (isCorrect && GameManager.instance != null && GameManager.instance.willBuy)
        {
            AplicarGlowNavideño();
            AplicarPulsoEscala();
        }
        else if (tieneMaterialGlow || transform.localScale != originalScale)
        {
            // Quitar glow si ya no es correcta o ya no se debe comprar
            QuitarGlow();
            RestaurarEscala();
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
            foreach (Renderer r in allRenderers)
            {
                if (r != null)
                    r.material.EnableKeyword("_EMISSION");
            }
            tieneMaterialGlow = true;
        }

        // Parpadeo alternando entre verde y rojo (colores navideños)
        tiempoGlow += Time.deltaTime * velocidadParpadeo;
        float brillo = (Mathf.Sin(tiempoGlow * Mathf.PI) + 1f) / 2f; // Oscilación más suave

        // Alternar entre verde y rojo cada ciclo
        Color colorActual = (Mathf.FloorToInt(tiempoGlow * 0.5f) % 2 == 0) ? colorGlowVerde : colorGlowRojo;

        // Aplicar emisión con alta intensidad (multiplicamos por 4 para que brille en URP)
        Color emissionColor = colorActual * (brillo * 4f);

        foreach (Renderer r in allRenderers)
        {
            if (r != null)
                r.material.SetColor("_EmissionColor", emissionColor);
        }
    }

    void AplicarPulsoEscala()
    {
        float pulso = 1f + Mathf.Sin(Time.time * 5f) * 0.05f;
        transform.localScale = originalScale * pulso;
    }

    void RestaurarEscala()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 10f);
    }

    private bool selectionHighlighted = false;
    private Color selectionColor = new Color(1f, 1f, 1f, 0.5f); // White/Greyish for selection

    void QuitarGlow()
    {
        if (tieneMaterialGlow)
        {
            foreach (Renderer r in allRenderers)
            {
                if (r != null)
                {
                    r.material.SetColor("_EmissionColor", Color.black);
                }
            }
            tieneMaterialGlow = false;
            tiempoGlow = 0f;
        }
    }

    public void ToggleSelectionHighlight(bool active)
    {
        if (selectionHighlighted == active) return;
        selectionHighlighted = active;

        foreach (Renderer r in allRenderers)
        {
            if (r != null)
            {
                if (active)
                {
                    r.material.EnableKeyword("_EMISSION");
                    // Subtle selection glow if not already doing the objective glow
                    if (!tieneMaterialGlow)
                        r.material.SetColor("_EmissionColor", selectionColor * 0.2f);
                }
                else if (!tieneMaterialGlow)
                {
                    r.material.SetColor("_EmissionColor", Color.black);
                }
            }
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
        // Solo romper si el impacto es lo suficientemente fuerte
        if (collision.relativeVelocity.magnitude >= 2f)
        {
            // Debug.Log($"Impacto detectado: {collision.gameObject.name} con fuerza {collision.relativeVelocity.magnitude}");
            StartCoroutine(Blink());
        }
    }

    public IEnumerator Blink()
    {
        // 1. Soltar si el jugador lo tiene agarrado
        RaycasterGrabber playerGrabber = GameObject.FindGameObjectWithTag("Player")?.GetComponent<RaycasterGrabber>();
        if (playerGrabber != null && playerGrabber.grabbedObject == gameObject)
        {
            playerGrabber.DropObject();
        }

        // 2. Desactivar físicas y colisiones para el estado "roto"
        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null) myCollider.enabled = false;

        rb.isKinematic = true;
        if (destroyPrefab != null && render.activeInHierarchy)
        {
            Instantiate(destroyPrefab, transform.position, transform.rotation);
        }
        if (render != null)
        {
            render.SetActive(false);
        }

        // 3. Efecto visual de destrucción




        // 4. Esperar antes de respawnear
        yield return new WaitForSeconds(TiempoLimite);

        // 5. Resetear y volver al mueble
        VolverAlMueble();

        if (render != null)
        {
            render.SetActive(true);
        }

        rb.isKinematic = false;
        if (myCollider != null) myCollider.enabled = true;

        timer = 0;
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