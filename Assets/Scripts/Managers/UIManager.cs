using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Camera mainCamera;

    TopPanel topPanel;
    ULDInfoPanel ULDInfoPanel;
    public BottomPanel bottomPanel;
    public CloudPanel cloudPanel;
    

    private void Awake()
    {
        mainCamera = Camera.main;
    }
}
