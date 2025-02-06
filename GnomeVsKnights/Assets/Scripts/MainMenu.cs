using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");  // Ensure "GameScene" is the correct scene name
    }

    public void LoadGame()
    {
        // Placeholder for implementing a load game system
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedScene = PlayerPrefs.GetString("SavedScene");
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    public void OpenOptions()
    {
        Debug.Log("Options Menu Opened");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
        
    }
}
