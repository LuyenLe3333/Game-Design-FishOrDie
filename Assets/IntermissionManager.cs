using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntermissionManager : MonoBehaviour
{
    [Header("References")]
    public GameObject intermissionBubble;
    public GameObject timerTextObject;
    public GameObject scoreTextObject;
    public GameTimer gameTimer;
    public HookCatch hookCatch;
    public BearController bearController;

    [Header("Settings")]
    public int triggerScore = 20;
    public float endlessTimerReset = 30f;
    public float endlessTimeBonus = 3f;

    public bool IsShowing { get; private set; }

    private bool shown = false;

    void Start()
    {
        intermissionBubble.SetActive(false);

        Button fineBtn = intermissionBubble.transform.Find("Fine")?.GetComponent<Button>();
        Button imDoneBtn = intermissionBubble.transform.Find("ImDone")?.GetComponent<Button>();

        if (fineBtn != null) fineBtn.onClick.AddListener(OnFinePressed);
        if (imDoneBtn != null) imDoneBtn.onClick.AddListener(OnImDonePressed);
    }

    public void TryShow(int score)
    {
        if (!shown && score >= triggerScore)
            ShowIntermission();
    }

    void ShowIntermission()
    {
        shown = true;
        IsShowing = true;
        intermissionBubble.SetActive(true);
        timerTextObject.SetActive(false);
        scoreTextObject.SetActive(false);
        gameTimer.Pause();
    }

    void OnFinePressed()
    {
        GetComponent<ScoreManager>().endlessMode = true;

        if (hookCatch != null)
            hookCatch.timeBonus = endlessTimeBonus;

        gameTimer.timeRemaining = endlessTimerReset;
        HideIntermission();
    }

    void OnImDonePressed()
    {
        intermissionBubble.SetActive(false);
        if (bearController != null)
            bearController.TriggerKillSequence();
        else
            GoToGameOver();
    }

    public static void GoToGameOver()
    {
        if (SceneFader.Instance != null)
            SceneFader.Instance.FadeTo("GameOverScene");
        else
            SceneManager.LoadScene("GameOverScene");
    }

    public void HideIntermission()
    {
        IsShowing = false;
        intermissionBubble.SetActive(false);
        timerTextObject.SetActive(true);
        scoreTextObject.SetActive(true);
        gameTimer.Resume();
        GetComponent<ScoreManager>()?.CheckWinCondition();
    }
}
