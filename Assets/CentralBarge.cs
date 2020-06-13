using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralBarge : MonoBehaviour
{
//    public float Velocity = 10;
//
    private Rigidbody _rb;
//
    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.GetComponent<Rigidbody>())
        {
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//        float verticalAxis = Input.GetAxis("Vertical");
//        float horizontalAxis = Input.GetAxis("Horizontal");
//
//        if ((Math.Abs(verticalAxis) + Math.Abs(horizontalAxis)) > 0f)
//        {
//            float signHorizontal = horizontalAxis.CompareTo(0f) == 0 ? 0 : horizontalAxis * Velocity;
//            float signVertical = verticalAxis.CompareTo(0f) == 0 ? 0 : verticalAxis * Velocity;
//            _rb.velocity = transform.forward * signVertical + transform.right * signHorizontal;
//        }
//        else
//        {
//            _rb.velocity = Vector3.zero;
//        }
//    }
}
