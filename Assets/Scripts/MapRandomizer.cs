using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
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

    protected float MapExtentX;
    protected float MapExtentY;
    protected Transform MapPicture;
    protected FrameLocker TimeWaiter = new FrameLocker();
    protected Vector3 NewPosition = new Vector3();
    protected Vector3 NewBargePosition;

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

        Debug.Log(MapExtentX + " " + MapExtentY);

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
