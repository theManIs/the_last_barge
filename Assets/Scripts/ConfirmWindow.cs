using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmWindow : MonoBehaviour
{
    public PointOfInterest ParentPointOfInterest;

    public void OnClickConfirmButton()
    {
        if (ParentPointOfInterest && ParentPointOfInterest.ParentMapRandomizer)
        {
            ParentPointOfInterest.ParentMapRandomizer.ReleaseModal = true;
            ParentPointOfInterest.OnMouseExit();
            ParentPointOfInterest.ParentMapRandomizer.MoveBargeToPosition(ParentPointOfInterest.transform.position);
        }

        Destroy(gameObject);
    }
}
