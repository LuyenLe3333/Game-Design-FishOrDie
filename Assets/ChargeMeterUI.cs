using UnityEngine;
using UnityEngine.UI;

public class ChargeMeterUI : MonoBehaviour
{
    public FishingRod fishingRod;
    public GameObject meterContainer;
    public Image fill;

    private RectTransform fillRect;

    void Start()
    {
        if (meterContainer != null)
            meterContainer.SetActive(false);

        if (fill != null)
        {
            fill.type = Image.Type.Simple;
            fillRect = fill.rectTransform;
            // Start fully collapsed on the right edge
            fillRect.anchorMin = new Vector2(1f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.sizeDelta = Vector2.zero;
        }
    }

    void Update()
    {
        if (fishingRod == null || meterContainer == null) return;

        bool charging = fishingRod.IsCharging;
        meterContainer.SetActive(charging);

        if (!charging) return;

        float t = fishingRod.ChargeFraction;
        // anchorMin.x goes from 1 (empty) down to 0 (full), anchored to right edge
        fillRect.anchorMin = new Vector2(1f - t, 0f);
        fill.color = Color.Lerp(Color.red, Color.green, t);
    }
}
