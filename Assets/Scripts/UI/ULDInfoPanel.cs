using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ULDInfoPanel : MonoBehaviour
{
    public float loadedCapacity;
    float total_Volume_water;
    public float loadedWeight;

    public TMP_Text loadedCapacity_txt;
    public TMP_Text loadedWeight_txt;

    public void AddCargo(CargoInfo cargoInfo)
    {
        total_Volume_water += cargoInfo.volume_water;
        loadedCapacity = total_Volume_water / Cacher.uldManager.currentULD.volume * 100;
        loadedWeight += cargoInfo.weight;
        loadedCapacity_txt.text = ("적재율: ") + Math.Round(loadedCapacity, 2).ToString() + ("%");
        loadedWeight_txt.text = ("총 중량: ") + loadedWeight.ToString() + ("kg");
    }

    public void SubCargo(CargoInfo cargoInfo)
    {
        total_Volume_water -= cargoInfo.volume_water;
        loadedCapacity = total_Volume_water / Cacher.uldManager.currentULD.volume * 100;
        loadedWeight -= cargoInfo.weight;
        loadedCapacity_txt.text = ("적재율: ") + Math.Round(loadedCapacity, 2).ToString() + ("%");
        loadedWeight_txt.text = ("총 중량: ") + loadedWeight.ToString() + ("kg");
    }

    public void Reset()
    {
        total_Volume_water = 0;
        loadedCapacity = total_Volume_water / Cacher.uldManager.currentULD.volume * 100;
        loadedWeight = 0;
        loadedCapacity_txt.text = ("적재율: ") + Math.Round(loadedCapacity, 2).ToString() + ("%");
        loadedWeight_txt.text = ("총 중량: ") + loadedWeight.ToString() + ("kg");
    }
}
