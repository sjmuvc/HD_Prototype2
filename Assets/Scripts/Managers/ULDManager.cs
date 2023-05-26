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

        // uld 원본 높이로 비율 측정
        SCA_RealHeight = 297.7f;
        SCA_UnityHeight = currentULD.transform.Find("SCA_Type1_Full").GetComponent<BoxCollider>().bounds.size.y; 
        Destroy(currentULD.transform.Find("SCA_Type1_Full").gameObject);
        ratio = SCA_RealHeight / SCA_UnityHeight;
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
