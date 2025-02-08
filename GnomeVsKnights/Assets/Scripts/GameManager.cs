﻿using System;
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
    public Tilemap map;
    public Camera cam;
    private bool touchBegan = false;
    private GameObject placementIndicator = null;
    public Dictionary<Vector3Int, GnomeBase> placedGnomes = new Dictionary<Vector3Int, GnomeBase>();
    public List<GameObject> knightQueue = new List<GameObject>();
    private int placementType = 0;
    public float knightSpawnInterval = 2f; // Time interval between knight spawns
    private int knightsToSpawn = 0; // Number of knights left to spawn in the current wave
    private bool isWaveActive = false; // Is the wave currently active?
    private float knightSpawnTimer = 0f;

    // UI Elements
    public TMP_Text energyText;
    public TMP_Text waveText;
    public GameObject pauseMenu;
    public GameObject winnerPanel;
    public GameObject gameOverPanel;
    public Button[] gnomeButtons;

    private int playerEnergy = 100;
    private int currentWave = 1;
    private int maxWaves = 5;
    private bool gameEnded = false;

    private void Start()
    {
        knightSpawnTimer = knightSpawnInterval;
        UpdateEnergyUI();
        UpdateWaveUI();
        pauseMenu.SetActive(false);
        winnerPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        for (int i = 0; i < gnomeButtons.Length; i++)
        {
            int index = i;
            gnomeButtons[i].onClick.AddListener(() => InitiatePlacement(index));
        }
    }

    private void Update()
    {
        if (gameEnded) return;

        int input = getInput(0);
        if (input == 1)
        {
            InitiatePlacement(placementType);
        }
        else if (input == 2)
        {
            UpdatePlacementIndicator();
        }
        else if (input == 3)
        {
            PlaceGnome();
        }

        knightSpawnTimer -= Time.deltaTime;
        if (knightSpawnTimer <= 0)
        {
            SpawnKnight();
            knightSpawnTimer = knightSpawnInterval;
        }

        // Check if the player wins
        if (currentWave > maxWaves && knightQueue.Count == 0)
        {
            ShowWinnerScreen();
        }
        if (isWaveActive)
        {
            // Spawn knights during active waves
            knightSpawnTimer -= Time.deltaTime;
            if (knightSpawnTimer <= 0 && knightsToSpawn > 0)
            {
                SpawnKnight();
                knightsToSpawn--;
                knightSpawnTimer = knightSpawnInterval; // Reset the timer
            }

            // End the wave when all knights are spawned and defeated
            if (knightsToSpawn <= 0 && knightQueue.Count == 0)
            {
                EndWave();
            }
        }
    }

    public void InitiatePlacement(int type)
    {
        if (playerEnergy < 25)
        {
            Debug.Log("Not enough energy to place a gnome!");
            return;
        }

        if (placementIndicator != null) Destroy(placementIndicator);
        placementIndicator = Instantiate(placementPrefabs[type]);
        placementType = type;
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
        if (fullGnomes.Length == 0)
        {
            Debug.LogError("The fullGnomes array is empty. Add gnome prefabs in the Inspector.");
            return;
        }

        if (placementIndicator != null)
        {
            Destroy(placementIndicator);
            placementIndicator = null;
        }

        Vector3Int at = GetCell(GetWorld(getInputLocation()));
        Debug.Log($"Clicked Position: {at}");

        BoundsInt tilemapBounds = map.cellBounds;

        if (tilemapBounds.Contains(at))
        {
            if (!placedGnomes.ContainsKey(at))
            {
                if (placementType < 0 || placementType >= fullGnomes.Length)
                {
                    Debug.LogError($"Invalid placementType: {placementType}. Array size: {fullGnomes.Length}");
                    return;
                }

                GameObject gnome = Instantiate(fullGnomes[placementType]);
                gnome.transform.position = GetWorld(at) + new Vector3(map.cellSize.x * 0.5f, map.cellSize.y * 0.5f, 0);
                GnomeBase gnomeData = gnome.GetComponent<GnomeBase>();
                gnomeData.Cell = at;
                placedGnomes.Add(at, gnomeData);

                Debug.Log($"Placed gnome of type {placementType} at {at}");
            }
            else
            {
                Debug.Log("Cannot place gnome: Spot is already occupied.");
            }
        }
        else
        {
            Debug.Log($"Cannot place gnome: Position {at} is outside the tilemap bounds {tilemapBounds}.");
        }
    }

    private void SpawnKnight()
    {
        GameObject knightPrefab = knightQueue[UnityEngine.Random.Range(0, knightQueue.Count)];
        GameObject knight = Instantiate(knightPrefab);

        // Randomize spawn row
        int randomRow = UnityEngine.Random.Range(0, 5); 
        Vector3Int spawnCell = new Vector3Int(9, randomRow, 0);
        knight.transform.position = GetWorld(spawnCell) + map.cellSize * 0.5f;
    }

    public void KnightReachedEnd()
    {
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        gameEnded = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pause game
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

        // Increase knight count for the wave
        int knightCount = 5 + (currentWave * 2);
        for (int i = 0; i < knightCount; i++)
        {
            GameObject knightPrefab = knightQueue[UnityEngine.Random.Range(0, knightQueue.Count)];
            knightQueue.Add(knightPrefab); // Add knights to queue
        }

        winnerPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void StartWave()
    {
        currentWave++;
        UpdateWaveUI();
        StartCoroutine(DelayedWaveStart());

        knightsToSpawn = 5 + (currentWave * 2); 
        isWaveActive = true;

        
        winnerPanel.SetActive(false);

        // Resume game
        Time.timeScale = 1;
    }
    private IEnumerator DelayedWaveStart()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds before starting the wave

        
        currentWave++;
        UpdateWaveUI();

        knightsToSpawn = 5 + (currentWave * 2);
        isWaveActive = true;

        // Hide the Winner Panel
        winnerPanel.SetActive(false);

        // Resume game
        Time.timeScale = 1;
    }
    private void EndWave()
    {
        isWaveActive = false;

        winnerPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0;
    }

    private int getInput(int button)
    {
        int result = 0;
        if (Input.GetMouseButtonDown(button)) result = 1;
        else if (Input.GetMouseButton(button)) result = 2;
        else if (Input.GetMouseButtonUp(button)) result = 3;

        if (result == 0 && Input.touchSupported)
        {
            TouchPhase touchPhase = Input.GetTouch(button).phase;
            switch (touchPhase)
            {
                case TouchPhase.Began:
                    result = 1;
                    touchBegan = true;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    result = touchBegan ? 3 : 0;
                    touchBegan = false;
                    break;
                default:
                    result = touchBegan ? 2 : 0;
                    break;
            }
        }
        return result;
    }

    private Vector2 getInputLocation()
    {
        return touchBegan ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
    }

    public Vector3Int GetCell(Vector3 world)
    {
        return map.WorldToCell(world);
    }

    public Vector3 GetWorld(Vector3Int cell)
    {
        return map.CellToWorld(cell);
    }

    public Vector3 GetWorld(Vector3 camera)
    {
        return cam.ScreenToWorldPoint(camera);
    }

    public void KillGnome(Vector3Int at)
    {
        if (placedGnomes.ContainsKey(at))
        {
            Destroy(placedGnomes[at].gameObject);
            placedGnomes.Remove(at);
        }
    }

    public void UpdateEnergyUI()
    {
        energyText.text = playerEnergy.ToString();
    }

    public void UpdateWaveUI()
    {
        waveText.text = $"Wave {currentWave}/{maxWaves}";
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }
    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure time is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
}

