using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavyBrig : MonoBehaviour
{
    public float Velocity = 2;
    public float PassCourseTiming = 5;
    public float YawAngle = 30;

    private Rigidbody _rb;
    private float _nextAdjust = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.GetComponent<Rigidbody>())
        {
           _rb = gameObject.AddComponent<Rigidbody>();
           _rb.velocity = transform.forward * Velocity;
           _rb.useGravity = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _nextAdjust)
        {
            transform.Rotate(new Vector3(0,  YawAngle, 0));

            _rb.velocity = transform.forward * Velocity;
            _nextAdjust = Time.time + Random.value * PassCourseTiming;
            
        }
    }

}
