using UnityEngine;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private SpriteRenderer[] spriteRenderers; // Reference to all game sprites

    private void Start()
    {
        // Load saved graphics settings (default to Medium if not set)
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", 1);
        SetGraphicsQuality(savedQuality);
        graphicsDropdown.value = savedQuality;

        // Add listener for dropdown changes
        graphicsDropdown.onValueChanged.AddListener(SetGraphicsQuality);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);  // Apply Unity Quality Setting
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex); // Save Preference
        PlayerPrefs.Save(); // Ensure it persists

        AdjustSpriteQuality(qualityIndex); // Apply sprite settings
    }

    private void AdjustSpriteQuality(int qualityIndex)
    {
        // Set sprite filtering and compression based on quality
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            if (sprite != null && sprite.sprite != null)
            {
                Texture2D texture = sprite.sprite.texture;
                switch (qualityIndex)
                {
                    case 0: // Low Quality
                        texture.filterMode = FilterMode.Point; // Pixelated for performance
                        texture.anisoLevel = 0; // No anisotropic filtering
                        break;

                    case 1: // Medium Quality
                        texture.filterMode = FilterMode.Bilinear; // Smooth interpolation
                        texture.anisoLevel = 2; // Minimal anisotropic filtering
                        break;

                    case 2: // High Quality
                        texture.filterMode = FilterMode.Trilinear; // High-quality interpolation
                        texture.anisoLevel = 8; // High anisotropic filtering
                        break;
                }
            }
        }
    }
}
