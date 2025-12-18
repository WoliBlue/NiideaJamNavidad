using UnityEngine;

public class PosterCambiante : MonoBehaviour
{
// Texturas Poster1 y Poster2 para poner en el inspector
public Texture poster1;
public Texture poster2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Al hacer click en el PosterLargo que tenga de textura Poster1, 
    // que cambie su textura a Poster2
    private void OnMouseDown()
    {
        Renderer renderer = GetComponent<Renderer>();
        Texture currentTexture = renderer.material.mainTexture;

        // Asumiendo que las texturas están asignadas en el inspector
        Texture poster1 = Resources.Load<Texture>("Poster1");
        Texture poster2 = Resources.Load<Texture>("Poster2");

        if (currentTexture == poster1)
        {
            renderer.material.mainTexture = poster2;
        }
        else if (currentTexture == poster2)
        {
            renderer.material.mainTexture = poster1;
        }
    }
}
