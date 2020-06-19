using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedievalCannon : MonoBehaviour
{
    public Transform CannonShell;
    public float Velocity = 25;
    public float Timing = 1;
    public float ShellDestruction = 3;
    public string FirePointPath = "FirePoint";

    private NavyBrig[] _navyAims;
    private float _nestShot;
    private Transform _firePoint;


    // Start is called before the first frame update
    void Start()
    {
        _navyAims = FindObjectsOfType<NavyBrig>();
        _firePoint = transform.Find(FirePointPath);
        _firePoint = _firePoint ? _firePoint : transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_navyAims.Length > 0)
        {
            NavyBrig navyAim = _navyAims[0];

            transform.LookAt(navyAim.transform);
        }

        FireInTheHole();
    }

    private void FireInTheHole()
    {
        if (Time.time > _nestShot)
        {
            if (CannonShell)
            {
                GameObject shellInstance = Instantiate(CannonShell.gameObject, _firePoint);
                Rigidbody rb = shellInstance.GetComponent<Rigidbody>();
                rb = !rb ? shellInstance.AddComponent<Rigidbody>() : rb;
                rb.useGravity = false;
                BoxCollider boxCollider = shellInstance.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;

                rb.velocity = transform.forward * Velocity;

                Destroy(shellInstance, ShellDestruction);
            }

            _nestShot = Time.time + Timing;
        }
    }

}
