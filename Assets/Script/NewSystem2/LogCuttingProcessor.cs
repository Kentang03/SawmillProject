using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCuttingProcessor : MonoBehaviour
{
    public Transform spawnPosition; // awal posisi log muncul
    public List<Transform> outputSpawnPoints; // posisi spawn output

    // Prefabs untuk tiap jenis produk
    public GameObject balokPrefab;
    public GameObject papanPrefab;
    public GameObject usukPrefab;
    public GameObject rengPrefab;
    [SerializeField] private List<Cloth> clothList;
    private int currentSpawnIndex = 0;

    private Queue<SpawnRequest> spawnQueue = new Queue<SpawnRequest>();
    private bool isSpawning = false;

    private class SpawnRequest
    {
        public int quantity;
        public float width;
        public float height;
        public float length;
        public WoodProductType productType;
    }

    public List<CuttingResult> ProcessSingleType(LogInfo log, float widthCm, float heightCm, WoodProductType type)
    {
        float diameter = log.diameter;
        float logLength = log.length;

        GameObject logPrefabs = log.logPrefabs;
        List<CuttingResult> result = new List<CuttingResult>();

        if (widthCm > diameter || heightCm > diameter)
        {
            result.Add(new CuttingResult
            {
                productName = "Ukuran output melebihi diameter log",
                quantity = 0
            });
            return result;
        }

        int verticalCount = Mathf.FloorToInt(diameter / heightCm);
        int horizontalCount = Mathf.FloorToInt(diameter / widthCm);
        int totalCount = verticalCount * horizontalCount;

        if (totalCount > 0)
        {
            result.Add(new CuttingResult
            {
                productName = $"Menghasilkan {type}:\n" +
              $"tinggi {heightCm} cm\n" +
              $"lebar {widthCm} cm\n" +
              $"panjang {logLength} cm\n" +
              $"sebanyak ",
            quantity = totalCount
            });

            // Tambahkan ke antrian spawn
            spawnQueue.Enqueue(new SpawnRequest
            {
                quantity = totalCount,
                width = widthCm,
                height = heightCm,
                length = logLength,
                productType = type
            });

            InstantiateLog(log, diameter, type);
        }
        else
        {
            result.Add(new CuttingResult
            {
                productName = "Diameter log terlalu kecil untuk ukuran ini",
                quantity = 0
            });
        }

        return result;
    }

    public void StartSpawningOutputs()
    {
        if (!isSpawning && spawnQueue.Count > 0)
        {
            var request = spawnQueue.Dequeue();
            StartCoroutine(SpawnOutputRoutine(request));
        }
    }

    private IEnumerator SpawnOutputRoutine(SpawnRequest request)
    {
        isSpawning = true;

        int spawned = 0;

        while (spawned < request.quantity)
        {
            Transform spawnPoint = outputSpawnPoints[currentSpawnIndex];
            GameObject prefabToSpawn = GetPrefabByType(request.productType);
            Quaternion spawnRotation = Quaternion.Euler(0, 90, 0);
            GameObject newObj = Instantiate(prefabToSpawn, spawnPoint.position, spawnRotation);

            // Atur skala output sesuai input user (cm ke meter)
            newObj.transform.localScale = new Vector3(
            request.length / 100f,   // panjang ke X
            request.height / 100f,   // tinggi ke Y
            request.width / 100f     // lebar ke Z
        );

            currentSpawnIndex = (currentSpawnIndex + 1) % outputSpawnPoints.Count;
            spawned++;

            yield return new WaitForSeconds(1f); // jeda antar spawn
        }

        isSpawning = false;

        // Proses antrian berikutnya
        if (spawnQueue.Count > 0)
        {
            StartSpawningOutputs();
        }
    }

    private GameObject GetPrefabByType(WoodProductType type)
    {
        switch (type)
        {
            case WoodProductType.Balok: return balokPrefab;
            case WoodProductType.Papan: return papanPrefab;
            case WoodProductType.Usuk: return usukPrefab;
            case WoodProductType.Reng: return rengPrefab;
            default: return null;
        }
    }

    private void InstantiateLog(LogInfo logInfo, float diameter, WoodProductType productType)
    {
        if (diameter == 0 || logInfo.logPrefabs == null) return;

        /*float count = diameter / 50;
        Vector3 spawnPoint;

        if (count > 1)
        {
            spawnPoint = new Vector3(
                spawnPosition.position.x + (0.7f * count),
                spawnPosition.position.y,
                spawnPosition.position.z
            );
        }
        else
        {
            spawnPoint = new Vector3(
                spawnPosition.position.x + 0.5f,
                spawnPosition.position.y,
                spawnPosition.position.z
            );
        }*/

        Quaternion spawnRotation = Quaternion.Euler(0, 90, 0);
        GameObject spawnedObject = Instantiate(logInfo.logPrefabs, spawnPosition.position, spawnRotation, this.gameObject.transform);

        CapsuleCollider capsule = spawnedObject.GetComponent<CapsuleCollider>();

        if(capsule != null)
        {
            AddClothColliderToAll(capsule);
        }
        // Set tipe produk pada log yang di-spawn
        LogInfoHolder holder = spawnedObject.GetComponent<LogInfoHolder>();
        if (holder != null)
        {
            holder.productType = productType;
        }
    }

    public void TriggerSpawnFromZone(WoodProductType productType)
    {
        if (!isSpawning && spawnQueue.Count > 0)
        {
            var request = spawnQueue.Dequeue();
            StartCoroutine(SpawnOutputRoutine(request));
        }
    }

    void AddClothColliderToAll(CapsuleCollider newCollider)
    {
        foreach (var cloth in clothList)
        {
            var currentColliders = cloth.capsuleColliders;
            var updatedColliders = new CapsuleCollider[currentColliders.Length + 1];

            for (int i = 0; i < currentColliders.Length; i++)
            {
                updatedColliders[i] = currentColliders[i];
            }

            updatedColliders[updatedColliders.Length - 1] = newCollider;
            cloth.capsuleColliders = updatedColliders;
        }
    }
}
