using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReaderTest : MonoBehaviour
{
    private void Start()
    {
        List<Dictionary<string, object>> csv_Data = CSVReader.Read("LibraryExmaple");

        for (int i = 0; i < csv_Data.Count; i++)
        {
            print(csv_Data[i]["weight"].ToString());
        }
    }
}

