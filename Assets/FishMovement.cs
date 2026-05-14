using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 1f;
    public AudioSource swimSound;

    [Header("Underwater Bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -3.5f;
    public float maxY = 0.7f;

    [Header("Flee Behavior")]
    [Range(0f, 1f)] public float fleeProbability = 0.5f;
    public float fleeDetectionRadius = 2.5f;
    public float fleeSpeedMultiplier = 2f;

    private Vector2 direction;
    private float nextTurnTime;
    private SpriteRenderer spriteRenderer;

    private Transform hookTransform;
    private bool isFleeing = false;
    private float fleeCheckTimer = 0f;
    private const float fleeCheckInterval = 0.5f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
        ScheduleNextTurn();
        FlipSprite();

        GameObject hookGO = GameObject.FindWithTag("Hook");
        if (hookGO != null) hookTransform = hookGO.transform;

        if (swimSound != null)
            swimSound.Play();
    }

    void Update()
    {
        CheckFlee();

        float currentSpeed = isFleeing ? speed * fleeSpeedMultiplier : speed;

        if (!isFleeing && Time.time >= nextTurnTime)
        {
            float angle = Random.Range(-60f, 60f);
            direction = Quaternion.Euler(0f, 0f, angle) * direction;
            direction.Normalize();
            ScheduleNextTurn();
            FlipSprite();
        }

        transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);

        // Bounce off bounds
        Vector3 pos = transform.position;
        bool bounced = false;

        if (pos.x < minX) { pos.x = minX; direction.x = Mathf.Abs(direction.x); bounced = true; }
        else if (pos.x > maxX) { pos.x = maxX; direction.x = -Mathf.Abs(direction.x); bounced = true; }

        if (pos.y < minY) { pos.y = minY; direction.y = Mathf.Abs(direction.y); }
        else if (pos.y > maxY) { pos.y = maxY; direction.y = -Mathf.Abs(direction.y); }

        transform.position = pos;

        if (bounced)
            FlipSprite();
    }

    void CheckFlee()
    {
        if (hookTransform == null || !hookTransform.gameObject.activeInHierarchy)
        {
            isFleeing = false;
            return;
        }

        fleeCheckTimer -= Time.deltaTime;
        if (fleeCheckTimer > 0f) return;
        fleeCheckTimer = fleeCheckInterval;

        float dist = Vector2.Distance(transform.position, hookTransform.position);
        if (dist < fleeDetectionRadius)
        {
            // Roll against this fish's personality
            if (Random.value < fleeProbability)
            {
                Vector2 awayFromHook = ((Vector2)transform.position - (Vector2)hookTransform.position).normalized;
                direction = Vector2.Lerp(direction, awayFromHook, 0.8f).normalized;
                isFleeing = true;
                FlipSprite();
            }
        }
        else
        {
            isFleeing = false;
        }
    }

    void ScheduleNextTurn()
    {
        nextTurnTime = Time.time + Random.Range(1.5f, 3.5f);
    }

    void FlipSprite()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0f;
    }
}
