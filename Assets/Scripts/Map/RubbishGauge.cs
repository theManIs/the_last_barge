using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishGauge : MonoBehaviour
{
    public RectTransform GaugeArrow;
    public RectTransform ArrowRotationPoint;
    public int AngleSpeed = 10;
    public int HighBarAngle = 360;
    public int LowBarAngle = 180;
    public bool EngageGauge = false;

    // Update is called once per frame
    void Update()
    {
//        Debug.Log(GaugeArrow.transform.rotation.eulerAngles);

        if (EngageGauge)
        {
            for (int i = 0; i < AngleSpeed; i++)
            {
                if (GaugeArrow.transform.rotation.eulerAngles.z > LowBarAngle && GaugeArrow.transform.rotation.eulerAngles.z < HighBarAngle)
                {
                    GaugeArrow.transform.Rotate(Vector3.forward, 1 * Time.deltaTime);
//                    GaugeArrow.transform.RotateAround(ArrowRotationPoint.transform.position, Vector3.forward, 1 * Time.deltaTime);
                }
            }
        }

    }
}
