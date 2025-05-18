using UnityEngine;

[System.Serializable]
public class CutRequest
{
    public string name;
    public Vector3 desiredSizeCM; // ukuran dalam cm
    public int quantityRequested;
}
