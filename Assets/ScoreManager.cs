using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public static bool gameEnded = false;

    public TextMeshProUGUI scoreText;
    public int totalFish = 3;

    private IntermissionManager intermissionManager;

    // Set this in Inspector (last level number)
    public int finalLevelIndex = 2;

    [Header("Spawner Mode")]
    [Tooltip("Enable when fish are spawned dynamically. Win by reaching targetScore instead of catching all fish.")]
    public bool spawnerMode = false;
    public int targetScore = 10;
    [HideInInspector] public bool endlessMode = false;

    [Header("Debug")]
    [Tooltip("Starting fish count. Set above 0 to skip ahead for testing.")]
    public int startScore = 0;

    void Start()
    {
        score = startScore;
        gameEnded = false;

        intermissionManager = GetComponent<IntermissionManager>();

        if (!spawnerMode)
            totalFish = GameObject.FindGameObjectsWithTag("Fish").Length;

        UpdateScore();
    }

    public void AddScore(int amount)
    {
        if (gameEnded) return;

        score += amount;

        if (!spawnerMode)
            totalFish--;

        UpdateScore();

        intermissionManager?.TryShow(score);

        if (intermissionManager == null || !intermissionManager.IsShowing)
            CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if (endlessMode) return;
        bool winConditionMet = spawnerMode ? score >= targetScore : totalFish <= 0;
        if (winConditionMet)
            LoadNextLevel();
    }

    void UpdateScore()
    {
        scoreText.text = "Fish Caught: " + score;
    }

    void LoadNextLevel()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (spawnerMode)
        {
            SceneManager.LoadScene("GameOverScene");
            return;
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex >= finalLevelIndex)
            SceneManager.LoadScene("GameOverScene");
        else
            SceneManager.LoadScene(currentIndex + 1);
    }
}
