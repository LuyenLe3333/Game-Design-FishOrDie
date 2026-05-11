using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public static bool gameEnded = false;

    public TextMeshProUGUI scoreText;
    public int totalFish = 3;

    // Set this in Inspector (last level number)
    public int finalLevelIndex = 2;

    [Header("Spawner Mode")]
    [Tooltip("Enable when fish are spawned dynamically. Win by reaching targetScore instead of catching all fish.")]
    public bool spawnerMode = false;
    public int targetScore = 10;

    void Start()
    {
        score = 0;
        gameEnded = false;

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
