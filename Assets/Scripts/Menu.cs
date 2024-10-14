using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Gameplay scene
        SceneManager.LoadScene("Gameplay"); 
    }

    public void ExitGame()
    {
        //Quit game
        Application.Quit();
    }
}
