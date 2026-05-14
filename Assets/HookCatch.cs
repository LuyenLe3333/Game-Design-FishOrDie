using UnityEngine;

public class HookCatch : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameTimer gameTimer;
    public float timeBonus = 5f;
    public AudioSource catchSound;

    [Header("Escape Timer")]
    public float escapeTime = 4f;

    public bool HasFish => caughtFish != null;
    public bool FishDelivered { get; private set; }

    private GameObject caughtFish = null;
    private float escapeTimer = 0f;
    private float deliveredTimer = 0f;
    private const float deliveredDuration = 0.8f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && caughtFish == null)
        {
            caughtFish = other.gameObject;
            caughtFish.transform.SetParent(transform);

            FishMovement movement = caughtFish.GetComponent<FishMovement>();
            if (movement != null)
                movement.enabled = false;

            escapeTimer = escapeTime;

            if (catchSound != null)
                catchSound.Play();
        }
    }

    void Update()
    {
        if (FishDelivered)
        {
            deliveredTimer -= Time.deltaTime;
            if (deliveredTimer <= 0f)
                FishDelivered = false;
        }

        if (caughtFish != null)
        {
            escapeTimer -= Time.deltaTime;
            if (escapeTimer <= 0f)
                ReleaseFish();
        }
    }

    void ReleaseFish()
    {
        if (caughtFish == null) return;

        caughtFish.transform.SetParent(null);

        FishMovement movement = caughtFish.GetComponent<FishMovement>();
        if (movement != null)
            movement.enabled = true;

        caughtFish = null;
    }

    public void TryDeliverFish()
    {
        if (caughtFish == null) return;

        if (scoreManager != null)
            scoreManager.AddScore(1);
        else
            Debug.LogWarning("HookCatch: scoreManager is not assigned!");

        if (gameTimer != null)
            gameTimer.AddTime(timeBonus);

        Destroy(caughtFish);
        caughtFish = null;

        FishDelivered = true;
        deliveredTimer = deliveredDuration;
    }
}
