using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public int diaActual;
    public int diasTotales;
    public int figurasVendidas;
    // Esto lleva la lógica de cuantas figuras has vendido que dias. Para saber si vas a conseguir el final bueno o malo.
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (diaActual == diasTotales)
        {
            print("Dia final");
            if (figurasVendidas >= 8)
            {
                print("Final bueno");
            }
            else
            {
                print("Final malo");
            }
        }
    }
    public void IncrementarFigurasVendidas()
    {
        figurasVendidas++;
    }
}
