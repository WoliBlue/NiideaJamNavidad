using UnityEngine;

public class Figuras : MonoBehaviour
{
    public float timer;
    public int TiempoLimite;
    public bool isCorrrect;
    // Esta es la clase de la Figura. Toda figura tiene estas propiedades y métodos.
    // Cuando una figura tiene que ser elegida por el jugador, se puede coger con el RaycasterGrabber
    // y se debe arrastrar hasta el mantel de venta.
    // en el Mueble siempre salen 4 figuras pero solo se puede elegir cuando la Persona ha decidido que quiere comprar
    // y te ha dicho cual de las figuras quiere. Solo puedes coger esa (brillará) y arrastrarla hasta el mantel de venta.
    void Start()
    {
        
    }


    void Update()
    {
        timer+= Time.deltaTime;
        if(timer>= TiempoLimite)
        {
            print("Respwaneando");
            timer=0;
        }
        if (isCorrrect)
        {
            //efectitos
        }
    }

    /*
    Tipos de figuras:
    Caganer
    Jesús
    María
    José
    Melchor
    Gaspar
    Baltasar
    Toro
    */
}
