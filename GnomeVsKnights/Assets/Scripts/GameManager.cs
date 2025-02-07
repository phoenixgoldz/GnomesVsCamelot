using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] placementPrefabs;
    private GameObject[] fullGnomes;
    public Tilemap map;
    public Camera cam;
    private bool touchBegan = false;
    private GameObject placementIndicator = null;
    public Dictionary<Vector3Int, GnomeBase> placedGnomes;
    //public Dictionary<int, GameObject> knightQueue
    public int knightQueueLocation = 0;
    private int placementType = 0;
    public void InitiatePlacement(int type)
    {
        placementIndicator = Instantiate(placementPrefabs[type]);
    }

    private void Update()
    {
        int input = getInput(0);
        if (input == 1)
        {
            InitiatePlacement(0);
        }
        else if(input == 2)
        {
            updatePlacementIndicator();
        }
        else if(input == 3)
        {
            GameObject.Destroy(placementIndicator);
            placementIndicator = null;
            Vector3Int at = GetCell(GetWorld(getInputLocation()));
            if (at.x >= 0 && at.x <= 8 && at.y >= 0 && at.y <= 4)
            {
                if (!placedGnomes.ContainsKey(at))
                {
                    GameObject gnome = Instantiate(fullGnomes[placementType]);
                    gnome.transform.position = GetWorld(at) + map.cellSize * 0.5f;
                    GnomeBase gnomeData = gnome.GetComponent<GnomeBase>();
                    gnomeData.cell = at;
                    placedGnomes.Add(at, gnomeData);
                }
                else
                {
                    Debug.Log("Not placed because occupied");
                }
            }
            else
            {
                Debug.Log($"Not placed because at ({at.x}, {at.y})");
            }

        }
    }

    private void updatePlacementIndicator()
    {
        placementIndicator.transform.position = GetWorld(GetCell(GetWorld(getInputLocation())));
        placementIndicator.transform.position += map.cellSize * 0.5f;
    }

    private int getInput(int button)
    {
        int result = 0;
        if(Input.GetMouseButtonDown(button))
        {
            result = 1;
        }
        else if(Input.GetMouseButton(button))
        {
            result = 2;
        }
        else if(Input.GetMouseButtonUp(button))
        {
            result = 3;
        }
        if(result == 0 && Input.touchSupported)
        {
            TouchPhase touchPhase = Input.GetTouch(button).phase;
            switch(touchPhase)
            {
                case TouchPhase.Began:
                    {
                        result = 1;
                        touchBegan = true;
                        break;
                    }
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    {
                        result = touchBegan ? 3 : 0;
                        touchBegan = false;
                        break;
                    }
                default:
                    {
                        result = touchBegan ? 2 : 0;
                        break;
                    }
            }
        }
        return result;
    }
    private Vector2 getInputLocation()
    {
        Vector2 location = new Vector2();
        if(touchBegan)
        {
            location = Input.GetTouch(0).position;
        }
        else
        {
            location = Input.mousePosition;
        }
        return location;
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

    private void FixedUpdate()
    {
        //if(knightQueue.ContainsKey(knightQueueLocation)
        //{
            //foreach(GameObject knight in knightQueue[knightQueueLocation])
            //{
                //spawnKnight(knight);
            //}
        //}

    }

    public void KillGnome(Vector3Int at)
    {
        placedGnomes.Remove(at);
    }

    private void spawnKnight(GameObject knight)
    {
        Instantiate(knight);
        knight.transform.position = GetWorld(new Vector3Int(9, 0, UnityEngine.Random.Range(0, 4))) + map.cellSize * 0.5f;
    }
}
