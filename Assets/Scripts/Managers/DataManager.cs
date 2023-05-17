using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Start()
    {
        
        List<Dictionary<string, object>> csv_Data = CSVReader.Read("LibraryExmaple");

        for (int i = 0; i < csv_Data.Count; i++)
        {
            for(int j = 0; j < csv_Data.Count; j++)
            {
                if (Cacher.cargoManager.cargos[i].name == csv_Data[j]["modelName"].ToString())
                {
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().width = float.Parse(csv_Data[j]["width"].ToString());
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().length = float.Parse(csv_Data[j]["length"].ToString());
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().height = float.Parse(csv_Data[j]["height"].ToString());
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().volume_water = float.Parse(csv_Data[j]["volume_water"].ToString());
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().volume_square = float.Parse(csv_Data[j]["volume_square"].ToString());
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().weight = float.Parse(csv_Data[j]["weight"].ToString());
                    if (csv_Data[j]["isStructed"].ToString() == "o")
                    {
                        Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().isStructed = true;
                    }
                    else
                    {
                        Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().isStructed = false;
                    }
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().spawnRate = float.Parse(csv_Data[j]["spawnRate"].ToString());
                    Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().priority = csv_Data[j]["priority"].ToString();
                }
            }
            
             
            
        }
        
    }

    public void Excel()
    {
        List<string> strings = new List<string>();

        //Cacher.cargoManager.GenerateCargo(strings);


    }
    /*
    public CargoInfo[] ParseCargoDB()
    {
        return 
    }
    */
}
