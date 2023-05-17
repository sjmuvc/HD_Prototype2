using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULD : MonoBehaviour
{
    public string typeName;
    public float volume;

    public GameObject uld;
    public GameObject uld_Plane;
    public float uldHeight;

    GameObject virtualPlane;
    public float virtualPlaneHeight;
    public MeshRenderer virtualPlaneMeshRenderer;

    public void Initialize()
    {
        uld = this.gameObject;
        uld_Plane = this.transform.Find("ULD_Plane").gameObject;
        uldHeight = uld_Plane.transform.position.y;

        virtualPlane = this.transform.Find("ULD_VirtualPlane").gameObject;
        virtualPlaneMeshRenderer = virtualPlane.GetComponent<MeshRenderer>();
        virtualPlaneHeight = virtualPlane.transform.position.y;

        SetVolume();
    }

    void SetVolume()
    {
        if (this.gameObject.name == "SCA" || this.gameObject.name == "SCA(Clone)") 
        {
            volume = 19.5f;
        }
        else if (this.gameObject.name == "SCB" || this.gameObject.name == "SCB(Clone)")
        {
            volume = 17.7f;
        }
    }

    void ResetULD()
    {

    }
}
