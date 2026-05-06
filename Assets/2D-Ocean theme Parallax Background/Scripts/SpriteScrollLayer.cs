using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteScrollLayer : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float speed = 0.05f;

    private SpriteRenderer sr;
    private SpriteRenderer copySR;
    private float spriteWidth;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Use sprite's local bounds × scale — sr.bounds can be zero before first render
        spriteWidth = sr.sprite.bounds.size.x * transform.localScale.x;

        var copy = new GameObject(name + "_copy");
        copy.transform.SetParent(transform.parent);
        copy.transform.position = transform.position + Vector3.right * spriteWidth;
        copy.transform.localScale = transform.localScale;

        copySR = copy.AddComponent<SpriteRenderer>();
        copySR.sprite = sr.sprite;
        copySR.sortingLayerID = sr.sortingLayerID;
        copySR.sortingOrder = sr.sortingOrder;
    }

    void Update()
    {
        float move = speed * Time.deltaTime;
        transform.position += Vector3.left * move;
        copySR.transform.position += Vector3.left * move;

        var cam = Camera.main;
        float camLeft = cam.transform.position.x - cam.orthographicSize * cam.aspect;

        if (sr.bounds.max.x < camLeft)
            transform.position = copySR.transform.position + Vector3.right * spriteWidth;
        else if (copySR.bounds.max.x < camLeft)
            copySR.transform.position = transform.position + Vector3.right * spriteWidth;
    }
}
