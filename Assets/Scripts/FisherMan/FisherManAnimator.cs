using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FisherManAnimator : MonoBehaviour
{
    public FishingRod fishingRod;
    public HookCatch hookCatch;
    public GameObject hookObject;
    public LineRenderer fishingLine;
    public GameTimer gameTimer;
    public AudioSource deathSound;

    private Animator animator;
    private bool deathTriggered = false;

    static readonly int HookOutHash = Animator.StringToHash("hookOut");

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (fishingRod == null) fishingRod = GetComponent<FishingRod>();

        if (hookObject != null) hookObject.SetActive(false);
        if (fishingLine != null) fishingLine.enabled = false;
    }

    public void TriggerDeath()
    {
        if (deathTriggered) return;
        deathTriggered = true;
        animator.Play("FisherManDeath", 0, 0f);
        if (deathSound != null) deathSound.Play();
    }

    void Update()
    {
        if (!deathTriggered && gameTimer != null && gameTimer.timeRemaining <= 0)
        {
            deathTriggered = true;
            animator.Play("FisherManDeath", 0, 0f);
            if (deathSound != null) deathSound.Play();
            return;
        }

        if (deathTriggered) return;

        if (fishingRod == null) return;

        bool hookOut = fishingRod.IsHookOut;
        animator.SetBool(HookOutHash, hookOut);

        if (hookObject != null) hookObject.SetActive(hookOut);
        if (fishingLine != null) fishingLine.enabled = hookOut;
    }
}
