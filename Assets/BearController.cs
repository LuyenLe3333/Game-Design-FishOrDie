using UnityEngine;

public class BearController : MonoBehaviour
{
    public GameTimer gameTimer;
    public SpeechBubble bearSpeechBubble;

    private Animator animator;
    private int currentZone = -1;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (gameTimer != null)
            currentZone = GetZone(gameTimer.timeRemaining);
    }

    int GetZone(float t)
    {
        if (t > 20f) return 0;
        if (t > 10f) return 1;
        if (t > 5f)  return 2;
        if (t > 0f)  return 3;
        return 4;
    }

    void Update()
    {
        if (gameTimer == null) return;

        int zone = GetZone(gameTimer.timeRemaining);
        if (zone == currentZone) return;

        int previousZone = currentZone;
        currentZone = zone;
        switch (zone)
        {
            case 0: animator.SetTrigger("sit");                                        break;
            case 1: animator.SetTrigger(previousZone > 1 ? "getDown" : "getUp");      break;
            case 2: animator.SetTrigger("stand");                                      break;
            case 4:
                animator.SetTrigger("kill");
                if (bearSpeechBubble != null) bearSpeechBubble.Show();
                break;
        }
    }
}
