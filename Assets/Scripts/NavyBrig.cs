using System;
using System.Collections;
using System.Collections.Generic;
using ArchimedsLab;
using UnityEngine;
using Random = UnityEngine.Random;

public class NavyBrig : MonoBehaviour
{
    public float AngleVelocity = 1f;
    public float PassCourseTiming = 5;
    public float YawAngle = 15;
    public float EngineMaxRpm = 600;
    public float AccelerationStep = 20;
    public float MaxAngleShift = 60;
    public BillboardHealth HealthCanvas;
    public bool IsDead = false;

    private Rigidbody _rb;
    private float _nextAdjust = 0;
    private Animator _animator;
    private float _angle = 0;
    private float _engineRpm = 0;
    private float _throttle = 0;
    private int _healthLevel = 100;

    // Start is called before the first frame update
    void Start()
    {
//        _animator = GetComponent<Animator>();

        //        if (!gameObject.GetComponent<Rigidbody>())
        //        {
        //           _rb = gameObject.AddComponent<Rigidbody>();
        //           _rb.velocity = transform.forward * Velocity;
        //           _rb.useGravity = false;
        //        }
        
        /** Water interaction */
        if (!(_rb = GetComponent<Rigidbody>()))
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }

        if (!GetComponent<MeshCollider>())
        {
            MeshCollider mc = gameObject.AddComponent<MeshCollider>();
            mc.convex = true;
            mc.sharedMesh = BuoyancyMesh;
        }

        if (!_rb)
            _rb = GetComponent<Rigidbody>();

        S_centerOfMass = _rb.centerOfMass;
        _lastPosition = transform.position;
        _rb.mass = TotalMass;

        WaterCutter.CookCache(BuoyancyMesh, ref _triangles, ref worldBuffer, ref wetTris, ref dryTris);
        /** Water interaction */

        /** Health display */
        if (HealthCanvas)
        {
            HealthCanvas.SetHealth(_healthLevel);
        }
        /** Health display */

        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.IsDead)
        {
            if (Input.GetKey(KeyCode.A))
                RudderLeft();
            else if (Input.GetKey(KeyCode.D))
                RudderRight();

            if (Input.GetKey(KeyCode.W))
            {
                ThrottleUp();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                ThrottleDown();
            }

            //        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            //            Brake();
            //        }

            if (Time.time > _nextAdjust)
            {
                for (int i = 0; i < MaxAngleShift; i++)
                {
                    RudderRight();
                }

                //            transform.Rotate(new Vector3(0,  YawAngle, 0));
                //        
                //            _rb.velocity = transform.forward * Velocity;
                _nextAdjust = Time.time + Random.value * PassCourseTiming;

            }

            ThrottleUp();
            AddTorque();
            AngleDamping();
        }
        else
        {
            Brake();
            AngleDamping();
        }
    }

    protected void AddTorque()
    {
        _engineRpm = _throttle * EngineMaxRpm;
        _rb.AddForceAtPosition(Quaternion.Euler(0, _angle, 0) * transform.forward * _engineRpm, transform.position);
    }
    public void ThrottleUp()
    {
        _throttle += AccelerationStep * 0.001F;
        _throttle = Mathf.Clamp(_throttle, 0f, 1f);
    }

    public void ThrottleDown()
    {
        _throttle -= AccelerationStep * 0.001F;
        _throttle = Mathf.Clamp(_throttle, -1, 0);
    }

    public void Brake()
    {
        if (_throttle > 0)
        {
            _throttle -= AccelerationStep * 0.001F;
        }
        else
        {
            _throttle += AccelerationStep * 0.001F;
        }
    }

    public void RudderRight()
    {
        _angle -= AngleVelocity;
        _angle = Mathf.Clamp(_angle, -1f * YawAngle, YawAngle);
    }

    public void RudderLeft()
    {
        _angle += AngleVelocity;
        _angle = Mathf.Clamp(_angle, -1f * YawAngle, YawAngle);
    }

    public void AngleDamping()
    {
        _angle = Mathf.Lerp(_angle, 0.0F, 0.02F);
    }


    private void OnTriggerEnter()
    {
        _healthLevel -= 10;

        if (HealthCanvas)
        {
            HealthCanvas.TakeDamage(10);
        }

        if (_animator && _healthLevel < 0)
        {
            this.IsDead = true;

            _animator.SetBool("break", true);

            Invoke("DecommissionBrig", 1);
        }

////        Debug.Log(Math.Round(Random.value * 50));
//        if (Math.Round(Random.value * 50) == 5)
//            _animator.enabled = true;
    }

    protected void DecommissionBrig()
    {
        AnimatorClipInfo[] ac = _animator.GetCurrentAnimatorClipInfo(0);

        if (ac.Length > 0)
        {
            Destroy(gameObject, ac[0].clip.length);
        }
    }


    public int TotalMass = 70000;
    public Vector3 centerOfMassOffset = new Vector3(0F, 0F, 0F);
    Vector3 S_centerOfMass;
    public Mesh BuoyancyMesh;
    private Vector3 _lastPosition;


    /* These 4 arrays are cache array, preventing some operations to be done each frame. */
    tri[] _triangles;
    tri[] worldBuffer;
    tri[] wetTris;
    tri[] dryTris;
    //These two variables will store the number of valid triangles in each cache arrays. They are different from array.Length !
    uint nbrWet, nbrDry;

    WaterSurface.GetWaterHeight realist = delegate (Vector3 pos)
    {
        const float eps = 0.1f;
        return (OceanAdvanced.GetWaterHeight(pos + new Vector3(-eps, 0F, -eps))
              + OceanAdvanced.GetWaterHeight(pos + new Vector3(eps, 0F, -eps))
              + OceanAdvanced.GetWaterHeight(pos + new Vector3(0F, 0F, eps))) / 3F;
    };

    protected void FixedUpdate()
    {
    #if UNITY_EDITOR
        if (_rb.centerOfMass != S_centerOfMass + centerOfMassOffset)
                _rb.centerOfMass = S_centerOfMass + centerOfMassOffset;
    #endif

        Vector3 speedVector = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;

        if (_rb.IsSleeping())
            return;

        WaterCutter.CookMesh(transform.position, transform.rotation, ref _triangles, ref worldBuffer);

        WaterCutter.SplitMesh(worldBuffer, ref wetTris, ref dryTris, out nbrWet, out nbrDry, realist);
        Archimeds.ComputeAllForces(wetTris, dryTris, nbrWet, nbrDry, speedVector, _rb);
    }

#if UNITY_EDITOR
    //Some visualizations for this buoyancy script.
    protected void OnDrawGizmos()
    {
        if (_rb)
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(_rb.worldCenterOfMass, 0.25F);

            Gizmos.color = Color.blue;
            for (uint i = 0; i < nbrWet; i++)
            {
                Gizmos.DrawLine(wetTris[i].a, wetTris[i].b);
                Gizmos.DrawLine(wetTris[i].b, wetTris[i].c);
                Gizmos.DrawLine(wetTris[i].a, wetTris[i].c);
            }

            Gizmos.color = Color.yellow;
            for (uint i = 0; i < nbrDry; i++)
            {
                Gizmos.DrawLine(dryTris[i].a, dryTris[i].b);
                Gizmos.DrawLine(dryTris[i].b, dryTris[i].c);
                Gizmos.DrawLine(dryTris[i].a, dryTris[i].c);
            }
        }
    }
#endif
}
