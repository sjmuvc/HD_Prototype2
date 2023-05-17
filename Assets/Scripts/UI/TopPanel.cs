using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    public Button btn_Reload;
    public Button btn_Home;
    int zero = 0;

    private void Awake()
    {
        btn_Reload.onClick.AddListener(OnClickButton_Reload);
        btn_Home.onClick.AddListener(OnClickButton_Home);
    }

    void OnClickButton_Reload()
    {
        Cacher.uiManager.GetComponent<BottomPanel>().OnClickButton_SCA();
        Cacher.uiManager.GetComponent<BottomPanel>().inputField_Quantity.text = zero.ToString();
        Cacher.uiManager.GetComponent<BottomPanel>().OnClickButton_SpawnCargo();
        Cacher.uiManager.GetComponent<BottomPanel>().inputField_Quantity.text = null;
    }

    void OnClickButton_Home()
    {
        Cacher.uiManager.GetComponent<BottomPanel>().OnClickButton_SCA();
        Cacher.uiManager.GetComponent<BottomPanel>().inputField_Quantity.text = zero.ToString();
        Cacher.uiManager.GetComponent<BottomPanel>().OnClickButton_SpawnCargo();
        Cacher.uiManager.GetComponent<BottomPanel>().inputField_Quantity.text = null;
    }
}
