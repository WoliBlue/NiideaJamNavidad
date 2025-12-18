using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
public void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }
public void Exit()
    {
        Application.Quit();
    }
    // Abrir el panel de crédtos
    public void OpenCredits(GameObject creditsPanel)
    {
        creditsPanel.SetActive(true);
    }
    // Cerrar el panel de créditos
    public void CloseCredits(GameObject creditsPanel)
    {
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
