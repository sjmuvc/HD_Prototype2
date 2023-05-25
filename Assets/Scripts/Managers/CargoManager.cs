using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public Cargo[] cargos;

    public GameObject cargoZone;
    public GameObject cargoZonePlane;
    public GameObject dragObject;
    float cargoZoneLength_X;
    float cargoZoneLength_Z;
    public float currentCargoZoneLength_X;
    float currentCargoZoneLength_Z;
    float longestAxis_Z;
    float axisSpacing_Z;
    public GameObject abovePlaneObjects;

    public List<GameObject> cargoZoneObjects = new List<GameObject>();  
    public List<GameObject> uldObjects = new List<GameObject>();
    int cargoIndex;
    int remainCargoIndex;
    int currentGenerateCargo;

    private void Awake()
    {
        cargoZone = GameObject.Find("CargoZone");
        cargoZonePlane = GameObject.Find("CargoZonePlane");
        cargoZoneLength_X = cargoZonePlane.GetComponent<MeshCollider>().bounds.size.x;
        cargoZoneLength_Z = cargoZonePlane.GetComponent<MeshCollider>().bounds.size.z;
        abovePlaneObjects = GameObject.Find("AbovePlaneObjects"); 
    }

    public void CargoSizeSetting()
    {
        for (int i = 0; i < cargos.Length; i++)
        {
            Cargo cargoPrefab = Instantiate(cargos[i]);
            Debug.Log(cargoPrefab.GetComponent<MeshCollider>().bounds.size);
            cargoPrefab.transform.localScale = new Vector3(1, 1, 1);

            float cargoWidth = cargoPrefab.GetComponent<CargoInfo>().width;
            float cargoLength = cargoPrefab.GetComponent<CargoInfo>().length;
            float cargoHeight = cargoPrefab.GetComponent<CargoInfo>().height;
            

        }
    }

    public void GenerateCargo(int cargosQuantity)
    {
        for (int i = 0; i < uldObjects.Count; i++)
        {
            uldObjects[i].GetComponent<Cargo>().isUsingGeneratePos = true;
        }
        for (int i = 0; i < cargoZoneObjects.Count; i++)
        {
            Destroy(cargoZoneObjects[i].GetComponent<Cargo>().Objectpivot);
        }
        cargoZoneObjects.Clear();

        cargoIndex = 0;
        remainCargoIndex = 0;
        currentGenerateCargo = 0;
        currentCargoZoneLength_X = 0;
        currentCargoZoneLength_Z = 0;
        longestAxis_Z = 0;
        axisSpacing_Z = 0;

        // spawnRate만큼 갯수 생성
        for (int i = 0; i < cargos.Length; i++)
        {
            if (cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate == 0)
            {
                remainCargoIndex = cargoIndex;
            }
            for (int j = 0; j < Mathf.Floor((cargosQuantity * cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate)); j++) 
            {
                Cargo generatedCargo = Instantiate(cargos[cargoIndex], cargoZone.transform.Find("Objects").gameObject.transform);
                cargoZoneObjects.Add(generatedCargo.gameObject);
                currentGenerateCargo++;
                generatedCargo.GetComponent<Cargo>().GenerateSetting();
                GeneratePositioning(generatedCargo.gameObject);
                generatedCargo.startPosition = generatedCargo.Objectpivot.transform.localPosition;
                generatedCargo.startLocalEulerAngles = generatedCargo.Objectpivot.transform.localEulerAngles;
            }
            cargoIndex++;
        }

        // 부족한 갯수는 spawnRate가 0인 오브젝트로 채움
        for (int i = 0; i < cargosQuantity - currentGenerateCargo; i++) 
        {
            Cargo generatedCargo = Instantiate(cargos[remainCargoIndex], cargoZone.transform.Find("Objects").gameObject.transform);
            cargoZoneObjects.Add(generatedCargo.gameObject);
            generatedCargo.GetComponent<Cargo>().GenerateSetting();
            GeneratePositioning(generatedCargo.gameObject);
            generatedCargo.startPosition = generatedCargo.Objectpivot.transform.localPosition;
            generatedCargo.startLocalEulerAngles = generatedCargo.Objectpivot.transform.localEulerAngles;
        }
    }

    public void GeneratePositioning(GameObject addedCargo)
    {
        // X축 초과할 경우 Z축으로 이동
        if (currentCargoZoneLength_X + addedCargo.GetComponent<Cargo>().originBoundsX > cargoZoneLength_X)
        {
            // CargoZone의 오브젝트중 가장 긴 Z축의 값을 Z축 간격으로 설정
            for (int i = 0; i < cargoZoneObjects.Count; i++)
            {
                if (longestAxis_Z < cargoZoneObjects[i].gameObject.GetComponent<Cargo>().originBoundsZ)
                {
                    longestAxis_Z = cargoZoneObjects[i].gameObject.GetComponent<Cargo>().originBoundsZ;
                }
            }
            axisSpacing_Z += longestAxis_Z;
            currentCargoZoneLength_X = 0;
        }
        addedCargo.GetComponent<Cargo>().Objectpivot.transform.localPosition = new Vector3(currentCargoZoneLength_X + addedCargo.GetComponent<Cargo>().originBoundsX / 2, addedCargo.GetComponent<Cargo>().originBoundsY / 2, -axisSpacing_Z);
        currentCargoZoneLength_X += addedCargo.GetComponent<Cargo>().originBoundsX;
    }

    public void RemoveAtuldObjects()
    {
        if (uldObjects.Count - 1 >= 0)
        {
            uldObjects[uldObjects.Count - 1].GetComponent<Cargo>().GotoCargoZone();
            uldObjects.RemoveAt(uldObjects.Count - 1);
        }
    }

    public void GotoCargoZoneAll()
    {
        for(int i = 0; i < uldObjects.Count; i++)
        {
            uldObjects[i].GetComponent<Cargo>().GotoCargoZone();
            Cacher.cargoManager.cargoZoneObjects.Add(uldObjects[i]);  
        }
        uldObjects.Clear();
    }

    public void AllFreeze(bool freeze)
    {
        for (int i = 0; i < uldObjects.Count; i++)
        {
            if (freeze)
            {
                uldObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                uldObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
}
