using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private readonly Vector3 referenceScale = new Vector3(1.40493071f, 1.33465612f, 1.40493071f); // Your manually set scale for 16:9
    private readonly float referenceAspectRatio = 16f / 9f; // The target aspect ratio for your reference scale

    private void Start()
    {
        AdjustBackgroundSize();
    }

    private void AdjustBackgroundSize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        Camera cam = Camera.main;
        float currentAspectRatio = (float)Screen.width / Screen.height; // Get the current screen's aspect ratio

        float scaleMultiplier = currentAspectRatio / referenceAspectRatio; // Scale based on aspect ratio difference

        Vector3 adjustedScale = new Vector3(
            referenceScale.x * scaleMultiplier,
            referenceScale.y * scaleMultiplier,
            referenceScale.z * scaleMultiplier
        );

        transform.localScale = adjustedScale;

        // Ensure background remains centered
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}
