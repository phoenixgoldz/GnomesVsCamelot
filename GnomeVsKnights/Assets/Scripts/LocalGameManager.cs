using UnityEngine;
using UnityEngine.Tilemaps;

public class LocalGameManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject[] placementPrefabs; // Prefabs for gnomes
    private GameObject placementIndicator = null;
    private int selectedGnomeIndex = -1; // Currently selected gnome (-1 means none)

    private void Start()
    {
        // Initialize references for the GameManager
        GameManager.Instance.cam = cam;
        GameManager.Instance.map = tilemap;
        GameManager.Instance.placementPrefabs = placementPrefabs;
    }

    private void Update()
    {
        HandlePlacement();
    }

    public void SelectGnome(int gnomeIndex)
    {
        if (placementIndicator != null)
            Destroy(placementIndicator);

        selectedGnomeIndex = gnomeIndex;

        if (selectedGnomeIndex >= 0 && selectedGnomeIndex < placementPrefabs.Length)
        {
            placementIndicator = Instantiate(placementPrefabs[selectedGnomeIndex]);
            placementIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); 
        }
    }

    private void HandlePlacement()
    {
        if (selectedGnomeIndex == -1 || placementIndicator == null)
            return;

        // Update placement indicator position
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition);
        placementIndicator.transform.position = tilemap.CellToWorld(cellPosition) + tilemap.cellSize * 0.5f;

        // Place gnome on left-click
        if (Input.GetMouseButtonDown(0))
        {
            if (CanPlaceGnome(cellPosition))
            {
                PlaceGnome(cellPosition);
            }
            else
            {
                Debug.Log("Cannot place gnome here.");
            }
        }
    }

    private bool CanPlaceGnome(Vector3Int cellPosition)
    {
        
        return GameManager.Instance.placedGnomes.ContainsKey(cellPosition) == false;
    }

    private void PlaceGnome(Vector3Int cellPosition)
    {
        GameObject gnome = Instantiate(placementPrefabs[selectedGnomeIndex]);
        gnome.transform.position = tilemap.CellToWorld(cellPosition) + tilemap.cellSize * 0.5f;

        GameManager.Instance.placedGnomes[cellPosition] = gnome.GetComponent<GnomeBase>();

        GameManager.Instance.UpdateEnergyUI();

        Destroy(placementIndicator);
        selectedGnomeIndex = -1;
    }
}
