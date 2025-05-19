using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogProcessor : MonoBehaviour
{
    [Header("List Semua Ukuran Produk")]
    public List<ProductSize> productSizeSOs;

    /// <summary>
    /// Proses log berdasarkan fraksi dan prioritas produk utama (Balok / Papan)
    /// </summary>
    public List<WoodProduct> ProcessLog(LogData log, float fraction, ProductType mainPreference)
    {
        float initialHeight = log.height * fraction;
        float availableHeight = initialHeight;
        float logLength = log.length;
        float logWidth = log.width;

        List<WoodProduct> result = new List<WoodProduct>();

        // 1. Prioritas utama
        var mainList = productSizeSOs
            .Where(p => p.type == mainPreference && p.widthCm <= logWidth)
            .OrderByDescending(p => p.heightCm);

        foreach (var size in mainList)
        {
            int count = Mathf.FloorToInt(availableHeight / size.heightCm);
            if (count > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"{size.type} {size.heightCm}x{size.widthCm} cm panjang {logLength}m",
                    quantity = count
                });

                availableHeight -= count * size.heightCm;
            }
        }

        // 2. Sisa → Usuk dan Reng
        var secondary = productSizeSOs
            .Where(p => (p.type == ProductType.Usuk || p.type == ProductType.Reng) && p.widthCm <= logWidth)
            .OrderByDescending(p => p.heightCm);

        foreach (var size in secondary)
        {
            int count = Mathf.FloorToInt(availableHeight / size.heightCm);
            if (count > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"{size.type} {size.heightCm}x{size.widthCm} cm panjang {logLength}m",
                    quantity = count
                });

                availableHeight -= count * size.heightCm;
            }
        }

        return result;
    }

    /// <summary>
    /// Proses berdasarkan satu atau dua SO target (dari dropdown user)
    /// </summary>
    public List<WoodProduct> ProcessWithDropdown(LogData log, ProductSize balok, ProductSize papan, float fraction)
    {
        float logLength = log.length;
        float logWidth = log.width;

        float availableHeightBalok = 0f;
        float availableHeightPapan = 0f;

        List<WoodProduct> result = new List<WoodProduct>();

        // Alokasi tinggi log
        float totalUsableHeight = log.height * fraction;

        bool hasBalok = balok != null;
        bool hasPapan = papan != null;

        if (hasBalok && hasPapan)
        {
            availableHeightBalok = totalUsableHeight * 0.6f;
            availableHeightPapan = totalUsableHeight * 0.4f;
        }
        else if (hasBalok)
        {
            availableHeightBalok = totalUsableHeight;
        }
        else if (hasPapan)
        {
            availableHeightPapan = totalUsableHeight;
        }

        // Proses Balok
        if (hasBalok && balok.widthCm <= logWidth)
        {
            int verticalCount = Mathf.FloorToInt(availableHeightBalok / balok.heightCm);
            int horizontalCount = Mathf.FloorToInt(logWidth / balok.widthCm);
            int totalCount = verticalCount * horizontalCount;

            if (totalCount > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"Balok {balok.heightCm}x{balok.widthCm} cm panjang {logLength}m",
                    quantity = totalCount
                });

                availableHeightBalok -= verticalCount * balok.heightCm;
            }
        }

        // Proses Papan
        if (hasPapan && papan.widthCm <= logWidth)
        {
            int verticalCount = Mathf.FloorToInt(availableHeightPapan / papan.heightCm);
            int horizontalCount = Mathf.FloorToInt(logWidth / papan.widthCm);
            int totalCount = verticalCount * horizontalCount;

            if (totalCount > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"Papan {papan.heightCm}x{papan.widthCm} cm panjang {logLength}m",
                    quantity = totalCount
                });

                availableHeightPapan -= verticalCount * papan.heightCm;
            }
        }

        // Sisa tinggi total bisa digunakan untuk Usuk dan Reng
        float remainingHeight = availableHeightBalok + availableHeightPapan;

        var secondary = productSizeSOs
            .Where(p => (p.type == ProductType.Usuk || p.type == ProductType.Reng) && p.widthCm <= logWidth)
            .OrderByDescending(p => p.heightCm);

        foreach (var size in secondary)
        {
            int verticalCount = Mathf.FloorToInt(remainingHeight / size.heightCm);
            int horizontalCount = Mathf.FloorToInt(logWidth / size.widthCm);
            int totalCount = verticalCount * horizontalCount;

            if (totalCount > 0)
            {
                result.Add(new WoodProduct
                {
                    productName = $"{size.type} {size.heightCm}x{size.widthCm} cm panjang {logLength}m",
                    quantity = totalCount
                });

                remainingHeight -= verticalCount * size.heightCm;
            }
        }

        return result;
    }




}
