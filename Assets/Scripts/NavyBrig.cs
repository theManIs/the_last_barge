using System;
using System.Collections;
using System.Collections.Generic;
using ArchimedsLab;
using Assets.Scripts.Guns.MedievalCannon;
using Assets.Scripts.Ships;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class NavyBrig : NavalNavigation
{
    public float PassCourseTiming = 5;
    public BillboardHealth HealthCanvas;
    public bool IsDead = false;

    [Header("Military effectiveness")]
    public ArmorType ShipArmorType = ArmorType.NonArmored;

    public int ArmorQuantity = 0;

    private float _nextAdjust = 0;
    private Animator _animator;
    private int _healthLevel = 100;
    protected int TmpDamage = 10;

    // Start is called before the first frame update
    protected void Start()
    {
        /** Water interaction */
        Rb = GetRigidBody();

        if (!GetComponent<MeshCollider>())
        {
            MeshCollider mc = gameObject.AddComponent<MeshCollider>();
            mc.convex = true;
            mc.sharedMesh = BuoyancyMesh;
        }

        if (!Rb)
            Rb = GetComponent<Rigidbody>();

        S_centerOfMass = Rb.centerOfMass;
        _lastPosition = transform.position;
        Rb.mass = TotalMass;

        WaterCutter.CookCache(BuoyancyMesh, ref _triangles, ref worldBuffer, ref wetTris, ref dryTris);
        /** Water interaction */

        /** Health display */
        if (HealthCanvas)
        {
            HealthCanvas.SetHealth(_healthLevel);

            if (ShipArmorType == ArmorType.Armored)
            {
                HealthCanvas.DisableArmorAmount(3 - ArmorQuantity); 
            }
            else
            {
                HealthCanvas.DisableArmorAmount(3);
            }
        }
        /** Health display */

        _animator = GetComponentInChildren<Animator>();
    }

    private bool _crushCourse = false;

    // Update is called once per frame
    protected void Update()
    {
        if (!this.IsDead)
        {
            ChangeCourse();

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
                for (int i = 0; i < YawAngle; i++)
                {
                    RudderRight();
                }

                //            transform.Rotate(new Vector3(0,  YawAngle, 0));
                //        
                //            _rb.velocity = transform.forward * Velocity;
                _nextAdjust = Time.time + Random.value * PassCourseTiming;

            }

            ThrottleUp();
            AngleDamping();
//            FixedUpdate2();
        }
        else
        {
            Brake();
            AngleDamping();
        }
    }

    protected void FixedUpdate()
    {
        AddTorque();
        FixedUpdate2();
    }


    protected void OnTriggerEnter()
    {
        _healthLevel -= TmpDamage;

        if (HealthCanvas)
        {
            HealthCanvas.TakeDamage(TmpDamage);
        }

        if (_healthLevel < 0)
        {
            IsDead = true;

            if (_animator)
            {
                _animator.SetBool("break", true);
            }

            DecommissionBrig();

            if (HealthCanvas)
            {
                HealthCanvas.DisableHealthBar();
                HealthCanvas.DisableArmorAmount(3);
            }
        }
    }

    protected void DecommissionBrig()
    {
        if (!_animator)
        {
            Destroy(gameObject, 1);
        }
        else
        {
            AnimatorClipInfo[] ac = _animator.GetCurrentAnimatorClipInfo(0);

            if (ac.Length > 0)
            {
                Destroy(gameObject, ac[0].clip.length);
            }
            else
            {
                Invoke("DecommissionBrig", 1);
            }
        }
    }


    public int TotalMass = 30000;
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

    protected void FixedUpdate2()
    {
    #if UNITY_EDITOR
        if (Rb.centerOfMass != S_centerOfMass + centerOfMassOffset)
                Rb.centerOfMass = S_centerOfMass + centerOfMassOffset;
    #endif

        Vector3 speedVector = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;

        if (Rb.IsSleeping())
            return;

        WaterCutter.CookMesh(transform.position, transform.rotation, ref _triangles, ref worldBuffer);

        WaterCutter.SplitMesh(worldBuffer, ref wetTris, ref dryTris, out nbrWet, out nbrDry, realist);
        Archimeds.ComputeAllForces(wetTris, dryTris, nbrWet, nbrDry, speedVector, Rb);
    }

#if UNITY_EDITOR
    //Some visualizations for this buoyancy script.
    protected void OnDrawGizmos()
    {
        if (Rb)
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(Rb.worldCenterOfMass, 0.25F);

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
