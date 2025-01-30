using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_Text musicVolumeText;
    public TMP_Text sfxVolumeText;
    public Toggle subtitlesToggle;
    public Button applyButton;
    public Button restoreDefaultsButton;
    public Button closeButton;
    public TMP_Text saveMessage;
    public AudioSource buttonClickAudioSource;
    public AudioClip buttonClickSound;
    public Image savingImage;
    public AudioMixer audioMixer;
    public RectTransform savingImageTransform;
    public Dropdown musicSelectionDropdown;
    public AudioSource musicSource;
    public AudioClip[] musicTracks;

    private float tempMusicVolume;
    private float tempSfxVolume;
    private bool tempSubtitles;

    private void Start()
    {
        optionsPanel.SetActive(false);
        saveMessage.gameObject.SetActive(false);
        savingImage.gameObject.SetActive(false);

        // Load saved settings
        tempMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        tempSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        tempSubtitles = PlayerPrefs.GetInt("Subtitles", 1) == 1;

        // Apply settings to UI
        musicVolumeSlider.value = tempMusicVolume;
        sfxVolumeSlider.value = tempSfxVolume;
        subtitlesToggle.isOn = tempSubtitles;

        // Populate music selection dropdown
        musicSelectionDropdown.ClearOptions();
        foreach (AudioClip track in musicTracks)
        {
            musicSelectionDropdown.options.Add(new Dropdown.OptionData(track.name));
        }
        musicSelectionDropdown.onValueChanged.AddListener(ChangeMusicTrack);
        
        // Add Listeners
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSfxVolume);
        subtitlesToggle.onValueChanged.AddListener(value => tempSubtitles = value);
        applyButton.onClick.AddListener(() => { PlayButtonSound(); ApplySettings(); });
        restoreDefaultsButton.onClick.AddListener(() => { PlayButtonSound(); RestoreDefaults(); });
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
        PlayerPrefs.SetFloat("SfxVolume", tempSfxVolume);
        PlayerPrefs.SetInt("Subtitles", tempSubtitles ? 1 : 0);
        PlayerPrefs.Save();

        //Debug.Log("Settings Applied: Music Volume = " + (tempMusicVolume * 100) + "%", "SFX Volume = " + (tempSfxVolume * 100) + "%", "Subtitles = " + tempSubtitles);
        StartCoroutine(ShowSaveMessage());
    }

    public void RestoreDefaults()
    {
        musicVolumeSlider.value = 0.5f;
        sfxVolumeSlider.value = 0.5f;
        subtitlesToggle.isOn = true;
        musicSelectionDropdown.value = 0;
    }

    private void UpdateMusicVolume(float volume)
    {
        tempMusicVolume = volume;
        musicVolumeText.text = Mathf.RoundToInt(volume * 100) + "%";
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    private void UpdateSfxVolume(float volume)
    {
        tempSfxVolume = volume;
        sfxVolumeText.text = Mathf.RoundToInt(volume * 100) + "%";
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
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

    private void ChangeMusicTrack(int index)
    {
        if (musicTracks.Length > index && musicTracks[index] != null)
        {
            musicSource.clip = musicTracks[index];
            musicSource.Play();
        }
    }
}
