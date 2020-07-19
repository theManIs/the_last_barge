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

    private Vector2 _mousePosition;
    private FrameLocker _fl = new FrameLocker();
    private ConstructionCraneModel _ccm = new ConstructionCraneModel();
    private bool _buildingInProgress = false;
    private CursorMasterMono _cmm;
    private bool _repairInProgress = false;
    private bool _menuInProgress = false;
    private bool _dismantleInProgress = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (_menuInProgress && Input.GetKey(KeyCode.Mouse1) && _fl.CheckFrame())
        {
            DeactivateMenus();

            _fl.StartLock();
        } 
        else if (Input.GetKey(KeyCode.Mouse1) && !_repairInProgress && !CraneBridgeProxy.BuildingInProgress && _fl.CheckFrame()) 
        {
            if (_ccm.CastAnyRayFromScreen(Input.mousePosition, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("BuildingZone"))
                {
                    StructurePanel.position = Input.mousePosition;
                    CraneBridgeProxy.MousePosition = Input.mousePosition;
                    _menuInProgress = true;

                    StructurePanel.gameObject.SetActive(true);
                    _fl.StartLock();
                }
            }
        }

        if (_buildingInProgress && !CraneBridgeProxy.StartBuilding && !CraneBridgeProxy.BuildingInProgress)
        {
            _buildingInProgress = false;

            StructureInfo?.gameObject.SetActive(false);
        }

        ReleaseCursor();
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
                _buildingInProgress = true;
            }
        };
    }

    protected void DeactivateMenus()
    {
        StructurePanel?.gameObject.SetActive(false);
        StructureInfo?.gameObject.SetActive(false);

        _menuInProgress = false;
    }

    protected void HideStructureMenu() => StructurePanel?.gameObject.SetActive(false);

    protected void ReleaseCursor()
    {
        if (_repairInProgress && Input.GetKey(KeyCode.Mouse1))
        {
            _cmm?.UseDefault();
            _repairInProgress = false;
        } 
        else if (_dismantleInProgress && Input.GetKey(KeyCode.Mouse1))
        {
            _cmm?.UseDefault();
            _dismantleInProgress = false;
        } 
        else if (!_repairInProgress && !_dismantleInProgress)
        {
            _cmm?.UseDefault();
        }
    }

    public void ClickRepairButton()
    {
        HideStructureMenu();

        _cmm.UseRepair();

        _repairInProgress = true;
    }

    public void ClickDismantleButton()
    {
        HideStructureMenu();

        _cmm.UseDismantle();

        _dismantleInProgress = true;
    }
}
