using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Camera mainCamera;

    TopPanel topPanel;
    ULDInfoPanel ULDInfoPanel;
    public BottomPanel bottomPanel;
    public CloudPanel cloudPanel;
    public TMP_Text text;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Cacher.cargoManager.dragObject != null)
        {
            text.text = Cacher.cargoManager.dragObject.transform.GetChild(0).GetComponent<Cargo>().abovePlaneObject.transform.position.ToString();
            /*
            if (Cacher.cargoManager.dragObject.transform.GetChild(0).GetComponent<Cargo>().isOnVirtualPlane == true)
            {
                text.text = "true";
            }
            else
            {
                text.text = "false";
            }
            */
        } 
    }
}
