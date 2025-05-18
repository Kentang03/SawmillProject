using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWoodType", menuName = "Wood/Wood Type")]
public class WoodTypeSO : ScriptableObject
{
    [Header("Settings")]
    public string woodName;
    public GameObject woodPrefab;

    [Header("Checklist if Wood IsBent")]
    public bool isBent; 
}
