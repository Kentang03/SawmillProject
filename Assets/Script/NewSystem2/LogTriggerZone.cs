using UnityEngine;

public class LogTriggerZone : MonoBehaviour
{
    public LogCuttingProcessor processor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            Debug.Log("Log masuk ke area sawmill.");

            Destroy(other.gameObject); // hancurkan log-nya

            if (processor != null)
            {
                
                processor.TriggerSpawnFromZone(other.gameObject.GetComponent<LogInfoHolder>().productType);
            }
        }
    }
}
