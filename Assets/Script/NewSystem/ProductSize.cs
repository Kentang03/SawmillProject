using UnityEngine;

public enum ProductType
{
    Balok, Papan, Usuk, Reng
}

[CreateAssetMenu(fileName = "ProductSize", menuName = "Sawmill/Product Size")]
public class ProductSize : ScriptableObject
{
    public ProductType type;
    public float heightCm;
    public float widthCm;
    public GameObject prefab; // Optional: untuk visualisasi
}
