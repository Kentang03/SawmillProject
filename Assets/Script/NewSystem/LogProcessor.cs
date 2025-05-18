using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogProcessor : MonoBehaviour
{
    public List<ProductSize> productSizes;

    public List<WoodProduct> ProcessLog(LogData log, float fraction, ProductType mainPreference)
    {
        float logVolume = CalculateLogVolume(log, fraction);
        List<WoodProduct> result = new List<WoodProduct>();

        var mainSizes = productSizes
            .Where(s => s.type == mainPreference)
            .OrderByDescending(s => GetVolumePerMeter(s));

        foreach (var size in mainSizes)
        {
            float productVol = GetVolumePerMeter(size);
            int count = Mathf.FloorToInt(logVolume / productVol);
            if (count > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"{size.type} {size.heightCm}x{size.widthCm} cm",
                    quantity = count
                });
                logVolume -= count * productVol;
            }
        }

        // Convert remaining volume to Usuk and Reng
        var secondaryTypes = productSizes
            .Where(s => s.type == ProductType.Usuk || s.type == ProductType.Reng)
            .OrderByDescending(s => GetVolumePerMeter(s));

        foreach (var size in secondaryTypes)
        {
            float productVol = GetVolumePerMeter(size);
            int count = Mathf.FloorToInt(logVolume / productVol);
            if (count > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"{size.type} {size.heightCm}x{size.widthCm} cm",
                    quantity = count
                });
                logVolume -= count * productVol;
            }
        }

        return result;
    }

    private float CalculateLogVolume(LogData log, float fraction)
    {
        float radius = log.diameter / 2f;
        return Mathf.PI * radius * radius * log.length * fraction; // volume in m³
    }

    private float GetVolumePerMeter(ProductSize size)
    {
        return (size.heightCm / 100f) * (size.widthCm / 100f) * 1f;
    }
}
