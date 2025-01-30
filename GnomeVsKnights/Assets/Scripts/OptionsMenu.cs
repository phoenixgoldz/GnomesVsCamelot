using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider volumeSlider;
    public TMP_Text volumeText;
    public Toggle fullscreenToggle;
    public Button applyButton;
    public TMP_Text saveMessage;

    private float tempVolume;
    private bool tempFullscreen;

    private void Start()
    {
        optionsPanel.SetActive(false);
        saveMessage.gameObject.SetActive(false);

        // Load saved settings
        tempVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        tempFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        volumeSlider.value = tempVolume;
        fullscreenToggle.isOn = tempFullscreen;
        UpdateVolumeText(tempVolume);

        // Add Listeners
        volumeSlider.onValueChanged.AddListener(UpdateVolumeText);
        fullscreenToggle.onValueChanged.AddListener(value => tempFullscreen = value);
        applyButton.onClick.AddListener(ApplySettings);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("Volume", tempVolume);
        PlayerPrefs.SetInt("Fullscreen", tempFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        AudioListener.volume = tempVolume;
        Screen.fullScreen = tempFullscreen;

        Debug.Log("Settings Applied: Volume = " + (tempVolume * 100) + "% Fullscreen = " + tempFullscreen);
        StartCoroutine(ShowSaveMessage());
    }

    private void UpdateVolumeText(float volume)
    {
        volumeText.text = Mathf.RoundToInt(volume * 100) + "%";
        tempVolume = volume;
    }

    private IEnumerator ShowSaveMessage()
    {
        saveMessage.gameObject.SetActive(true);
        saveMessage.text = "Settings Saved!";
        yield return new WaitForSeconds(1.5f);
        saveMessage.gameObject.SetActive(false);
    }
}