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
        animator.SetBool(HookOutHash, hookOut);

        if (hookObject != null) hookObject.SetActive(hookOut);
        if (fishingLine != null) fishingLine.enabled = hookOut;
    }
}
