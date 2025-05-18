using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WoodCutterUIHandler : MonoBehaviour
{
    public Dropdown woodDropdown;
    public InputField quantityInput1;
    public InputField sizeInput1; // format: 4x1x1
    public InputField quantityInput2;   
    public InputField sizeInput2; // format: 4x4x4

    public WoodCutterManager cutterManager;
    public Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(() => OnStartCutting());
    }

    void OnStartCutting()
    {
        int index = woodDropdown.value;
        cutterManager.selectedWood = cutterManager.availableWoodTypes[index];

        cutterManager.cutRequests = new List<CutRequest>();

        cutterManager.cutRequests.Add(new CutRequest
        {
            name = "Plank",
            desiredSizeCM = ParseSize(sizeInput1.text),
            quantityRequested = int.Parse(quantityInput1.text)
        });

        cutterManager.cutRequests.Add(new CutRequest
        {
            name = "Papan",
            desiredSizeCM = ParseSize(sizeInput2.text),
            quantityRequested = int.Parse(quantityInput2.text)
        });

        cutterManager.StartCutting();
    }

    Vector3 ParseSize(string input)
    {
        string[] parts = input.Split('x');
        if (parts.Length != 3) return Vector3.one * 10f;
        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2])
        );
    }
}
