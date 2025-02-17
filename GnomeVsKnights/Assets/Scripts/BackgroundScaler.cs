using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private void Start()
    {
        AdjustBackgroundSize();
    }

    private void AdjustBackgroundSize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        Camera cam = Camera.main;

        float screenHeight = cam.orthographicSize * 2; // Get screen height in world units
        float screenWidth = screenHeight * Screen.width / Screen.height; // Calculate width in world units

        Vector3 newScale = transform.localScale;
        newScale.x = screenWidth / sr.bounds.size.x;  // Scale X based on screen width
        newScale.y = screenHeight / sr.bounds.size.y; // Scale Y based on screen height
        transform.localScale = newScale;

        // Ensure background is centered
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}
