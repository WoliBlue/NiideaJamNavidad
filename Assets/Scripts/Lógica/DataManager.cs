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
            if (figurasVendidas >= 8)
            {
                print("Final bueno");
            }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.willBuy=true;
        }
        
        if (diaActual == diasTotales && figurasVendidas<=5)
        {
                    print("Perdistes");
                     // cambiar a la escena.
                     GameManager.instance.endgamePanel.gameObject.SetActive(true);
                    GameManager.instance.endgamePanel.text="LOL NOOB";
            print("Dia final");

        }
         if (diaActual == diasTotales && figurasVendidas>=5)
        {
            GameManager.instance.endgamePanel.gameObject.SetActive(true);
        }
    }
    public void IncrementarFigurasVendidas()
    {
        figurasVendidas++;
    }
}
