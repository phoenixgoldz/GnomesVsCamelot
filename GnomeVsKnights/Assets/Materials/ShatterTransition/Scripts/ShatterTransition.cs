using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ShatterTransition : MonoBehaviour
{
    public float transitionDuration = 1.5f;
    public float fadeSpeed = 2f;
    private Transform[] shards;
    private CanvasGroup canvasGroup;

    void Start()
    {
        shards = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            shards[i] = transform.GetChild(i);
        }

        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        StartCoroutine(ShatterEffect());
    }

    IEnumerator ShatterEffect()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Transform shard in shards)
        {
            StartCoroutine(MoveShard(shard));
        }

        float fadeTimer = 0;
        while (fadeTimer < transitionDuration)
        {
            fadeTimer += Time.deltaTime;
            canvasGroup.alpha = 1 - (fadeTimer / transitionDuration);
            yield return null;
        }

        SceneManager.LoadScene("GameScene");
    }

    IEnumerator MoveShard(Transform shard)
    {
        Vector3 randomDirection = new Vector3(Random.Range(-100f, 100f), Random.Range(-50f, 150f), 0);
        Vector3 startPosition = shard.position;
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            shard.position = Vector3.Lerp(startPosition, startPosition + randomDirection, elapsedTime / transitionDuration);
            shard.localScale = Vector3.Lerp(shard.localScale, Vector3.zero, elapsedTime / transitionDuration);
            yield return null;
        }

        shard.gameObject.SetActive(false);
    }
}
