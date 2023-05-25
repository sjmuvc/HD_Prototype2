using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULDManager : MonoBehaviour
{
    public ULD[] ulds;
    public ULD currentULD;

    public float SCA_RealHeight;
    public float SCA_UnityHeight;
    public float ratio;

    private void Awake() 
    {
        currentULD.Initialize();
        SCA_RealHeight = 297.7f;
        SCA_UnityHeight = currentULD.transform.Find("").GetComponent<BoxCollider>().bounds.size.x; // uld분리 전 원본 높이 측정
        ratio = SCA_RealHeight / SCA_UnityHeight;
        ratio = 27f;
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
