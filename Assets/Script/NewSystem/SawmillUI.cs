using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
public class SawmillUI : MonoBehaviour
{
    public LogProcessor processor;
    public LogData log;
    public TextMeshProUGUI outputText;
    public TMP_Dropdown typeDropdown;
    public Slider fractionSlider;

    public void OnProcess()
    {
        ProductType selected = (ProductType)typeDropdown.value;
        float fraction = fractionSlider.value;

        var result = processor.ProcessLog(log, fraction, selected);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Potongan {fraction * 100}% dari Log:");
        foreach (var product in result)
        {
            sb.AppendLine($"- {product.productName} x{product.quantity}");
        }

        outputText.text = sb.ToString();
    }
}
