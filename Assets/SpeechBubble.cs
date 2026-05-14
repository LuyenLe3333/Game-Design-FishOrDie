using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
    public float displayDuration = 3f;
    public float fadeDuration = 0.4f;
    public bool showOnStart = true;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

        if (showOnStart)
            StartCoroutine(ShowAndHide());
    }

    public void Show()
    {
        StartCoroutine(ShowAndHide());
    }

    IEnumerator ShowAndHide()
    {
        // Fade in
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return StartCoroutine(Fade(1f, 0f));

        gameObject.SetActive(false);
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
