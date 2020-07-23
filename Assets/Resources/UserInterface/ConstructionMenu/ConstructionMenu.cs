using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Constructions.ConstructionCrane;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConstructionMenu : MonoBehaviour
{
    #region Fields

    public RectTransform StructurePanel;
    public RectTransform StructureInfo;
    public int ComfortFramesBlock = 30;
    public Camera WorldCamera;
    public Button[] ConstructionsList;
    public CraneBridgeProxy CraneBridgeProxy;
    public Transform[] ConstructionPrefabs;
    public Button RepairButton;
    public Button DismantleButton;

    private FrameLocker _fl = new FrameLocker();
    private ConstructionCraneModel _ccm = new ConstructionCraneModel();
    private CursorMasterMono _cmm;
    private ConstructionFlagsCapsule _cfc;

    #endregion


    #region UnityMethods

    private void Start()
    {
        UnityStart();
    } 

    #endregion


    protected void UnityStart()
    {
        StructurePanel?.gameObject.SetActive(false);
        StructureInfo?.gameObject.SetActive(false);

        _fl.LockFrames = ComfortFramesBlock;
        _ccm.WorldCamera = WorldCamera;


        for (int i = 0; i < ConstructionsList.Length; i++)
        {
            ConstructionsList[i].onClick.AddListener(ClickTheButton(i));
        }

        RepairButton?.onClick.AddListener(ClickRepairButton);
        DismantleButton?.onClick.AddListener(ClickDismantleButton);

        _cmm = FindObjectOfType<CursorMasterMono>();
        _cfc = CraneBridgeProxy ? CraneBridgeProxy.Cfc : new ConstructionFlagsCapsule();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cfc.MenuInProgress && Input.GetKey(KeyCode.Mouse1) && _fl.CheckFrame())
        {
            HideStructureInfo();
            HideStructureMenu();

            _fl.StartLock();
        } 
        else if (Input.GetKey(KeyCode.Mouse1) && !_cfc.RepairInProgress && !CraneBridgeProxy.BuildingInProgress && _fl.CheckFrame()) 
        {
            if (_ccm.CastAnyRayFromScreen(Input.mousePosition, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("BuildingZone"))
                {
                    StructurePanel.position = Input.mousePosition;
                    CraneBridgeProxy.MousePosition = Input.mousePosition;
                    _cfc.MenuInProgress = true;

                    StructurePanel.gameObject.SetActive(true);
                    _fl.StartLock();
                }
            }
        }

        ReleaseAllocation();
        _fl.RefineFrame();
    }

    public UnityAction ClickTheButton(int number)
    {
        return () =>
        {
            HideStructureMenu();
            StructureInfo?.gameObject.SetActive(true);

            if (CraneBridgeProxy)
            {
                CraneBridgeProxy.BuildingNumber = number;
                CraneBridgeProxy.AvailableBuilding = ConstructionPrefabs[CraneBridgeProxy.BuildingNumber];
                CraneBridgeProxy.StartBuilding = true;
                _cfc.BuildingInProgress = true;
            }
        };
    }

    protected void HideStructureInfo()
    {
        _cfc.BuildingInProgress = false;

        StructureInfo?.gameObject.SetActive(false);
    }

    protected void HideStructureMenu()
    {
        _cfc.MenuInProgress = false;

        StructurePanel?.gameObject.SetActive(false);
    }

    protected void ReleaseAllocation()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (_cfc.RepairInProgress)
            {
                _cfc.RepairInProgress = false;
            }
            else if (_cfc.DismantleInProgress)
            {
                _cfc.DismantleInProgress = false;
            }
        }

        if (!_cfc.RepairInProgress && !_cfc.DismantleInProgress)
        {
            _cmm?.UseDefault();
        }
        else if (_cfc.DismantleInProgress)
        {
            _cmm?.UseDismantle();
        }
        else if (_cfc.RepairInProgress)
        {
            _cmm?.UseRepair();
        }

        if (_cfc.BuildingInProgress && !CraneBridgeProxy.StartBuilding && !CraneBridgeProxy.BuildingInProgress)
        {
            HideStructureInfo();
        }
    }

    public void ClickRepairButton()
    {
        HideStructureMenu();

        _cfc.RepairInProgress = true;
    }

    public void ClickDismantleButton()
    {
        HideStructureMenu();

        _cfc.DismantleInProgress = true;
    }
}
