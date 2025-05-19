using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SawmillUIManager : MonoBehaviour
{
    [Header("Data")]
    public LogProcessor processor;
    public LogData inputLog;
    public List<ProductSize> allSizes;

    [Header("UI")]
    public TMP_Dropdown balokDropdown;
    public TMP_Dropdown papanDropdown;
    public TMP_Text resultText;
    public Button processButton;

    [Header("Slider Fraksi Log")]
    public Slider logFractionSlider;

    private List<ProductSize> balokSizes;
    private List<ProductSize> papanSizes;

    void Start()
    {
        // Setup dropdowns
        balokSizes = allSizes.Where(p => p.type == ProductType.Balok).ToList();
        papanSizes = allSizes.Where(p => p.type == ProductType.Papan).ToList();
        SetupDropdown(balokDropdown, balokSizes);
        SetupDropdown(papanDropdown, papanSizes);

        // Setup slider
        logFractionSlider.onValueChanged.AddListener(UpdateFractionLabel);
        UpdateFractionLabel(logFractionSlider.value);

        // Process button
        processButton.onClick.AddListener(ProcessLog);
    }

    void SetupDropdown(TMP_Dropdown dropdown, List<ProductSize> sizeList)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string> { "Tidak Dipilih" };
        options.AddRange(sizeList.Select(p => $"{p.heightCm}x{p.widthCm} cm"));
        dropdown.AddOptions(options);
        dropdown.value = 0;
    }

    void UpdateFractionLabel(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
    }

    void ProcessLog()
    {
        float fraction = logFractionSlider.value;

        ProductSize selectedBalok = balokDropdown.value > 0 ? balokSizes[balokDropdown.value - 1] : null;
        ProductSize selectedPapan = papanDropdown.value > 0 ? papanSizes[papanDropdown.value - 1] : null;

        List<WoodProduct> results = processor.ProcessWithDropdown(inputLog, selectedBalok, selectedPapan, fraction);

        resultText.text = string.Join("\n", results.Select(r => $"{r.productName} x{r.quantity}"));
    }
}
