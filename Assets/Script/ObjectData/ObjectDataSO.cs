using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ObjectData", menuName = "New Object Data")]
public class ObjectDataSO : ScriptableObject
{
    [Header("Object Properties")]
    public string objectName;
    public List<GameObject> outputPrefabs;


}
