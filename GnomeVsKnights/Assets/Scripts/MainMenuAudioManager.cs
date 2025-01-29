using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAudioManager : MonoBehaviour
{
    private static MainMenuAudioManager instance;
    private AudioSource audioSource;
    public AudioClip[] playlist; // Array of songs
    private int currentTrackIndex = 0;

    private void Awake()
    {
        // Ensure there is only one instance of MainMenuAudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Subscribe to scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Start playing music
        PlayNextTrack();
    }

    private void Update()
    {
        // Check if the current track has ended
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        if (playlist.Length == 0) return; // Prevent crash if playlist is empty

        audioSource.clip = playlist[currentTrackIndex];
        audioSource.Play();

        // Move to the next song or loop back to the start
        currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop and destroy music when leaving the Main Menu
        if (scene.name != "MainMenuScene") // Change this to match your Main Menu scene name
        {
            audioSource.Stop();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
