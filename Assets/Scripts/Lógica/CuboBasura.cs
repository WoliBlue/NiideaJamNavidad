using UnityEngine;

public class Papelera : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip sonidoAplauso;
    private AudioSource audioSource;

    void Start()
    {
        // Añadir AudioSource si no existe
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
        // Verificar si es una figura
        Figuras figura = other.GetComponent<Figuras>();
        
        if (figura != null)
        {
            // Reproducir sonido de aplauso
            if (sonidoAplauso != null)
            {
                audioSource.PlayOneShot(sonidoAplauso);
            }
            
            // Hacer que la figura vuelva a su posición inicial instantáneamente
            figura.VolverAlMuebleInstantaneo();
        }
    }
}