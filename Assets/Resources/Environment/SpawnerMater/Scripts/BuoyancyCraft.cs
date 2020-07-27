using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BuoyancyCraft : MonoBehaviour
{
    #region Fields

    public Transform ObjectToSpawn;
    public Vector3 ErrorAngle;
    public float AccelerationStep = 1;
    public float SpawningCooldown = 20;
    public int OverallEnemies = 5;
    public Vector3 BlinkCounter = Vector3.one * 10;

    private readonly FrameLocker _fl = new FrameLocker(); 

    #endregion


    #region UnityMethods

    private void Start() => UnityStart();

    private void Update() => UnityUpdate();

    #endregion


    #region Methods

    protected void UnityStart()
    {
        _fl.LockSeconds = SpawningCooldown;
    }

    protected void UnityUpdate()
    {
        if (OverallEnemies > 0)
        {
            if (_fl.CheckTime())
            {
                Vector3 startPos = transform.position;
                startPos.x += Random.Range(-BlinkCounter.x, BlinkCounter.x);
                startPos.z += Random.Range(-BlinkCounter.z, BlinkCounter.z);
                startPos.y += Random.Range(-BlinkCounter.y, BlinkCounter.y);
                Transform obj = Instantiate(ObjectToSpawn, transform);
                Vector3 thisEuler = obj.rotation.eulerAngles;
                obj.rotation = Quaternion.Euler(thisEuler.x + ErrorAngle.x, thisEuler.y + ErrorAngle.y, thisEuler.z + ErrorAngle.z);
                obj.transform.position = startPos;

                SpawningCooldown -= AccelerationStep;
                _fl.LockSeconds = SpawningCooldown;
                OverallEnemies--;

                _fl.StartCountdown();
            }

            _fl.Countdown();
        }
    } 

    #endregion
}
