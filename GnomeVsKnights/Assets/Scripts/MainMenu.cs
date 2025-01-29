using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu; // Assign in Unity Inspector

    public void NewGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        // Implement Load Game functionality
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedScene = PlayerPrefs.GetString("SavedScene");
            SceneManager.LoadScene(savedScene); // Load last saved scene
            Debug.Log("Loading saved game: " + savedScene);
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    public void OpenOptions()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
            Debug.Log("Options Menu Opened");
        }
        else
        {
            Debug.LogError("OptionsMenu GameObject is not assigned in the Inspector!");
        }
    }

    public void CloseOptions()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game Clicked");
        Application.Quit(); 
    }
}
