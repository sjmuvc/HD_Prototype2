using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CloudPanel : MonoBehaviour
{
    public float volume;
    public float weight;
    public string priority;

    public TMP_Text volume_txt;
    public TMP_Text weight_txt;
    public TMP_Text priority_txt_1;
    public TMP_Text priority_txt_2;

    public GameObject cloudImage;
    float cloudHeight = 1.5f;


    public void ShowData(CargoInfo cargoInfo, bool active)
    {
        if(Cacher.cargoManager.dragObject == null)
        {
            if (active)
            {
                volume = cargoInfo.volume_water;
                weight = cargoInfo.weight;
                priority = cargoInfo.priority;
                string[] priorityText = priority.Split("]");

                volume_txt.text = ("Wavolumeter: ") + volume.ToString() + ("m³");
                weight_txt.text = ("Weight: ") + weight.ToString() + ("kg");
                if (priorityText.Length == 1)
                {
                    priority_txt_1.text = ("Priority: ");
                    priority_txt_2.text = ("None");
                }
                else if (priorityText.Length == 2)
                {
                    priority_txt_1.text = ("Priority: ") + priorityText[0] + ("]");
                    priority_txt_2.text = priorityText[1];
                }
                cloudImage.transform.position = Cacher.uiManager.mainCamera.WorldToScreenPoint(cargoInfo.GetComponent<Cargo>().Objectpivot.transform.position + new Vector3(0, cloudHeight, 0));
            }
            cloudImage.SetActive(active);
        }
        else
        {
            cloudImage.SetActive(false);
        }
    } 
}
