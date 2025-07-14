using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        
        Application.Quit();            
    }
}
