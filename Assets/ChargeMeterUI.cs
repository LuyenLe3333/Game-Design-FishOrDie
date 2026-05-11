using UnityEngine;
using UnityEngine.UI;

public class ChargeMeterUI : MonoBehaviour
{
    public FishingRod fishingRod;
    public GameObject meterContainer;
    public Image fill;

    void Start()
    {
        if (meterContainer != null)
            meterContainer.SetActive(false);
    }

    void Update()
    {
        if (fishingRod == null || meterContainer == null) return;

        bool charging = fishingRod.IsCharging;
        meterContainer.SetActive(charging);

        if (!charging) return;

        float t = fishingRod.ChargeFraction;
        fill.fillAmount = t;
        fill.color = Color.Lerp(Color.red, Color.green, t);
    }
}
