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

            // Set transparency for placement preview
            SpriteRenderer sr = placementIndicator.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(1f, 1f, 1f, 0.5f);
                sr.sortingLayerName = "Foreground"; 
            }
        }
    }

    private void HandlePlacement()
    {
        if (selectedGnomeIndex == -1 || placementIndicator == null)
            return;

        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition);
        placementIndicator.transform.position = tilemap.CellToWorld(cellPosition) + tilemap.cellSize * 0.5f;

        
        Debug.Log($"Placement Indicator Position: {placementIndicator.transform.position}, Cell: {cellPosition}");

        
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
        return !GameManager.Instance.placedGnomes.ContainsKey(cellPosition);
    }

    private void PlaceGnome(Vector3Int cellPosition)
    {
        if (selectedGnomeIndex < 0 || selectedGnomeIndex >= placementPrefabs.Length)
        {
            Debug.LogError("Invalid gnome selection!");
            return;
        }

        GameObject gnome = Instantiate(placementPrefabs[selectedGnomeIndex]);
        gnome.transform.position = tilemap.CellToWorld(cellPosition) + tilemap.cellSize * 0.5f;

        // Debugging
        Debug.Log($"Placing gnome at: {gnome.transform.position}, Cell: {cellPosition}");

        SpriteRenderer sr = gnome.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 1f); 
            sr.sortingLayerName = "Foreground";
        }
        else
        {
            Debug.LogWarning("SpriteRenderer missing from Gnome prefab!");
        }

        GameManager.Instance.placedGnomes[cellPosition] = gnome.GetComponent<GnomeBase>();

        GameManager.Instance.UpdateEnergyUI();

        Destroy(placementIndicator);
        selectedGnomeIndex = -1;
    }
}
