using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }

    [Range(0.1f, 2f)] public float fadeDuration = 0.5f;

    private Image overlay;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildOverlay();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void BuildOverlay()
    {
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        gameObject.AddComponent<CanvasScaler>();

        var imgGO = new GameObject("FadeOverlay");
        imgGO.transform.SetParent(transform, false);

        var rect = imgGO.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        overlay = imgGO.AddComponent<Image>();
        overlay.color = Color.black;
        overlay.raycastTarget = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOutThenLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        SetAlpha(1f);
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(1f - Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }
        SetAlpha(0f);
    }

    IEnumerator FadeOutThenLoad(string sceneName)
    {
        float t = 0f;
        SetAlpha(0f);
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }
        SetAlpha(1f);
        SceneManager.LoadScene(sceneName);
    }

    void SetAlpha(float a)
    {
        overlay.color = new Color(0f, 0f, 0f, a);
    }
}
