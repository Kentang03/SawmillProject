using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LogCuttingUIController : MonoBehaviour
{
    public TMP_Dropdown logDropdown;
    public TMP_Dropdown outputTypeDropdown;
    public TMP_InputField inputWidth;
    public TMP_InputField inputHeight;
    public Button startButton;
    public TextMeshProUGUI resultText;

    public List<LogInfo> logs;
    public LogCuttingProcessor processor;

    private void Start()
    {
        PopulateLogDropdown();
        PopulateOutputTypeDropdown();
        startButton.onClick.AddListener(ProcessLog);
    }

    void PopulateLogDropdown()
    {
        logDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var log in logs)
        {
            options.Add($"Log: {log.length} M x {log.diameter} CM");
        }
        logDropdown.AddOptions(options);
    }

    void PopulateOutputTypeDropdown()
    {
        outputTypeDropdown.ClearOptions();
        var types = System.Enum.GetNames(typeof(WoodProductType));
        outputTypeDropdown.AddOptions(new List<string>(types));
    }

    void ProcessLog()
    {
        var selectedLog = logs[logDropdown.value];
        var typeStr = outputTypeDropdown.options[outputTypeDropdown.value].text;

        if (!float.TryParse(inputWidth.text, out float widthCm) ||
            !float.TryParse(inputHeight.text, out float heightCm))
        {
            resultText.text = "Input Width dan Height tidak valid!";
            return;
        }

        var type = (WoodProductType)System.Enum.Parse(typeof(WoodProductType), typeStr);

        if (processor == null)
        {
            Debug.Log("Processor is null");
            return;
        }

        var results = processor.ProcessSingleType(selectedLog, widthCm, heightCm, type);
        resultText.text = string.Join("\n", results.ConvertAll(p => $"{p.productName} x{p.quantity}"));
    }

}
