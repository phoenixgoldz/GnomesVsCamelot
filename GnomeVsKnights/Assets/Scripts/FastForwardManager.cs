using UnityEngine;

public class FastForwardManager : MonoBehaviour
{
    private bool isFastForward = false;

    public void ToggleFastForward()
    {
        if (isFastForward)
        {
            Time.timeScale = 1f;  // Normal speed
        }
        else
        {
            Time.timeScale = 2f;  // Double speed
        }

        isFastForward = !isFastForward;
    }
}
