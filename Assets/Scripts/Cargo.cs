using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cargo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    GameManager gameManager;
    RaycastHit hitLayerMask;
    public Vector3 thisPos;
    GameObject virtualObject;
    GameObject abovePlaneObject;
    public float objectHeight;
    public float objectHeightX;
    public float objectHeightY;
    public float objectHeightZ;
    Vector3 pivot;
    public GameObject Objectpivot;
    Vector3 settingPivotPosition;
    Vector3 settingPivotRotation;
    Rigidbody rigidBody;
    LineRenderer lineRenderer;
    Material virtualObjectOriginMat;
    MeshCollider meshCollider;
    public Vector3 startPosition;
    public Vector3 startLocalEulerAngles;
    Vector3 virtualObjectPos;

    float cameraToObjectDistance = 20;
    float mouseRayDistance = 1000;
    bool isOnVirtualPlane = false;
    float currentStackHeight;
    public bool isInsideTheWall = true;
    float lineWidth = .05f;
    public float time;
    float simulationTime;
    Vector3 currentPos, lastPos;
    float delayTimeToSimulation = 0.5f;
    float replayTimeToSimulation = 2.5f;
    bool isSimulationOn;
    bool isEnableStack;
    int layerName;
    public bool isPreviousCargo = false;

    public void GenerateSetting()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.gameObject.tag = "StackObject";
        for (int i = 0; i < this.transform.childCount; i++) 
        {
            this.gameObject.transform.GetChild(i).tag = "StackObject";
        }
        layerName = LayerMask.NameToLayer("Cargo");
        this.gameObject.layer = layerName;

        AddComponenet();

        // 부모 오브젝트 생성후 올바른 피봇 지정
        pivot = GetComponent<MeshCollider>().bounds.center;
        Objectpivot = new GameObject();
        Objectpivot.transform.position = pivot;
        this.transform.parent = Objectpivot.transform;
        settingPivotPosition = this.transform.localPosition;
        settingPivotRotation = this.transform.localEulerAngles;
        Objectpivot.name = this.gameObject.name;

        // 피봇 위치를 맞춘 가상 오브젝트 생성하고 false
        virtualObject = Instantiate(Objectpivot, Objectpivot.transform);
        Destroy(virtualObject.GetComponentInChildren<Cargo>());
        Destroy(virtualObject.GetComponentInChildren<LineRenderer>());
        virtualObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().convex = true;
        virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
        virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
        virtualObjectOriginMat = gameManager.greenMaterial;
        virtualObject.transform.GetChild(0).gameObject.tag = "Untagged";
        for (int i = 0; i < virtualObject.transform.GetChild(0).transform.childCount; i++)
        {
            virtualObject.transform.GetChild(0).transform.GetChild(i).tag = "Untagged";
        }
        virtualObject.transform.GetChild(0).gameObject.AddComponent<VirtualObjectTrigger>();
        virtualObject.transform.GetChild(0).gameObject.GetComponent<VirtualObjectTrigger>().cargoManager = this.GetComponent<Cargo>();
        virtualObject.SetActive(false);

        abovePlaneObject = Instantiate(Objectpivot, Objectpivot.transform);
        Destroy(abovePlaneObject.transform.GetChild(0).GetComponentInChildren<Cargo>());
        Destroy(abovePlaneObject.transform.GetChild(0).GetComponentInChildren<CargoInfo>());
        Destroy(abovePlaneObject.transform.GetChild(0).GetComponentInChildren<LineRenderer>());

        abovePlaneObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().convex = true;
        abovePlaneObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
        abovePlaneObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
        abovePlaneObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < abovePlaneObject.transform.GetChild(0).transform.childCount; i++) 
        {
            abovePlaneObject.transform.GetChild(0).transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        abovePlaneObject.transform.GetChild(0).gameObject.tag = "Untagged";
        Destroy(abovePlaneObject.transform.GetChild(1).gameObject);

        SettingObjectTransform();
        Objectpivot.transform.parent = Cacher.cargoManager.cargoZone.transform.Find("Objects").gameObject.transform;
    }

    void AddComponenet()
    {
        // Mesh Collider 생성
        if (this.gameObject.GetComponent<MeshCollider>() == null)
        {
            this.gameObject.AddComponent<MeshCollider>();
        }
        meshCollider = this.GetComponent<MeshCollider>();
        meshCollider.convex = true;
        objectHeightX = meshCollider.bounds.size.x;
        objectHeightY = meshCollider.bounds.size.y;
        objectHeightZ = meshCollider.bounds.size.z; // extents로 할 경우 x,y,z축과 상관없는 오브젝트의 높이, 하지만 높이가 안맞음

        objectHeight = objectHeightY;

        // rigidBody 생성
        this.gameObject.AddComponent<Rigidbody>();
        rigidBody = this.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = true;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // 라인 렌더러 생성
        this.gameObject.AddComponent<LineRenderer>();
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.material = gameManager.lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Cacher.uiManager.mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraToObjectDistance)); // 카메라로부터 거리값
        Cacher.cargoManager.AllFreeze(true);
        Cacher.inputManager.InPutRotate(Objectpivot);
        RayPositioning(worldMousePos);
        Debug.Log(currentStackHeight);
    }

    void RayPositioning(Vector3 worldMousePos)
    {
        Ray ray = Cacher.uiManager.mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * mouseRayDistance, Color.red);

        int layerMask = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer가 Virtual Plane인 것만 검출
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask)) // layerMask에 닿은 RaycastHit 반환
        {
            isOnVirtualPlane = true;

            #region VirtualPlane위의 높이 잡아주기 
            //Objectpivot.transform.position = new Vector3(hitLayerMask.point.x, Cacher.uldManager.currentULD.virtualPlaneHeight * 2, hitLayerMask.point.z); // 객체의 위치를 RaycastHit의 point값 위치로 이동

            abovePlaneObject.transform.position = new Vector3(hitLayerMask.point.x, Cacher.uldManager.currentULD.virtualPlaneHeight * 2, hitLayerMask.point.z);

            RaycastHit[] sweepTestHitAll;

            sweepTestHitAll = abovePlaneObject.transform.GetChild(0).GetComponent<Rigidbody>().SweepTestAll(new Vector3(0, -1, 0), Cacher.uldManager.currentULD.virtualPlaneHeight * 2, QueryTriggerInteraction.Ignore);
            if (sweepTestHitAll.Length == 0)
            {
                return;
            }

            RaycastHit sweepTestHitSelected = sweepTestHitAll[0];

            foreach (RaycastHit sweepTestHit in sweepTestHitAll)
            {
                if (sweepTestHit.collider.tag == "VirtualPlane")
                {
                    
                    if (sweepTestHitSelected.distance > sweepTestHit.distance || sweepTestHitSelected.collider.tag != "VirtualPlane")
                    {
                        sweepTestHitSelected = sweepTestHit;
                    }
                    
                    float height = abovePlaneObject.transform.position.y - (sweepTestHitSelected.distance);
                    Objectpivot.transform.position = new Vector3(hitLayerMask.point.x, height, hitLayerMask.point.z);
                }
            }
            
            #endregion

            DetectStackHeight();
            DrawVirtualObject(isOnVirtualPlane);
            Cacher.uldManager.currentULD.virtualPlaneMeshRenderer.enabled = false;
        }
        else
        {
            Objectpivot.transform.position = worldMousePos;
            isOnVirtualPlane = false;
            DrawVirtualObject(isOnVirtualPlane);
            Cacher.uldManager.currentULD.virtualPlaneMeshRenderer.enabled = true;
        }
    }
    private void OnMouseUp()
    {
        Cacher.cargoManager.dragObject = null;
        lineRenderer.enabled = false;
        virtualObject.SetActive(false);
        Cacher.uldManager.currentULD.virtualPlaneMeshRenderer.enabled = true;

        if (isOnVirtualPlane && isEnableStack)
        {
            Objectpivot.transform.position = virtualObjectPos;
            rigidBody.isKinematic = false;
            Cacher.cargoManager.uldObjects.Add(this.gameObject);
            Cacher.cargoManager.cargoZoneObjects.Remove(this.gameObject);
            Cacher.uiManager.GetComponent<ULDInfoPanel>().AddCargo(GetComponent<CargoInfo>());
        }
        else
        {
            GotoCargoZone();
        }
        Simulation(false);
        Cacher.cargoManager.AllFreeze(false);
    }

    private void OnMouseDown()
    {
        Objectpivot.transform.parent = Cacher.uldManager.currentULD.uld.transform.Find("Objects").gameObject.transform;
        Cacher.cargoManager.dragObject = Objectpivot;
        SettingObjectTransform();
        rigidBody.isKinematic = true;
    }

    void DrawVirtualObject(bool active)
    {
        thisPos = Objectpivot.GetComponent<Transform>().position;

        if (active)
        {
            virtualObject.SetActive(active);
            virtualObjectPos = new Vector3(Objectpivot.transform.position.x, currentStackHeight, Objectpivot.transform.position.z);
            virtualObject.transform.position = virtualObjectPos;
        }
        else
        {
            virtualObject.SetActive(active);
        }

        lineRenderer.SetPosition(0, thisPos);
        lineRenderer.SetPosition(1, virtualObjectPos);
        lineRenderer.enabled = active;

        #region 시뮬레이션 기능
        currentPos = Objectpivot.transform.position; // currentPos 계속 업데이트
        StartCoroutine(LastPosSetting(currentPos));  // lastPos는 딜레이를 두고 업데이트

        time += Time.deltaTime;

        IEnumerator LastPosSetting(Vector3 currentPos)
        {
            yield return new WaitForSeconds(0.1f);
            lastPos = currentPos;
        }

        if (time > delayTimeToSimulation && currentPos == lastPos) // 1초 이상 움직임 없으면 시뮬레이션 시작
        {
            Simulation(true);

            simulationTime += Time.deltaTime;
            if (simulationTime > replayTimeToSimulation)
            {
                SettingVirtualObjectTransform(thisPos);
            }
        }
        else if (currentPos != lastPos) // 움직이면 시뮬레이션 종료
        {
            Simulation(false);

            time = 0;
            SettingVirtualObjectTransform(thisPos);
        }
        #endregion

        // 벽 안쪽이어야만 내려놓을 수 있음
        if (isInsideTheWall == true && Cacher.uldManager.currentULD.virtualPlaneHeight > currentStackHeight + objectHeight)
        {
            EnableStack(true);
        }
        else
        {
            EnableStack(false);
        }
    }

    void DetectStackHeight()
    {
        RaycastHit[] sweepTestHitAll;

        sweepTestHitAll = rigidBody.SweepTestAll(new Vector3(0, -1, 0), Cacher.uldManager.currentULD.virtualPlaneHeight * 2, QueryTriggerInteraction.Ignore);
        if (sweepTestHitAll.Length == 0)
        {
            return;
        }

        RaycastHit sweepTestHitSelected = sweepTestHitAll[0];

        foreach (RaycastHit sweepTestHit in sweepTestHitAll)
        {
            if (sweepTestHit.collider.tag == "StackObject")
            {
                if (sweepTestHitSelected.distance > sweepTestHit.distance || sweepTestHitSelected.collider.tag != "StackObject")
                {
                    sweepTestHitSelected = sweepTestHit;
                }
                float rayHeight = Objectpivot.transform.position.y - (sweepTestHitSelected.distance);
                currentStackHeight = rayHeight;
            }
        }
    }
    void EnableStack(bool enable)
    {
        if (!isSimulationOn)
        {
            if (enable)
            {
                //virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = virtualObjectOriginMat;
                foreach (var item in virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials)
                {
                    item.color = Color.green;
                }
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    foreach (var item in virtualObject.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().materials)
                    {
                        item.color = Color.green;
                    }
                }
                isEnableStack = true;
            }
            else
            {
                //virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = gameManager.redMaterial;
                foreach (var item in virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials)
                {
                    item.color = Color.red;
                }
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    foreach (var item in virtualObject.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().materials)
                    {
                        item.color = Color.red;
                    }
                }
                isEnableStack = false;
            }
        }
    }

    public void GotoCargoZone()
    {
        SettingObjectTransform();
        Objectpivot.transform.parent = Cacher.cargoManager.cargoZone.transform.Find("Objects").gameObject.transform;
        // uld 안에 있었을 경우 CargoZonePositioning 방식 적용
        if (Cacher.cargoManager.uldObjects.Contains(this.gameObject) || isPreviousCargo == true)
        {
            Cacher.cargoManager.CargoZonePositioning(this.gameObject);
        }
        else
        {
            Objectpivot.transform.localPosition = startPosition;
            Objectpivot.transform.localEulerAngles = startLocalEulerAngles;
        }

    }

    void SettingObjectTransform()
    {
        this.gameObject.transform.localPosition = settingPivotPosition;
        this.gameObject.transform.localEulerAngles = settingPivotRotation;
    }

    public void SettingVirtualObjectTransform(Vector3 thisPos)
    {
        virtualObject.transform.position = new Vector3(thisPos.x, currentStackHeight, thisPos.z);
        virtualObject.transform.GetChild(0).gameObject.transform.localPosition = settingPivotPosition;
        virtualObject.transform.GetChild(0).gameObject.transform.localEulerAngles = settingPivotRotation;
        simulationTime = 0;
    }

    public void Simulation(bool active)
    {
        if (active)
        {
            virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isSimulationOn = active;
        }
        else
        {
            virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isSimulationOn = active;
        }
    }

    public void SetObjectHeight()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cacher.uiManager.cloudPanel.ShowData(GetComponent<CargoInfo>(), true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cacher.uiManager.cloudPanel.ShowData(GetComponent<CargoInfo>(), false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Cacher.uiManager.cloudPanel.ShowData(GetComponent<CargoInfo>(), false);
    }
}

