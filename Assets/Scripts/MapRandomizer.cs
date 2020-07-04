using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapRandomizer : MonoBehaviour
{
    #region Fields

    public Transform[] PointsOfInterest = new Transform[0];
    public string MapHierarchy = "Map";
    public float MapIndent = 2;
    public float ResetTime = 1;
    public bool ReleaseModal = true;
    public bool BigState = false;
    public Transform BargeTransform;
    public Transform RubbishGauge;
    public float ClosestToPoint = 0.25f;
    public Camera MainlineCamera;
    public float CameraSpeed = 0.1f;
    public PointOfInterest OverPoint;

    protected float MapExtentX;
    protected float MapExtentY;
    protected Transform MapPicture;
    protected FrameLocker TimeWaiter = new FrameLocker();
    protected Vector3 NewPosition = new Vector3();
    protected Vector3 NewBargePosition;
    protected float MapCameraMinY;
    protected float MapCameraMaxY;
    protected float MapCameraMinX;
    protected float MapCameraMaxX;
    protected bool TransportAwaits = false;

    #endregion


    #region UnityMethods

    // Start is called before the first frame update
    protected void Start()
    {
        MapPicture = transform.Find(MapHierarchy);

        if (MapPicture)
        {
            SpriteRenderer mapSprite = MapPicture.GetComponent<SpriteRenderer>();

            if (mapSprite)
            {
                MapExtentX = mapSprite.bounds.extents.x;
                MapExtentY = mapSprite.bounds.extents.y;
            }
        }

        SetCameraBounds();

//        Debug.Log(MapExtentX + " " + MapExtentY);

        TimeWaiter.LockSeconds = ResetTime;

        foreach (Transform trPoint in PointsOfInterest)
        {
            if (trPoint.GetComponent<PointOfInterest>())
            {
                trPoint.GetComponent<PointOfInterest>().ParentMapRandomizer = this;
            }
        }

        if (BargeTransform)
        {
            NewBargePosition = BargeTransform.position;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (TimeWaiter.CheckTime())
        {
            RandomizePoints();

            TimeWaiter.StartCountdown();
        }

        TimeWaiter.Countdown();

        if (BargeTransform)
        {
            BargeTransform.position = Vector3.Lerp(BargeTransform.position, NewBargePosition, Time.deltaTime);

            if (RubbishGauge.GetComponent<RubbishGauge>())
            {
                float closestDistanceToPoint = (NewBargePosition - BargeTransform.position).sqrMagnitude;

                if (closestDistanceToPoint > ClosestToPoint * ClosestToPoint)
                {
                    RubbishGauge.GetComponent<RubbishGauge>().EngageGauge = true;
                    TransportAwaits = true;
                }
                else
                {
                    RubbishGauge.GetComponent<RubbishGauge>().EngageGauge = false;

                    if (TransportAwaits)
                    {
                        LoadScene();

                        TransportAwaits = false;
                    }
                }
            }
        }


        MoveCamera();
    }

    protected void LoadScene()
    {
        if (OverPoint)
        {
            SceneManager.LoadScene(OverPoint.SceneToLoad);
        }
    }

    protected void MoveCamera()
    {
        if (MainlineCamera)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical") + Input.GetAxis("Horizontal")) > 0f)
            {
                Vector3 oldCameraPosition = MainlineCamera.transform.position;
                oldCameraPosition.y += Input.GetAxis("Vertical");
                oldCameraPosition.y = Mathf.Clamp(oldCameraPosition.y, MapCameraMinY, MapCameraMaxY);
                oldCameraPosition.x += Input.GetAxis("Horizontal");
                oldCameraPosition.x = Mathf.Clamp(oldCameraPosition.x, MapCameraMinX, MapCameraMaxX);
                MainlineCamera.transform.position = Vector3.Lerp(MainlineCamera.transform.position, oldCameraPosition, CameraSpeed);
            }
        }
    }

    protected void SetCameraBounds()
    {
        SpriteRenderer mapSprite = MapPicture.GetComponent<SpriteRenderer>();


        if (mapSprite && MainlineCamera)
        {
            Vector3 zeroVector3 = Vector3.zero;
            zeroVector3.z = mapSprite.transform.position.z - MainlineCamera.transform.position.z;
            Vector3 worldScreenMin = MainlineCamera.ScreenToWorldPoint(zeroVector3);
            zeroVector3.y = Screen.height;
            zeroVector3.x = Screen.width;
            Vector3 worldScreenMax = MainlineCamera.ScreenToWorldPoint(zeroVector3);

            MapCameraMinY = (mapSprite.bounds.min.y - worldScreenMin.y) + MainlineCamera.transform.position.y;
            MapCameraMaxY = (mapSprite.bounds.max.y - worldScreenMax.y) + MainlineCamera.transform.position.y;
            MapCameraMaxX = (mapSprite.bounds.max.x - worldScreenMax.x) + MainlineCamera.transform.position.x;
            MapCameraMinX = (mapSprite.bounds.min.x - worldScreenMin.x) + MainlineCamera.transform.position.x;
        }
    }

    #endregion

    protected void RandomizePoints()
    {
        if (PointsOfInterest.Length > 0)
        {
            for (int i = 0; i < PointsOfInterest.Length; i++)
            {
                NewPosition.z = PointsOfInterest[i].position.z;
                NewPosition.y = (Random.value * MapExtentY - MapIndent) * RandomSign();
                NewPosition.x = (Random.value * MapExtentX - MapIndent) * RandomSign();

                PointsOfInterest[i].position = NewPosition;
            }
        }
    }

    protected int RandomSign() => Random.value > 0.5f ? -1 : 1;

    public void MoveBargeToPosition(Vector3 newPosition) => NewBargePosition = newPosition;
}
