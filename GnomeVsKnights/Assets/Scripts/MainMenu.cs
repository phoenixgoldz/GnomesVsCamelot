using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene"); // Change "GameScene" to your actual game scene name
    }

    public void LoadGame()
    {
        // Implement Load Game functionality (e.g., Load saved data)
        Debug.Log("Load Game Clicked - Implement save/load system");
    }

    public void OpenOptions()
    {
        // Implement Options Menu logic (e.g., opening another UI panel)
        Debug.Log("Options Menu Opened");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game Clicked");
        Application.Quit(); // Exits the game (only works in a built application)
    }
}
