using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LocalGameManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject[] placementPrefabs;
    [SerializeField] private GameObject[] fullGnomes;
    [SerializeField] private RectTransform[] gnomeUILocations;
    [SerializeField] private List<GameObject> knightQueue;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float knightSpawnInterval = 1.5f;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.cam = cam;
            GameManager.Instance.map = tilemap;
            GameManager.Instance.placementPrefabs = placementPrefabs;
            GameManager.Instance.fullGnomes = fullGnomes;
            GameManager.Instance.gnomeUILocations = gnomeUILocations;
            GameManager.Instance.knightQueue = knightQueue;
            GameManager.Instance.energyText = energyText;
            GameManager.Instance.waveText = waveText;
            GameManager.Instance.pauseMenu = pauseMenu;
            GameManager.Instance.winnerPanel = winnerPanel;
            GameManager.Instance.gameOverPanel = gameOverPanel;
            GameManager.Instance.knightSpawnInterval = knightSpawnInterval;
        }
        else
        {
            Debug.LogError("GameManager Instance is NULL in LocalGameManager!");
        }

        Invoke(nameof(RemovePrePlacedGnomes), 0.1f); // Small delay to ensure objects are initialized
    }

    private void RemovePrePlacedGnomes()
    {
        GnomeBase[] prePlacedGnomes = Object.FindObjectsByType<GnomeBase>(FindObjectsSortMode.None);

        if (prePlacedGnomes.Length > 0)
        {
            foreach (GnomeBase gnome in prePlacedGnomes)
            {
                Destroy(gnome.gameObject);
            }
            Debug.Log($"Removed {prePlacedGnomes.Length} pre-placed gnomes.");
        }
    }
}
