using UnityEngine;

public class SplashAnimation : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 12f;

    private SpriteRenderer sr;
    private float timer;
    private int currentFrame;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0)
            sr.sprite = frames[0];
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        int frame = Mathf.FloorToInt(timer * fps);

        if (frame >= frames.Length)
        {
            Destroy(gameObject);
            return;
        }

        if (frame != currentFrame)
        {
            currentFrame = frame;
            sr.sprite = frames[currentFrame];
        }
    }
}
