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
    public string VerticalRotationBlock = "IronCannon";
    public string HorizontalRotationBlock = "GunCarriage";

    private NavyBrig[] _navyAims;
    private float _nestShot;
    private Transform _firePoint;
    private Transform _horizontalBlock;
    private Transform _verticalBlock;


    // Start is called before the first frame update
    void Start()
    {
        _navyAims = FindObjectsOfType<NavyBrig>();
        _firePoint = transform.Find(FirePointPath);
        _horizontalBlock = transform.Find(HorizontalRotationBlock);
        _verticalBlock = transform.Find(VerticalRotationBlock);
        _firePoint = _firePoint ? _firePoint : transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_navyAims.Length > 0)
        {
            NavyBrig navyAim = _navyAims[0];

            Vector3 dir = navyAim.transform.position - _horizontalBlock.position;
            Vector3 eulerAngles = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
            _horizontalBlock.rotation = Quaternion.Euler(new Vector3(0, eulerAngles.y, 0));

            Vector3 dir2 = navyAim.transform.position - _verticalBlock.position;
            Vector3 verticalShift = _verticalBlock.rotation.eulerAngles;
            verticalShift.x = Quaternion.LookRotation(dir2, Vector3.up).eulerAngles.x;
            _verticalBlock.rotation = Quaternion.Euler(verticalShift);

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

                rb.velocity = _verticalBlock.forward * Velocity;

                Destroy(shellInstance, ShellDestruction);
            }

            _nestShot = Time.time + Timing;
        }
    }

}
