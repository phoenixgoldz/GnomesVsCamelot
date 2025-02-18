using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UIElements;

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
    private int knightsToSpawn = 0; // Number of knights left to spawn in the current wave
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

    private float dt = 0;


    private void Start()
    {
        knightSpawnTimer = knightSpawnInterval;
        UpdateWaveUI();
        pauseMenu.SetActive(false);
        winnerPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameEnded) return;

        dt += Time.deltaTime;
        if (dt >= 1f)
        {
            var secondsPassed = Mathf.FloorToInt(dt);
            playerEnergy += secondsPassed;
            dt -= secondsPassed;
        }
        UpdateEnergyUI();

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

    public void ToggleFastForward()
    {
        isFastForward = !isFastForward;
        Time.timeScale = isFastForward ? 2f : 1f; // 2x Speed when active
        Debug.Log("Fast Forward: " + (isFastForward ? "ON" : "OFF"));
    }
    public void KnightReachedEnd()
    {
        Debug.Log("A knight reached the base! Game Over.");
        ShowGameOverScreen();
    }

    public void InitiatePlacement()
    {
        if (placementType != -1)
        {
            if (playerEnergy < 25)
            {
                Debug.Log("Not enough energy to place a gnome!");
                return;
            }

            placementIndicator = Instantiate(placementPrefabs[placementType]);
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
        if (placementType != -1)
        {
            if (fullGnomes.Length == 0)
            {
                Debug.LogError("The fullGnomes array is empty. Add gnome prefabs in the Inspector.");
                return;
            }

            Vector3Int at = GetCell(GetWorld(getInputLocation()));

            //Use hardcoded values instead of cell bounds because if someone accidentally places a cell far away it would be a pain to identify the cause and location
            if (at.x < 0 || at.x > 8 || at.y < 0 || at.y > 4)
            {
                Debug.Log("Cannot place gnome: Out of bounds");
            }
            else if (!placedGnomes.ContainsKey(at))
            {
                if (placementType < 0 || placementType >= fullGnomes.Length)
                {
                    Debug.LogError($"Invalid placementType: {placementType}. Array size: {fullGnomes.Length}");
                    return;
                }

                if (playerEnergy > 0)
                {
                    GameObject gnome = Instantiate(fullGnomes[placementType]);
                    gnome.transform.position = GetWorld(at) + new Vector3(map.cellSize.x * 0.5f, map.cellSize.y * 0.5f, 0);
                    GnomeBase gnomeData = gnome.GetComponent<GnomeBase>();
                    gnomeData.Cell = at;
                    placedGnomes.Add(at, gnomeData);

                    Debug.Log($"Placed gnome of type {placementType} at {at}");

                    playerEnergy -= 25;
                }
            }
            else
            {
                Debug.Log("Cannot place gnome: Spot is already occupied.");
            }
            Destroy(placementIndicator);
            placementIndicator = null;
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

        knightsToSpawn = 5 + (currentWave + 2);
        isWaveActive = true;


        winnerPanel.SetActive(false);

        // Resume game
        Time.timeScale = 1;
    }
    private IEnumerator DelayedWaveStart()
    {
        yield return new WaitForSeconds(3f);


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
        bool isPaused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0; // Freeze the game
            AudioListener.pause = true; // Pause audio
        }
        else
        {
            Time.timeScale = 1; // Resume game
            AudioListener.pause = false;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure time is running

        // Destroy all placed gnomes
        foreach (var gnome in placedGnomes.Values)
        {
            Destroy(gnome.gameObject);
        }
        placedGnomes.Clear();

        // Reset energy
        playerEnergy = 100;
        UpdateEnergyUI();

        // Reset knights
        foreach (GameObject knight in knightQueue)
        {
            Destroy(knight);
        }
        knightQueue.Clear();

        // Reset wave counter
        currentWave = 1;
        UpdateWaveUI();

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
}
