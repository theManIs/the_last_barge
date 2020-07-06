using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PointOfInterest : MonoBehaviour
{
    public Vector3 RescaleMod = Vector3.one;
    public Transform PointDescription;
    public Vector3 ImageExtents = Vector3.zero;
    public Transform ConfirmWindow;
    public MapRandomizer ParentMapRandomizer;

    [Header("GameLook")]
    public string SceneToLoad = "Scenes/BuildingScene";
    public Texture InfoImage;

    protected Vector3 NewScale = Vector3.one;
    protected Transform PointDescrObject;
    protected Transform ConfirmWindowObject;
    protected InfoPanelProxy InfoPanelProxy;

    protected void Start()
    {
        InfoPanelProxy = PointDescription.GetComponent<InfoPanelProxy>();
    }

//    protected void Update()
//    {
//        if (Input.GetKey(KeyCode.Mouse1) && (!ParentMapRandomizer.ReleaseModal || ParentMapRandomizer.BigState))
//        if (Input.GetKey(KeyCode.Mouse1) )
//        {
//            Vector3 ls = transform.localScale;
//            NewScale.x = ls.x / RescaleMod.x;
//            NewScale.y = ls.y / RescaleMod.y;
//            NewScale.z = ls.z / RescaleMod.z;
//            transform.localScale = NewScale;
//            
//            PointDescription.gameObject.SetActive(false);
//
//            ParentMapRandomizer.BigState = false;
//        }
//    }

    protected void OnMouseEnter()
    {
        if (ParentMapRandomizer && ParentMapRandomizer.ReleaseModal)
        {
            Vector3 ls = transform.localScale;

            NewScale = ls * RescaleMod.x;
//            NewScale.x = ls.x * RescaleMod.x;
//            NewScale.y = ls.y * RescaleMod.y;
//            NewScale.z = ls.z * RescaleMod.z;

            transform.localScale = NewScale;

            PointDescription.gameObject.SetActive(true);

            ParentMapRandomizer.BigState = true;
            ParentMapRandomizer.HoverPoint = this;

            DisplayDescription();
        }
    }

    protected void DisplayDescription()
    {
        InfoPanelProxy?.SetDifficulty((int)(Random.value * 5));
        InfoPanelProxy?.SetReward((int)(Random.value * 4));
        InfoPanelProxy?.SetInfoImage(InfoImage);
    }

    public void OnMouseExit()
    {
        if (ParentMapRandomizer && ParentMapRandomizer.BigState && ParentMapRandomizer.ReleaseModal)
        {
            Vector3 ls = transform.localScale;
            NewScale.x = ls.x / RescaleMod.x;
            NewScale.y = ls.y / RescaleMod.y;
            NewScale.z = ls.z / RescaleMod.z;

            transform.localScale = NewScale;
            
            PointDescription.gameObject.SetActive(false);

            ParentMapRandomizer.BigState = false;
            ParentMapRandomizer.HoverPoint = null;
        }
    }

    void OnMouseDown()
    {
        if (ParentMapRandomizer && ParentMapRandomizer && ParentMapRandomizer.ReleaseModal)
        {
            ConfirmWindow.gameObject.SetActive(true);

            ParentMapRandomizer.ReleaseModal = false;
            ParentMapRandomizer.OverPoint = this;

//            Transform confirmWindowObject = PointDescription.transform.Find("YesNoPanel/Yes");

//            confirmWindowObject.gameObject.GetComponent<Button>().onClick.AddListener(OnClickConfirmButton);

            foreach (Button butYesNo in ConfirmWindow.gameObject.GetComponentsInChildren<Button>())
            {
                butYesNo.onClick.AddListener(OnClickConfirmButton);
            }

        }
    }

    public void OnClickConfirmButton()
    {
        if (ParentMapRandomizer)
        {
            OnMouseExit();
            ParentMapRandomizer.MoveBargeToPosition(transform.position);
        }

        DeactivateConfirm();
    }

    public void DeactivateConfirm()
    {
        ParentMapRandomizer.ReleaseModal = true;
        ConfirmWindow.gameObject.SetActive(false);
    }

}
