using UnityEngine;
using UnityEngine.Tilemaps;

//Functions as a relay
public class LocalGameManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Camera cam;
    [SerializeField] GameObject[] placementPrefabs;

    private void Start()
    {
        GameManager.Instance.cam = cam;
        GameManager.Instance.map = tilemap;
        GameManager.Instance.placementPrefabs = placementPrefabs;
    }
}
