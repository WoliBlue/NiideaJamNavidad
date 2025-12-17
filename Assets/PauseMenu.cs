using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuCanvas;
    public bool cursorState;
    void ToggleMenu()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.visible=true;
        }
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.visible=false;
        }
        menuCanvas.SetActive(!menuCanvas.activeInHierarchy);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void CloseMenu()
    {
        ToggleMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
}
