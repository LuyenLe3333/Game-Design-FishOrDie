using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour
{
    public GameTimer gameTimer;
    public SpeechBubble bearSpeechBubble;
    public SpeechBubble speechBubble3;
    public FisherManAnimator fisherManAnimator;
    public AudioSource roarSound;

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

    public void TriggerKillSequence()
    {
        StartCoroutine(KillSequence());
    }

    IEnumerator KillSequence()
    {
        yield return null; // let current frame settle

        // Step 1: if sitting or going to sit, get up first
        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("BearSit") || state.IsName("BearGoingToSit"))
        {
            animator.SetTrigger("getUp");
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("BearIdle"));
        }
        else if (state.IsName("BearGetUp") || state.IsName("BearReverseStand"))
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("BearIdle"));
        }

        // Step 2: if idle, stand up
        state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("BearIdle"))
        {
            animator.SetTrigger("stand");
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("BearHoldingStand"));
        }
        else if (state.IsName("BearStand"))
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("BearHoldingStand"));
        }

        // Step 3: now in BearHoldingStand — fire kill
        animator.SetTrigger("kill");
        if (roarSound != null) roarSound.Play();
        if (fisherManAnimator != null) fisherManAnimator.TriggerDeath();
        if (speechBubble3 != null) speechBubble3.Show();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("BearAttack"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
        yield return new WaitForSeconds(0.3f);
        IntermissionManager.GoToGameOver();
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
                if (roarSound != null) roarSound.Play();
                if (fisherManAnimator != null) fisherManAnimator.TriggerDeath();
                if (bearSpeechBubble != null) bearSpeechBubble.Show();
                break;
        }
    }
}
