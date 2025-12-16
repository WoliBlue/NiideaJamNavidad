using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] figuras;

    public GameObject[] personas;

    public int clientesPorDia = 2;
    public int clientesTotalesDelDia = 0;
    public bool hasActivePerson = false;
    public int activePersonIndex = 0;
    public bool hasActiveFigures = false;
    public GameObject activeFigure;
    public bool willBuy = false;

    public int tiempoParaSalir =4;

    public Transform puntoDeSalida;

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
        instance = this;
        figuras = GameObject.FindGameObjectsWithTag("objeto");
        InitDay();

    }


    void Update()
    {
        DiaLogic();
        if (Input.GetKey(KeyCode.T))
        {
            SeFue();
        }
    }
    void DiaLogic()
    {
        if (!hasActiveFigures && willBuy || activeFigure==null)
        {
            activeFigure = figuras[Random.Range(0, figuras.Length)];
            if (figuras.Length == 0)
            {
                print("Ganastes");
            }
            activeFigure.GetComponent<Figuras>().isCorrect = true;
            hasActiveFigures = true;
        }
        if (clientesTotalesDelDia >= clientesPorDia)
        {
            clientesTotalesDelDia = 0;
            //triggerea el efecto del siguiente dia
            DataManager.instance.diaActual++;
        }
    }
    public void VenderFigura()
    {
        hasActiveFigures = false;
        activeFigure = null;
        willBuy=false;
        DataManager.instance.IncrementarFigurasVendidas();

    }

    public void ChangeClient()
    {
        
    }
    public void InitDay()
    {
        personas=GameObject.FindGameObjectsWithTag("persona");
    }
    public void AcercarsealMostrador()
    {
        
    }
    public void SeFue()
    {
        float step = 5 * Time.deltaTime;
        personas[activePersonIndex].transform.position = Vector3.MoveTowards(personas[activePersonIndex].transform.position, puntoDeSalida.position, step);
    }
}
