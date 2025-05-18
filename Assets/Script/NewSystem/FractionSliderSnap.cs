using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FractionSliderSnap : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI label; // Optional untuk menunjukkan 25%, 50%, dll

    private float[] steps = { 0.25f, 0.5f, 0.75f, 1.0f };

    public void OnSliderChanged()
    {
        float closest = FindClosestStep(slider.value);
        slider.value = closest;

        if (label != null)
        {
            label.text = $"{(int)(closest * 100)}%";
        }
    }

    private float FindClosestStep(float value)
    {
        float closest = steps[0];
        float minDiff = Mathf.Abs(value - closest);

        foreach (float step in steps)
        {
            float diff = Mathf.Abs(value - step);
            if (diff < minDiff)
            {
                minDiff = diff;
                closest = step;
            }
        }

        return closest;
    }
}
