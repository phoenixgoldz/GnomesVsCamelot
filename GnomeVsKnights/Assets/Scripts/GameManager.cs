using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] placementPrefabs;
    public GameObject[] fullGnomes;
    public RectTransform[] gnomeUILocations;
    public Tilemap map;
    public Camera cam;
    private bool touchBegan = false;
    private GameObject placementIndicator = null;
    public Dictionary<Vector3Int, GnomeBase> placedGnomes = new Dictionary<Vector3Int, GnomeBase>();
    public List<GameObject> knightQueue;
    private int placementType = 0;
    public float knightSpawnInterval = 1.5f;
    private int knightsToSpawn = 0;
    private bool isWaveActive = false;
    private float knightSpawnTimer = 0f;
    public TMP_Text energyText;
    public TMP_Text waveText;
    public GameObject pauseMenu;
    public GameObject winnerPanel;
    public GameObject gameOverPanel;
    private int playerEnergy = 100;
    private int currentWave = 1;
    private int maxWaves = 5;
    private bool gameEnded = false;
    public bool isFastForward = false;
    private bool isPaused = false;

    private void Start()
    {
        knightSpawnTimer = knightSpawnInterval;
        UpdateEnergyUI();
        UpdateWaveUI();
        pauseMenu.SetActive(false);
        winnerPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    private void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = playerEnergy.ToString();
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}/{maxWaves}";
        }
    }

    private void Update()
    {
        if (gameEnded) return;

        int input = getInput(0);
        if (input == 1)
        {
            placementType = -1;
            for (int i = 0; i < gnomeUILocations.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(gnomeUILocations[i], getInputLocation()))
                {
                    placementType = i;
                    break;
                }
            }
            InitiatePlacement();
        }
        else if (input == 2)
        {
            UpdatePlacementIndicator();
        }
        else if (input == 3)
        {
            PlaceGnome();
        }

        if (!isPaused)
        {
            knightSpawnTimer -= Time.deltaTime;
            if (knightSpawnTimer <= 0)
            {
                SpawnKnight();
                knightSpawnTimer = knightSpawnInterval;
            }

            if (isWaveActive)
            {
                knightSpawnTimer -= Time.deltaTime;
                if (knightSpawnTimer <= 0 && knightsToSpawn > 0)
                {
                    SpawnKnight();
                    knightsToSpawn--;
                    knightSpawnTimer = knightSpawnInterval;
                }

                if (knightsToSpawn <= 0 && knightQueue.Count == 0)
                {
                    EndWave();
                }
            }
        }
    }

    public void ToggleFastForward()
    {
        isFastForward = !isFastForward;
        Time.timeScale = isFastForward ? 2f : 1f;
        Debug.Log("Fast Forward: " + (isFastForward ? "ON" : "OFF"));
    }

    public void KnightReachedEnd()
    {
        Debug.Log("A knight reached the base! Game Over.");
        ShowGameOverScreen();
    }

    public void InitiatePlacement()
    {
        if (placementType != -1 && playerEnergy >= 25)
        {
            placementIndicator = Instantiate(placementPrefabs[placementType]);
        }
        else
        {
            Debug.Log("Not enough energy to place a gnome!");
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            Vector3Int cellPos = GetCell(GetWorld(getInputLocation()));
            placementIndicator.transform.position = GetWorld(cellPos) + map.cellSize * 0.5f;
        }
    }

    private void PlaceGnome()
    {
        if (placementType == -1) return;

        Vector3Int at = GetCell(GetWorld(getInputLocation()));

        if (at.x < 0 || at.x > 8 || at.y < 0 || at.y > 4)
        {
            Debug.Log("Cannot place gnome: Out of bounds");
        }
        else if (!placedGnomes.ContainsKey(at))
        {
            GameObject gnome = Instantiate(fullGnomes[placementType]);
            gnome.transform.position = GetWorld(at) + map.cellSize * 0.5f;
            GnomeBase gnomeData = gnome.GetComponent<GnomeBase>();
            gnomeData.Cell = at;
            placedGnomes.Add(at, gnomeData);
            Debug.Log($"Placed gnome of type {placementType} at {at}");
        }
        else
        {
            Debug.Log("Cannot place gnome: Spot is already occupied.");
        }

        Destroy(placementIndicator);
        placementIndicator = null;
    }

    private void SpawnKnight()
    {
        GameObject knightPrefab = knightQueue[UnityEngine.Random.Range(0, knightQueue.Count)];
        GameObject knight = Instantiate(knightPrefab);
        int randomRow = UnityEngine.Random.Range(0, 5);
        Vector3Int spawnCell = new Vector3Int(9, randomRow, 0);
        knight.transform.position = GetWorld(spawnCell) + map.cellSize * 0.5f;
    }

    private void ShowGameOverScreen()
    {
        gameEnded = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void ShowWinnerScreen()
    {
        gameEnded = true;
        winnerPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void NextWave()
    {
        currentWave++;
        UpdateWaveUI();
        knightsToSpawn = 5 + (currentWave * 2);
        isWaveActive = true;
        winnerPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void EndWave()
    {
        isWaveActive = false;
        winnerPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            PauseAllAnimations();
        }
        else
        {
            Time.timeScale = isFastForward ? 2 : 1;
            ResumeAllAnimations();
        }

        pauseMenu.SetActive(isPaused);
    }

    private void PauseAllAnimations()
    {
        Animator[] animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (Animator anim in animators)
        {
            anim.speed = 0; // Stop all animations
        }
    }

    private void ResumeAllAnimations()
    {
        Animator[] animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (Animator anim in animators)
        {
            anim.speed = 1; // Resume all animations
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    private int getInput(int button)
    {
        if (Input.GetMouseButtonDown(button)) return 1;
        if (Input.GetMouseButton(button)) return 2;
        if (Input.GetMouseButtonUp(button)) return 3;

        if (Input.touchSupported)
        {
            TouchPhase touchPhase = Input.GetTouch(button).phase;
            switch (touchPhase)
            {
                case TouchPhase.Began: return 1;
                case TouchPhase.Ended: case TouchPhase.Canceled: return 3;
                default: return 2;
            }
        }

        return 0;
    }

    private Vector2 getInputLocation()
    {
        return touchBegan ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
    }

    public Vector3Int GetCell(Vector3 world) => map.WorldToCell(world);
    public Vector3 GetWorld(Vector3Int cell) => map.CellToWorld(cell);
    public Vector3 GetWorld(Vector3 camera) => cam.ScreenToWorldPoint(camera);
}
