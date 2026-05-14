using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CastReelHelp : MonoBehaviour
{
    public FishingRod fishingRod;
    public TMP_Text castHelp;
    public TMP_Text reelHelp;

    [Header("Delays")]
    public float castHelpDelay = 5f;
    public float reelHelpDelay = 3f;

    private float castIdleTimer;
    private float hookAtBottomTimer;
    private bool castHelpDone;
    private bool reelHelpDone;

    void Start()
    {
        if (castHelp != null) castHelp.gameObject.SetActive(false);
        if (reelHelp != null) reelHelp.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleCastHelp();
        HandleReelHelp();
    }

    void HandleCastHelp()
    {
        if (castHelpDone) return;

        if (fishingRod.IsAtRod && !fishingRod.IsCharging)
        {
            castIdleTimer += Time.deltaTime;
            if (castIdleTimer >= castHelpDelay && castHelp != null)
                castHelp.gameObject.SetActive(true);
        }
        else
        {
            // Player started casting — dismiss and never show again
            if (castHelp != null) castHelp.gameObject.SetActive(false);
            castHelpDone = true;
        }
    }

    void HandleReelHelp()
    {
        if (reelHelpDone) return;

        // Player pressed X at any point — dismiss and never show again
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (reelHelp != null) reelHelp.gameObject.SetActive(false);
            reelHelpDone = true;
            return;
        }

        if (fishingRod.IsHookAtBottom)
        {
            hookAtBottomTimer += Time.deltaTime;
            if (hookAtBottomTimer >= reelHelpDelay && reelHelp != null)
                reelHelp.gameObject.SetActive(true);
        }
        else
        {
            hookAtBottomTimer = 0f;
        }
    }
}
