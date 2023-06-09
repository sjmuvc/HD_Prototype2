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

    public void InputRotate(GameObject objectPivot)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            objectPivot.transform.Rotate(0, rotateValue, 0, Space.World);
            ResetSimulation(objectPivot);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            objectPivot.transform.Rotate(rotateValue, 0, 0, Space.World);
            ResetSimulation(objectPivot);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            objectPivot.transform.Rotate(0, 0, rotateValue, Space.World);
            ResetSimulation(objectPivot);
        }
    }

    private void ResetSimulation(GameObject objectPivot)
    {
        objectPivot.transform.GetChild(0).GetComponent<Cargo>().Simulation(false);
        objectPivot.transform.GetChild(0).GetComponent<Cargo>().time = 0;
        objectPivot.transform.GetChild(0).GetComponent<Cargo>().SettingVirtualObjectTransform(objectPivot.transform.GetChild(0).GetComponent<Cargo>().thisPos);
    }
}
