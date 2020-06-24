using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constructions.ConstructionCrane;
using UnityEngine;

public class ConstructionCrane : MonoBehaviour
{
    public Camera WorldCamera;
    public Transform[] AvailableBuildings;
    public string TargetObjectName = "RaiderHub";
    public bool LockedThing = false;
    public Transform CurrentBuilding;
    public Material ConstructionMaterial;
    public Material BadConstructionMaterial;
    public Transform CentralBarge;
    public ResourceStack ResourcesInStoke;

    private int _frameLockerSoft = 0;
    private int _frameLockerHard = 25;
    private bool _canBuild = true;
    private ConstructionCraneModel _ccm;

    // Start is called before the first frame update
    void Start()
    {
        if (WorldCamera)
        {
            _ccm = new ConstructionCraneModel();
            _ccm.WorldCamera = WorldCamera;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && _frameLockerSoft < 0)
        {
            if (CastRayFromScreen(out RaycastHit hit) && hit.transform.name == TargetObjectName && !LockedThing)
            {
//                Debug.Log(hit.transform.name);

                if (AvailableBuildings.Length > 0)
                {
                    LockedThing = true;
                    CurrentBuilding = _ccm.SpawnBuilding(AvailableBuildings[0]);

                    ApplyLayer(CurrentBuilding, 2);
                }
//                SpawnBuilding();

                _frameLockerSoft = _frameLockerHard;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && LockedThing && _frameLockerSoft < 0)
        {
            BuildOne();

            _frameLockerSoft = _frameLockerHard;
        }

        ActualPosition();
        CanBeBuilt();

        _frameLockerSoft--;
    }

    protected void CanBeBuilt()
    {
        if (CurrentBuilding)
        {
            _canBuild = _ccm.CanBeBuilt(CurrentBuilding);
            _canBuild = !_ccm.DoesTouchTheBarge(CentralBarge, CurrentBuilding) && _canBuild;
            _canBuild = _ccm.DoesTouchBuildingZone(LayerMask.NameToLayer("BuildingZone")) && _canBuild;
            _canBuild = ResourcesInStoke && ResourcesInStoke.StackSize > 0 && _canBuild;
            
            if (!_canBuild && BadConstructionMaterial)
            {
                ApplyMaterial(CurrentBuilding, BadConstructionMaterial);
            }
            else if (ConstructionMaterial)
            {
                ApplyMaterial(CurrentBuilding, ConstructionMaterial);
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (CurrentBuilding)
        {
            Collider c = CurrentBuilding.GetComponent<Collider>();
            float sphereRadius = c.bounds.size.x / 2;
//            Gizmos.DrawSphere(c.bounds.center, c.bounds.size.x / 2);

            Vector3 topHalfCenter = c.bounds.center;
            topHalfCenter.y = c.bounds.max.y - sphereRadius;
            Gizmos.DrawSphere(topHalfCenter, sphereRadius);

            Vector3 bottomHalfCenter = c.bounds.center;
            bottomHalfCenter.y = c.bounds.min.y + sphereRadius;
            Gizmos.DrawSphere(bottomHalfCenter, sphereRadius);
        }
    }

    protected void BuildOne()
    {
        if (_canBuild)
        {
            if (CastRayFromScreen(out RaycastHit hit))
            {
                Collider buildingCollider = CurrentBuilding.GetComponent<Collider>();
                Vector3 mousePoint = hit.point;

                if (buildingCollider && ResourcesInStoke && ResourcesInStoke.StackSize > 0)
                {
                    mousePoint.y += buildingCollider.bounds.size.y / 2;

                    Instantiate(AvailableBuildings[0], mousePoint, Quaternion.identity);

                    Destroy(CurrentBuilding.gameObject);

                    CurrentBuilding = null;
                    LockedThing = false;

                    ResourcesInStoke.StackSize -= 1;
                }
            }
        }
    }

    protected void ActualPosition()
    {
        if (CurrentBuilding)
        {
            if (CastRayFromScreen(out RaycastHit hit))
            {
                Collider buildingCollider = CurrentBuilding.GetComponent<Collider>();
                Vector3 mousePoint = hit.point;

                if (buildingCollider)
                {
                    mousePoint.y += buildingCollider.bounds.size.y / 2;

                    CurrentBuilding.transform.position = mousePoint;
                }
            }
        }
    }

//    public void SpawnBuilding()
//    {
//        if (AvailableBuildings.Length > 0)
//        {
//            if (CastRayFromScreen(out RaycastHit hit))
//            {
//                CurrentBuilding = Instantiate(AvailableBuildings[0], hit.point, Quaternion.identity);
//
//                if (ConstructionMaterial)
//                {
//                    ApplyLayer(CurrentBuilding, 2);
//
//                    CurrentBuilding.GetComponent<Collider>().isTrigger = true;
//                }
//            }
//        }
//    }

    protected void ApplyLayer(Transform buildingTransform, int layerOrdinal)
    {
        buildingTransform.gameObject.layer = layerOrdinal;
        int childCount = buildingTransform.childCount;

        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                ApplyLayer(buildingTransform.GetChild(i), layerOrdinal);
            }
        }
    }

    protected void ApplyMaterial(Transform buildingTransform, Material toMaterial)
    {
        MeshRenderer meshRenderer = buildingTransform.GetComponent<MeshRenderer>();

        if (meshRenderer)
        {
            meshRenderer.material = toMaterial;
        }

        int childCount = buildingTransform.childCount;


        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                ApplyMaterial(buildingTransform.GetChild(i), toMaterial);
            }
        }
    }

    protected bool CastRayFromScreen(out RaycastHit hit)
    {
        Ray cameraRay = WorldCamera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(cameraRay, out hit, 1000f);
    }
}
