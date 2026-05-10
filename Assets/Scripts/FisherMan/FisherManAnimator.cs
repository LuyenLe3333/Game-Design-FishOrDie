using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FisherManAnimator : MonoBehaviour
{
    public FishingRod fishingRod;
    public HookCatch hookCatch;
    public GameObject hookObject;
    public LineRenderer fishingLine;

    private Animator animator;

    static readonly int HookOutHash = Animator.StringToHash("hookOut");
    static readonly int HasFishHash = Animator.StringToHash("hasFish");

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (fishingRod == null) fishingRod = GetComponent<FishingRod>();

        if (hookObject != null) hookObject.SetActive(false);
        if (fishingLine != null) fishingLine.enabled = false;
    }

    void Update()
    {
        if (fishingRod == null) return;

        bool hookOut = fishingRod.IsHookOut;
        bool hasFish = hookCatch != null && hookCatch.HasFish;

        animator.SetBool(HookOutHash, hookOut);
        animator.SetBool(HasFishHash, hasFish);

        if (hookObject != null) hookObject.SetActive(hookOut);
        if (fishingLine != null) fishingLine.enabled = hookOut;
    }
}
