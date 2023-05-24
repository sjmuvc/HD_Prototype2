using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULDManager : MonoBehaviour
{
    public ULD[] ulds;
    public ULD currentULD;

    public float SCA_height;
    public float SCA_bounds_Z;
    public float ratio;

    private void Awake() 
    {
        currentULD.Initialize();
        SCA_height = 297.7f;
        SCA_bounds_Z = currentULD.transform.GetChild(2).Find("SCA_Type1_Top").transform.position.y;
        ratio = SCA_height / SCA_bounds_Z;
    }

    public void ResetULD()
    {
        Cacher.cargoManager.GotoCargoZoneAll();
        Cacher.uiManager.GetComponent<ULDInfoPanel>().Reset();
    }

    public void ChangeULD(int selectedULDNum)
    {
        Destroy(currentULD.gameObject);
        currentULD = Instantiate(ulds[selectedULDNum]); 
        currentULD.Initialize();
        currentULD.transform.position = new Vector3(2.55f, 0f, 19.4f);
        ResetULD();
    }
}
