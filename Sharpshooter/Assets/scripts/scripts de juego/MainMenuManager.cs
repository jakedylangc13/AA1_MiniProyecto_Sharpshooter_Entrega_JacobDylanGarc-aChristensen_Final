using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego");  
        Application.Quit();            
    }
}
