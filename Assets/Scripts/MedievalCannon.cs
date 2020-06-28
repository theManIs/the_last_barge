using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Guns.MedievalCannon;
using UnityEngine;

public class MedievalCannon : MonoBehaviour
{
    [Header("Main characteristics")]
    public Transform CannonShell;
    public string FirePointPath = "FirePoint";
    public string VerticalRotationBlock = "IronCannon";
    public string HorizontalRotationBlock = "GunCarriage";


    [Header("BarrelEffectiveness")]
    public float ShellDestruction = 5;
    public float ProjectileForce = 25;
    public float EffectiveDistance = 100;
    public float SecCooldown = 1;
    public ArmorType PreferredTarget = ArmorType.NonArmored;


    private NavyBrig[] _navyAims;
    private float _nestShot;
    private Transform _firePoint;
    private Transform _horizontalBlock;
    private Transform _verticalBlock;
    private NavyBrig _navalTarget;


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
        _navalTarget = null;
        _navyAims = FindObjectsOfType<NavyBrig>();

        _navalTarget = FindNavalTarget(_navyAims, PreferredTarget);

        if (_navalTarget == null)
        {
            _navalTarget = FindNavalTarget(_navyAims, ArmorType.Indifference);
        }

        if (_navalTarget != null)
        {
            BoxCollider navalBox = _navalTarget.GetComponent<BoxCollider>();

            if (navalBox)
            {
                Vector3 dir = navalBox.bounds.center - _horizontalBlock.position;
                Vector3 eulerAngles = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
                _horizontalBlock.rotation = Quaternion.Euler(new Vector3(0, eulerAngles.y, 0));

                Vector3 dir2 = navalBox.bounds.center - _verticalBlock.position;
                Vector3 verticalShift = _verticalBlock.rotation.eulerAngles;
                verticalShift.x = Quaternion.LookRotation(dir2, Vector3.up).eulerAngles.x;
                _verticalBlock.rotation = Quaternion.Euler(verticalShift);

                FireInTheHole();
            }
        }

    }

    protected NavyBrig FindNavalTarget(NavyBrig[] navalAims, ArmorType preferredArmor)
    {
        float lastDistance = EffectiveDistance;
        NavyBrig navalTarget = null;

        foreach (NavyBrig navyBrig in navalAims)
        {
            if (!navyBrig.IsDead)
            {
                if (navyBrig.ArmorType == preferredArmor || ArmorType.Indifference == preferredArmor)
                {
                    float totalDistance = Mathf.Abs(Vector3.Distance(transform.position, navyBrig.transform.position));

                    if (totalDistance < lastDistance)
                    {
                        navalTarget = navyBrig;
                        lastDistance = totalDistance;
                    }
                }
            }
        }

        return navalTarget;
    }

    private void FireInTheHole()
    {
        if (Time.time > _nestShot)
        {
            BoxCollider navalBox = _navalTarget.GetComponent<BoxCollider>();

            if (CannonShell && navalBox)
            {
                GameObject shellInstance = Instantiate(CannonShell.gameObject, _firePoint);
                Rigidbody rb = shellInstance.GetComponent<Rigidbody>();
                rb = !rb ? shellInstance.AddComponent<Rigidbody>() : rb;
                rb.useGravity = false;
                BoxCollider boxCollider = shellInstance.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                Vector3 shellDirection = (navalBox.bounds.center - shellInstance.transform.position).normalized;
                shellInstance.transform.LookAt(navalBox.transform);

                rb.velocity = shellDirection * ProjectileForce;

                Destroy(shellInstance, ShellDestruction);
            }

            _nestShot = Time.time + SecCooldown;
        }
    }

}
