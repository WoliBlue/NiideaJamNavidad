using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Toda la lógica del juego se controla desde aquí. Pilla que dia es del DataManager para saber que personas llamar
    // y que figuras poner en el mueble. 
    // También control (si es el ultimo dia) de si se ha conseguido el final bueno o malo.

    // Cuando se empieza el día, llama a la persona correspondiente de Persona.cs
    // y pone las figuras correspondientes en el mueble.
    // Después, va controlando la interacción entre el jugador y la persona.
    // Si la persona quiere comprar, activa la lógica para que el jugador pueda coger la figura
    
    // Cuando la persona se va, llama a la siguiente persona, hasta que se acaben las personas del día.
    // Al final del día, llama al DataManager para que guarde los datos de las figuras vendidas.


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
