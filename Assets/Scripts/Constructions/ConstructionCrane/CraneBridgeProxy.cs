using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constructions.ConstructionCrane;
using UnityEngine;

public class CraneBridgeProxy : MonoBehaviour
{
    public int BuildingNumber = 0;
    public bool StartBuilding = false;
    public Vector3 MousePosition = Vector3.zero;
    public Transform AvailableBuilding;
    public bool BuildingInProgress = false;
    public ConstructionFlagsCapsule Cfc = new ConstructionFlagsCapsule();
}
