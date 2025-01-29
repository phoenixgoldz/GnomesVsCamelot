using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SavingLogoManager : MonoBehaviour
{
    public static SavingLogoManager Instance;
    private RectTransform logoTransform;
    private Image savingImage;
    private bool isSaving = false;

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Get the RectTransform component for rotation
        logoTransform = GetComponent<RectTransform>();
        savingImage = GetComponent<Image>();

        // Hide logo initially
        savingImage.enabled = false;
    }

    private void Update()
    {
        // Rotate the logo on Z-axis when active
        if (isSaving)
        {
            logoTransform.Rotate(0, 0, 200 * Time.deltaTime);
        }
    }

    public void ShowSavingLogo()
    {
        isSaving = true;
        savingImage.enabled = true;
        StartCoroutine(HideAfterDelay(2.5f)); // Hide after 2.5 seconds
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSaving = false;
        savingImage.enabled = false;
    }
}
