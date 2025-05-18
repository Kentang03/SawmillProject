// Di trigger button, collider, atau interaksi lainnya
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    private SawmillConverter sawmill;
    
    private void Start()
    {
        sawmill = SawmillConverter.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            ObjectDataSO data = other.GetComponent<ObjectProperties>().dataObject;
            sawmill.StartConversion(data);
            ConveyorScript conveyor = SawmillConverter.Instance.conveyor;
            conveyor.onBelt.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
