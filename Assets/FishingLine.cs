using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    public Transform rodTip;
    public Transform hook;

    [Header("Style")]
    public float lineWidth = 0.08f;
    public Color lineColor = new Color(0.25f, 0.18f, 0.12f);
    [Tooltip("Larger = chunkier pixels. Smaller = finer pixels.")]
    public float pixelSize = 1f;

    private LineRenderer line;
    private Material lineMaterial;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        SetupPixelLine();
    }

    void SetupPixelLine()
    {
        line.positionCount = 2;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.useWorldSpace = true;
        line.textureMode = LineTextureMode.Tile;
        line.numCapVertices = 0;
        line.numCornerVertices = 0;

        Texture2D tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Repeat;

        Color a = lineColor;
        Color b = new Color(
            Mathf.Clamp01(lineColor.r + 0.1f),
            Mathf.Clamp01(lineColor.g + 0.08f),
            Mathf.Clamp01(lineColor.b + 0.05f)
        );

        tex.SetPixels(new Color[] {
            a, b, a, b,
            b, a, b, a,
            a, b, a, b,
            b, a, b, a
        });
        tex.Apply();

        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineMaterial.mainTexture = tex;
        line.material = lineMaterial;

        line.sortingLayerName = "Default";
        line.sortingOrder = 8;
    }

    void Update()
    {
        if (rodTip == null || hook == null) return;
        line.SetPosition(0, rodTip.position);
        line.SetPosition(1, hook.position);

        if (lineMaterial != null)
            lineMaterial.mainTextureScale = new Vector2(1f / Mathf.Max(pixelSize, 0.01f), 1f);
    }
}
