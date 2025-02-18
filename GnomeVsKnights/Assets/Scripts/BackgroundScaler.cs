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
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        float spriteHeight = sr.sprite.bounds.size.y;
        float spriteWidth = sr.sprite.bounds.size.x;

        // Ensure full coverage without distortion
        float scaleY = screenHeight / spriteHeight;
        float scaleX = screenWidth / spriteWidth;
        float finalScale = Mathf.Max(scaleX, scaleY); // Use the larger scale

        // Apply scaling
        transform.localScale = new Vector3(finalScale, finalScale, 1f);

        // Center the background
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}
