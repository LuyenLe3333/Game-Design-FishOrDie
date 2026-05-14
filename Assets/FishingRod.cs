using UnityEngine;
using UnityEngine.InputSystem;

public class FishingRod : MonoBehaviour
{
    public Transform hook;
    public Transform rodTip;
    public AudioSource reelSound;
    public AudioSource splashSound;
    public GameObject splashPrefab;

    [Header("Cast")]
    public float castChargeTime = 4f;
    public float castMaxSpeed = 25f;
    public Vector2 castAngle = new Vector2(-1f, 0f);

    [Header("Arc & Water")]
    public float gravity = -9f;
    public float waterSurfaceY = 1.8225f;
    public float bottomLimit = -3f;

    [Header("Underwater")]
    public float sinkSpeed = 0.5f;
    public float reelingSpeed = 3f;

    // ── state machine ──────────────────────────────────────────────────────────
    private enum State { AtRod, Charging, InAir, InWater, Reeling }
    private State state = State.AtRod;

    public float ChargeFraction => castChargeTime > 0 ? chargeTimer / castChargeTime : 0f;
    public bool IsCharging => state == State.Charging;
    public bool IsHookOut => state == State.InAir || state == State.InWater || state == State.Reeling;
    public bool IsAtRod => state == State.AtRod;
    public bool IsHookAtBottom => state == State.InWater && hook != null && hook.position.y <= bottomLimit + 0.05f;

    private float chargeTimer;
    private Vector2 hookVelocity;
    private Vector3 hookHome;
    private Rigidbody2D hookRb;
    private HookCatch hookCatch;

    void Awake()
    {
        castAngle = castAngle.normalized;
        hookRb = hook.GetComponent<Rigidbody2D>();
        if (hookRb != null) hookRb.isKinematic = true;
        hookHome = hook.position;
        hookCatch = hook.GetComponent<HookCatch>();
    }

    void Update()
    {
        switch (state)
        {
            case State.AtRod:
            case State.Charging:
                UpdateCharging();
                break;
            case State.InAir:
                UpdateInAir();
                break;
            case State.InWater:
                UpdateInWater();
                break;
            case State.Reeling:
                UpdateReeling();
                break;
        }
    }

    // ── charging ───────────────────────────────────────────────────────────────

    void UpdateCharging()
    {
        if (Keyboard.current.spaceKey.isPressed && state != State.Charging)
            state = State.Charging;

        if (state == State.Charging && Keyboard.current.spaceKey.isPressed)
            chargeTimer = Mathf.Min(chargeTimer + Time.deltaTime, castChargeTime);

        if (state == State.Charging && Keyboard.current.spaceKey.wasReleasedThisFrame)
            Cast();
    }

    void Cast()
    {
        float power = chargeTimer / castChargeTime;
        chargeTimer = 0f;

        Vector3 origin = rodTip != null ? rodTip.position : hookHome;
        MoveHook(origin);
        hookVelocity = castAngle * (power * castMaxSpeed);
        state = State.InAir;
    }

    // ── airborne arc ───────────────────────────────────────────────────────────

    void UpdateInAir()
    {
        hookVelocity.y += gravity * Time.deltaTime;
        Vector3 next = hook.position + new Vector3(hookVelocity.x, hookVelocity.y, 0f) * Time.deltaTime;
        MoveHook(next);

        if (hook.position.y <= waterSurfaceY)
        {
            MoveHook(new Vector3(hook.position.x, waterSurfaceY, hook.position.z));
            hookVelocity = Vector2.zero;
            state = State.InWater;
            if (splashPrefab != null)
                Instantiate(splashPrefab, new Vector3(hook.position.x, waterSurfaceY, 0f), Quaternion.identity);
            if (splashSound != null)
                splashSound.Play();
        }
        else if (hook.position.y < bottomLimit)
        {
            MoveHook(new Vector3(hook.position.x, bottomLimit, hook.position.z));
            hookVelocity = Vector2.zero;
            state = State.InWater;
        }
    }

    // ── sinking in water ───────────────────────────────────────────────────────

    void UpdateInWater()
    {
        if (hook.position.y > bottomLimit)
        {
            float nextY = Mathf.Max(hook.position.y - sinkSpeed * Time.deltaTime, bottomLimit);
            MoveHook(new Vector3(hook.position.x, nextY, hook.position.z));
        }

        if (Keyboard.current.xKey.isPressed)
            state = State.Reeling;
    }

    // ── reeling (hold X) ───────────────────────────────────────────────────────

    void UpdateReeling()
    {
        Vector3 target = rodTip != null ? rodTip.position : hookHome;
        Vector3 toRod = target - hook.position;

        if (reelSound != null && !reelSound.isPlaying)
            reelSound.Play();

        if (toRod.magnitude <= 0.05f)
        {
            MoveHook(target);
            if (reelSound != null) reelSound.Stop();
            state = State.AtRod;
            hookCatch?.TryDeliverFish();
            return;
        }

        MoveHook(hook.position + toRod.normalized * reelingSpeed * Time.deltaTime);

        bool aboveWater = hook.position.y > waterSurfaceY;
        if (!aboveWater && !Keyboard.current.xKey.isPressed)
        {
            if (reelSound != null) reelSound.Stop();
            state = State.InWater;
        }
    }

    // ── helper ─────────────────────────────────────────────────────────────────

    void MoveHook(Vector3 pos)
    {
        hook.position = pos;
        if (hookRb != null) hookRb.position = pos;
    }
}