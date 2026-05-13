using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText;

    private bool isRunning = true;
    private bool isPaused = false;

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
        if (!isPaused)
            isRunning = true;
    }

    public void Pause()
    {
        isPaused = true;
        isRunning = false;
    }

    public void Resume()
    {
        isPaused = false;
        if (!ScoreManager.gameEnded)
            isRunning = true;
    }

    void LoseGame()
    {
        if (ScoreManager.gameEnded) return;
        ScoreManager.gameEnded = true;
        StartCoroutine(LoseGameDelay());
    }

    IEnumerator LoseGameDelay()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("YouLoseScene");
    }
}
