using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoInfo : MonoBehaviour
{
    public string modelName;
    public float width;
    public float length;
    public float height;
    public float volume_water; // 적재율 계산에 사용
    public float volume_square;
    public float weight;
    public bool isStructed;
    public float spawnRate;
    public string priority;
}
