using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float rotateValue = 90;

    private void Update()
    {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (Cacher.cargoManager.uldObjects.Count > 0)
                {
                    Cacher.cargoManager.cargoZoneObjects.Add(Cacher.cargoManager.uldObjects[Cacher.cargoManager.uldObjects.Count - 1]);
                    Cacher.uiManager.GetComponent<ULDInfoPanel>().SubCargo(Cacher.cargoManager.uldObjects[Cacher.cargoManager.uldObjects.Count - 1].GetComponent<CargoInfo>());
                }
                Cacher.cargoManager.RemoveAtuldObjects();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Cacher.uldManager.currentULD.uld.transform.localEulerAngles = new Vector3(Cacher.uldManager.currentULD.uld.transform.localEulerAngles.x, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.y + 10, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.z);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Cacher.uldManager.currentULD.uld.transform.localEulerAngles = new Vector3(Cacher.uldManager.currentULD.uld.transform.localEulerAngles.x, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.y - 10, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.z);
            }
        
        
    }

    public void InPutRotate(GameObject objectPivot)
    {
        Vector3 tmpVector = new Vector3();

        tmpVector = objectPivot.transform.localEulerAngles;

        if (Input.GetKeyDown(KeyCode.R))
        {
            objectPivot.transform.GetChild(0).GetComponent<Cargo>().objectHeight = objectPivot.transform.GetChild(0).GetComponent<Cargo>().objectHeightY;
            objectPivot.transform.Rotate(0, 90, 0, Space.World);
            ResetSimulation(objectPivot);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            objectPivot.transform.GetChild(0).GetComponent<Cargo>().objectHeight = objectPivot.transform.GetChild(0).GetComponent<Cargo>().objectHeightX;
            objectPivot.transform.Rotate(90, 0, 0, Space.World);
            ResetSimulation(objectPivot);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            objectPivot.transform.GetChild(0).GetComponent<Cargo>().objectHeight = objectPivot.transform.GetChild(0).GetComponent<Cargo>().objectHeightZ;
            objectPivot.transform.Rotate(0, 0,90, Space.World);
            ResetSimulation(objectPivot);
        }
        //objectPivot.transform.localEulerAngles = tmpVector; 
    }

    private void ResetSimulation(GameObject objectPivot)
    {
        objectPivot.transform.GetChild(0).GetComponent<Cargo>().Simulation(false);
        objectPivot.transform.GetChild(0).GetComponent<Cargo>().time = 0;
        objectPivot.transform.GetChild(0).GetComponent<Cargo>().SettingVirtualObjectTransform(objectPivot.transform.GetChild(0).GetComponent<Cargo>().thisPos);
    }
}
