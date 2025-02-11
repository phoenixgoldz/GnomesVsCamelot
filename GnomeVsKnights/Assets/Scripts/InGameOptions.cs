using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InGameOptions : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button closeButton;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Saving UI")]
    [SerializeField] private TMP_Text saveMessage;
    [SerializeField] private Image savingImage;
    [SerializeField] private RectTransform savingImageTransform;

<<<<<<< Updated upstream
    private bool isSaving = false;

=======
>>>>>>> Stashed changes
    private void Start()
    {
        LoadSettings();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        applyButton.onClick.AddListener(ApplySettings);
        closeButton.onClick.AddListener(CloseInGameOptions);

        // Ensure saving UI is hidden initially
        saveMessage?.gameObject.SetActive(false);
        savingImage?.gameObject.SetActive(false);
    }

    private void LoadSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

<<<<<<< Updated upstream
        // Apply settings when loading
=======
        // Apply settings on load
>>>>>>> Stashed changes
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicAudioSource != null)
            musicAudioSource.volume = volume; // Directly set volume on AudioSource
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxAudioSource != null)
            sfxAudioSource.volume = volume; // Directly set volume on AudioSource
    }

    public void ApplySettings()
    {
<<<<<<< Updated upstream
        if (isSaving) return; // Prevent multiple saves overlapping

        isSaving = true;
        
=======
>>>>>>> Stashed changes
        // Show saving UI immediately
        if (savingImage != null) savingImage.gameObject.SetActive(true);
        if (saveMessage != null)
        {
            saveMessage.gameObject.SetActive(true);
            saveMessage.text = "Saving...";
        }

<<<<<<< Updated upstream
        // Start rotating animation
        StartCoroutine(RotateSavingIcon());

=======
>>>>>>> Stashed changes
        // Save settings permanently
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();

        Debug.Log("Settings Applied!");

        // Start coroutine to hide saving message after a delay
        StartCoroutine(HideSaveMessage());
    }

    private IEnumerator HideSaveMessage()
    {
<<<<<<< Updated upstream
        yield return new WaitForSeconds(2.5f); // Wait for saving to be "complete"

        // Hide UI
        saveMessage?.gameObject.SetActive(false);
        savingImage?.gameObject.SetActive(false);
        
        isSaving = false; // Allow future saves
    }

    private IEnumerator RotateSavingIcon()
    {
        while (isSaving)
        {
            if (savingImageTransform != null)
                savingImageTransform.Rotate(0f, 0f, 100f * Time.deltaTime); // Rotate on Z-axis

            yield return null;
        }
=======
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f)
        {
            if (savingImageTransform != null)
                savingImageTransform.Rotate(0f, 0f, 100f * Time.deltaTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        saveMessage?.gameObject.SetActive(false);
        savingImage?.gameObject.SetActive(false);
>>>>>>> Stashed changes
    }

    public void CloseInGameOptions()
    {
        ApplySettings(); // Auto-save settings when closing
        gameObject.SetActive(false);
    }
}
