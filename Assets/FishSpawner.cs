using UnityEngine;
using System.Collections;

public class FishSpawner : MonoBehaviour
{
    [Header("Spawn Rate (seconds between spawns)")]
    public float initialSpawnInterval = 4f;
    public float minSpawnInterval = 1f;
    [Tooltip("Seconds until spawn rate reaches minimum")]
    public float spawnRampDuration = 60f;

    [Header("Fish Speed")]
    public float initialFishSpeed = 0.5f;
    public float maxFishSpeed = 3.5f;
    [Tooltip("Seconds until fish reach maximum speed")]
    public float speedRampDuration = 90f;

    [Header("Spawn Bounds (underwater area)")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -3.5f;
    public float maxY = 0.7f;

    [Header("Fish Prefabs (one per fish type)")]
    public GameObject[] fishPrefabs;

    [Header("Initial Fish")]
    [Tooltip("How many fish to spawn immediately when the game starts")]
    public int initialFishMin = 2;
    public int initialFishMax = 4;

    [Header("Population Cap")]
    public int maxFishInWater = 6;

    private float gameStartTime;

    void Start()
    {
        gameStartTime = Time.time;

        int initialCount = Random.Range(initialFishMin, initialFishMax + 1);
        for (int i = 0; i < initialCount; i++)
            SpawnFish();

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // Small initial delay so the scene finishes loading
        yield return new WaitForSeconds(1f);

        while (true)
        {
            float elapsed = Time.time - gameStartTime;
            float t = Mathf.Clamp01(elapsed / spawnRampDuration);
            float interval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);

            yield return new WaitForSeconds(interval);
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0)
        {
            Debug.LogWarning("FishSpawner: no fish prefabs assigned.");
            return;
        }

        if (GameObject.FindGameObjectsWithTag("Fish").Length >= maxFishInWater)
            return;

        float elapsed = Time.time - gameStartTime;
        float speedT = Mathf.Clamp01(elapsed / speedRampDuration);
        float speed = Mathf.Lerp(initialFishSpeed, maxFishSpeed, speedT);

        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        GameObject prefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        GameObject fish = Instantiate(prefab, spawnPos, Quaternion.identity);

        FishMovement movement = fish.GetComponent<FishMovement>();
        if (movement != null)
        {
            movement.speed = speed;
            movement.minX = minX;
            movement.maxX = maxX;
            movement.minY = minY;
            movement.maxY = maxY;
            movement.fleeProbability = Random.value;
        }
    }
}
