using UnityEngine;

[CreateAssetMenu(fileName = "LogInfo", menuName = "New Log Info")]
public class LogInfo : ScriptableObject
{
    public float length = 400f; // default 400 cm (4 meter)
    public float diameter;
    public GameObject logPrefabs;
}
