using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public TextMeshProUGUI timerText;

    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        UpdateTimerUI();

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            LoseGame();
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time until you <color=red>DIE</color>: <color=red>" + Mathf.Ceil(timeRemaining).ToString() + "</color>";
    }

    public void AddTime(float seconds)
    {
        timeRemaining += seconds;
        isRunning = true;
    }

    void LoseGame()
    {
        if (ScoreManager.gameEnded) return;
        ScoreManager.gameEnded = true;
        SceneManager.LoadScene("YouLoseScene");
    }
}
