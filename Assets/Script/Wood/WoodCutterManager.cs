using System.Collections.Generic;
using UnityEngine;

public class WoodCutterManager : MonoBehaviour
{
    public List<WoodTypeSO> availableWoodTypes;
    public Transform spawnPoint;

    [Header("Input")]
    public WoodTypeSO selectedWood;
    public List<CutRequest> cutRequests;

    public void StartCutting()
    {
        GameObject logObj = Instantiate(selectedWood.woodPrefab, spawnPoint.position, Quaternion.identity);
        Vector3 logSizeMeters = logObj.transform.localScale;

        Vector3 logSizeCM = logSizeMeters * 100f;

        if (selectedWood.isBent)
        {
            Debug.Log("Processing bent log...");
            logSizeCM = EstimateStraightPart(logSizeCM); // simulasikan potong bagian lurus
        }

        foreach (CutRequest request in cutRequests)
        {
            int totalPieces = CalculateOutput(logSizeCM, request.desiredSizeCM);
            Debug.Log($"Can produce {totalPieces} pieces of {request.name} from this log");
        }
    }

    private Vector3 EstimateStraightPart(Vector3 originalSize)
    {
        // misal potong bagian bengkok, sisakan 70% bagian lurus
        return new Vector3(originalSize.x * 0.7f, originalSize.y, originalSize.z);
    }

    private int CalculateOutput(Vector3 logSize, Vector3 outputSize)
    {
        int countX = Mathf.FloorToInt(logSize.x / outputSize.x);
        int countY = Mathf.FloorToInt(logSize.y / outputSize.y);
        int countZ = Mathf.FloorToInt(logSize.z / outputSize.z);
        return countX * countY * countZ;
    }
}
