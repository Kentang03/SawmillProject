using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class SawmillConverter : MonoBehaviour
{
    [Header("Make Sawmill Converter Global Script")]
    public static SawmillConverter Instance { get; private set; }

    [Header("Settings")]
    public List<Transform> outputSpawnPoint;
    public float conversionDelay = 5f; // delay antar output

    [Header("Conveyor")]
    public ConveyorScript conveyor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void StartConversion(ObjectDataSO inputObjectData)
    {

        if (inputObjectData == null || inputObjectData.outputPrefabs == null || inputObjectData.outputPrefabs.Count == 0)
        {
            Debug.LogWarning("No input data or output prefabs found.");
            return;
        }

        StartCoroutine(ConversionRoutine(inputObjectData));
    }

    private IEnumerator ConversionRoutine(ObjectDataSO inputObjectData)
    {
        yield return new WaitForSeconds(conversionDelay); // small wait before conversion starts

        int spawnIndex = 0;
        for (int i = 0; i < inputObjectData.outputPrefabs.Count; i++)
        {
            GameObject output = inputObjectData.outputPrefabs[i];

            if (output != null && outputSpawnPoint.Count > 0)
            {
                if (spawnIndex >= outputSpawnPoint.Count)
                {
                    spawnIndex = 0; 
                }
                Debug.Log($"Spawning at index: {spawnIndex} of {outputSpawnPoint.Count}");
                Instantiate(output, outputSpawnPoint[spawnIndex].position, output.transform.rotation);

                spawnIndex++;
                yield return new WaitForSeconds(conversionDelay);
            }
        }
    }

}
