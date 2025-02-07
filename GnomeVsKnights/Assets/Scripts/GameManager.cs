using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] placementPrefabs;
    public GameObject[] fullGnomes;
    public Tilemap map;
    public Camera cam;
    private bool touchBegan = false;
    private GameObject placementIndicator = null;
    public Dictionary<Vector3Int, GnomeBase> placedGnomes = new Dictionary<Vector3Int, GnomeBase>();
    public List<GameObject> knightQueue = new List<GameObject>(); // Queue for knights
    private int placementType = 0;
    public float knightSpawnInterval = 5f; // Time interval for knights to spawn
    private float knightSpawnTimer = 0f;

    private void Start()
    {
        knightSpawnTimer = knightSpawnInterval;
    }

    private void Update()
    {
        int input = getInput(0);
        if (input == 1)
        {
            InitiatePlacement(0);
        }
        else if (input == 2)
        {
            UpdatePlacementIndicator();
        }
        else if (input == 3)
        {
            PlaceGnome();
        }

        // Handle knight spawning logic
        knightSpawnTimer -= Time.deltaTime;
        if (knightSpawnTimer <= 0)
        {
            SpawnKnight();
            knightSpawnTimer = knightSpawnInterval; // Reset timer
        }
    }

    public void InitiatePlacement(int type)
    {
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
        if (placementIndicator != null)
        {
            Destroy(placementIndicator);
            placementIndicator = null;
        }

        Vector3Int at = GetCell(GetWorld(getInputLocation()));
        if (at.x >= 0 && at.x <= 8 && at.y >= 0 && at.y <= 4)
        {
            if (!placedGnomes.ContainsKey(at))
            {
                GameObject gnome = Instantiate(fullGnomes[placementType]);
                gnome.transform.position = GetWorld(at) + map.cellSize * 0.5f;
                GnomeBase gnomeData = gnome.GetComponent<GnomeBase>();
                gnomeData.Cell = at; // Use the newly added property
                placedGnomes.Add(at, gnomeData);
            }
            else
            {
                Debug.Log("Not placed because occupied");
            }
        }
        else
        {
            Debug.Log($"Not placed because out of bounds ({at.x}, {at.y})");
        }
    }

    private void SpawnKnight()
    {
        if (knightQueue.Count > 0)
        {
            GameObject knightPrefab = knightQueue[0];
            knightQueue.RemoveAt(0);
            GameObject knight = Instantiate(knightPrefab);
            knight.transform.position = GetWorld(new Vector3Int(9, 0, UnityEngine.Random.Range(0, 4))) + map.cellSize * 0.5f;
        }
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
}
