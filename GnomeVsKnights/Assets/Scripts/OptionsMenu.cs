using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider volumeSlider;
    public TMP_Text volumeText;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Button applyButton;
    public TMP_Text saveMessage; // Text to display when settings are applied
    public Image savingImage; // The rotating saving image

    private float tempVolume;
    private bool tempFullscreen;
    private Resolution[] resolutions;
    private List<string> resolutionOptions = new List<string>();
    private int currentResolutionIndex;
    private bool isSaving = false;

    private void Start()
    {
        optionsPanel.SetActive(false);
        saveMessage.gameObject.SetActive(false); // Hide message initially
        savingImage.enabled = false; // Hide saving image initially

        // Load saved settings or set default values
        float savedVolume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 0.5f;
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);

        // Apply volume settings
        volumeSlider.value = savedVolume;
        fullscreenToggle.isOn = savedFullscreen;
        UpdateVolumeText(savedVolume);

        // Store temporary settings
        tempVolume = savedVolume;
        tempFullscreen = savedFullscreen;

        // Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionString = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(resolutionString);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = PlayerPrefs.HasKey("ResolutionIndex") ? savedResolutionIndex : currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add Listeners
        volumeSlider.onValueChanged.AddListener(UpdateVolumeText);
        fullscreenToggle.onValueChanged.AddListener(value => tempFullscreen = value);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        applyButton.onClick.AddListener(ApplySettings);
    }

    private void Update()
    {
        // Rotate the saving image while saving
        if (isSaving && savingImage.enabled)
        {
            savingImage.transform.Rotate(0, 0, 200 * Time.deltaTime);
        }
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
        // Save settings
        PlayerPrefs.SetFloat("Volume", tempVolume);
        PlayerPrefs.SetInt("Fullscreen", tempFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.Save();

        // Apply settings
        AudioListener.volume = tempVolume;
        Screen.fullScreen = tempFullscreen;
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);

        Debug.Log("Settings Applied: Volume = " + (tempVolume * 100) + "%, Fullscreen = " + tempFullscreen +
                  ", Resolution = " + resolutionOptions[resolutionDropdown.value]);

        // Show and rotate the saving image
        StartCoroutine(ShowSavingImage());

        // Show save confirmation message
        StartCoroutine(ShowSaveMessage());
    }

    private IEnumerator ShowSavingImage()
    {
        isSaving = true;
        savingImage.enabled = true;
        yield return new WaitForSeconds(2.5f);
        isSaving = false;
        savingImage.enabled = false;
    }

    private IEnumerator ShowSaveMessage()
    {
        saveMessage.gameObject.SetActive(true);
        saveMessage.text = "Saving...";
        yield return new WaitForSeconds(2.5f);
        saveMessage.gameObject.SetActive(false);
    }

    private void UpdateVolumeText(float volume)
    {
        volumeText.text = Mathf.RoundToInt(volume * 100) + "%";
        tempVolume = volume;
    }

    private void SetResolution(int index)
    {
        currentResolutionIndex = index;
    }
}
