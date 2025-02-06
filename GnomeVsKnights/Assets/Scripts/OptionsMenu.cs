using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
   
    public Toggle subtitlesToggle;
    public Button applyButton;
    public Button restoreDefaultsButton;
    public Button closeButton;
    public TMP_Text saveMessage;
    public AudioSource buttonClickAudioSource;
    public AudioClip buttonClickSound;
    public Image savingImage;
    public RectTransform savingImageTransform;

    private float tempMusicVolume;
    private float tempSoundVolume;
    private bool tempSubtitles;

    private void Start()
    {
        optionsPanel.SetActive(false);
        saveMessage.gameObject.SetActive(false);
        savingImage.gameObject.SetActive(false);
        
        restoreDefaultsButton.onClick.AddListener(() => { PlayButtonSound(); });
        closeButton.onClick.AddListener(() => { PlayButtonSound(); CloseOptions(); });
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        PlayButtonSound();
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", tempMusicVolume);
        PlayerPrefs.SetFloat("SoundVolume", tempSoundVolume);
        PlayerPrefs.SetInt("Subtitles", tempSubtitles ? 1 : 0);
        PlayerPrefs.Save();

        Debug.LogFormat("Settings Applied: Music Volume = " + (tempMusicVolume * 100) + "%", "SoundVolume = " + (tempSoundVolume * 100) + "%", "Subtitles = " + tempSubtitles);
        StartCoroutine(ShowSaveMessage());
    }

    private IEnumerator ShowSaveMessage()
    {
        savingImage.gameObject.SetActive(true);
        saveMessage.gameObject.SetActive(true);
        saveMessage.text = "Saving...";
        
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f)
        {
            savingImageTransform.Rotate(0f, 0f, 100f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        saveMessage.gameObject.SetActive(false);
        savingImage.gameObject.SetActive(false);
    }

    private void PlayButtonSound()
    {
        if (buttonClickAudioSource != null && buttonClickSound != null)
        {
            buttonClickAudioSource.PlayOneShot(buttonClickSound);
        }
    }
    
}
