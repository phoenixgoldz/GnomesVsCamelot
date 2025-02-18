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

        RemovePrePlacedGnomes(); // Remove all pre-placed gnomes
    }

    private void RemovePrePlacedGnomes()
    {
        foreach (GnomeBase gnome in FindObjectsOfType<GnomeBase>())
        {
            Destroy(gnome.gameObject); // Remove all pre-spawned gnomes
        }
    }
}
