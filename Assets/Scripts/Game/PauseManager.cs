using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Canvas gameCanvas;
    public GameObject pauseMenu;

    private bool isPaused = false;
    private List<GameObject> gameUIChildren = new List<GameObject>();
    private Dictionary<GameObject, bool> savedActiveStates = new Dictionary<GameObject, bool>();

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (gameCanvas != null)
        {
            foreach (Transform child in gameCanvas.transform)
            {
                if (child.gameObject != pauseMenu)
                    gameUIChildren.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !ScoreManager.gameEnded)
            TogglePause();

        if (isPaused && Keyboard.current.digit1Key.wasPressedThisFrame)
            RestartScene();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (backgroundMusic != null)
        {
            if (isPaused) backgroundMusic.Pause();
            else backgroundMusic.UnPause();
        }

        if (isPaused)
        {
            savedActiveStates.Clear();
            foreach (var child in gameUIChildren)
            {
                savedActiveStates[child] = child.activeSelf;
                child.SetActive(false);
            }
        }
        else
        {
            foreach (var child in gameUIChildren)
            {
                if (savedActiveStates.TryGetValue(child, out bool wasActive))
                    child.SetActive(wasActive);
            }
        }

        if (pauseMenu != null)
            pauseMenu.SetActive(isPaused);
    }

    void RestartScene()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        if (SceneFader.Instance != null)
            SceneFader.Instance.FadeTo(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
