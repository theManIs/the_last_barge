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
    public RectTransform StructurePanel;
    public int ComfortFramesBlock = 30;
    public Camera WorldCamera;
    public Button[] ConstructionsList;
    public CraneBridgeProxy CraneBridgeProxy;
    public Transform[] ConstructionPrefabs;

    private Vector2 _mousePosition;
    private FrameLocker _fl = new FrameLocker();
    private ConstructionCraneModel _ccm = new ConstructionCraneModel();

    // Start is called before the first frame update
    void Start()
    {
        if (StructurePanel)
        {
            StructurePanel.gameObject.SetActive(false);
        }

        _fl.LockFrames = ComfortFramesBlock;
        _ccm.WorldCamera = WorldCamera;


        for (int i = 0; i < ConstructionsList.Length; i++)
        {
            ConstructionsList[i].onClick.AddListener(ClickTheButton(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
//        if (Input.GetKey(KeyCode.Mouse0) && StructurePanel.gameObject.activeSelf && _fl.CheckFrame())
//        {
//            StructurePanel.gameObject.SetActive(false);
//            _fl.StartLock();
//        }
//        else 
        if (Input.GetKey(KeyCode.Mouse1) && StructurePanel && !StructurePanel.gameObject.activeSelf && _fl.CheckFrame()) 
        {
            if (_ccm.CastAnyRayFromScreen(Input.mousePosition, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("BuildingZone"))
                {
                    StructurePanel.position = Input.mousePosition;
                    CraneBridgeProxy.MousePosition = Input.mousePosition;

                    StructurePanel.gameObject.SetActive(true);
                    _fl.StartLock();
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse1) && _fl.CheckFrame())
        {
            StructurePanel.gameObject.SetActive(false);
            _fl.StartLock();
        }

        _fl.RefineFrame();
    }

    public UnityAction ClickTheButton(int number)
    {
        return () =>
        {
            StructurePanel.gameObject.SetActive(false);

            if (CraneBridgeProxy)
            {
                CraneBridgeProxy.BuildingNumber = number;
                CraneBridgeProxy.AvailableBuilding = ConstructionPrefabs[CraneBridgeProxy.BuildingNumber];
                CraneBridgeProxy.StartBuilding = true;
            }
        };
    }
}
