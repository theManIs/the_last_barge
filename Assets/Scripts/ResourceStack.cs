using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStack : MonoBehaviour
{
    public Transform ResourceInstance;
    public float GapSize = 0.1f;
    public int StackSize = 7;
    public int StackWidth = 2;
    public int StackDepth = 2;
    public int StackHeight = 3;
    public float InstanceWidth = 1;
    public float InstanceHeight = 1;
    public bool ChessPosition = false;
    public bool DoGizmosDrawn = false;
    public bool ReleaseStack = false;

    // Start is called before the first frame update
    void Start()
    {
//        PlaceResources();
    }

    public void PlaceResources()
    {
        if (ResourceInstance)
        {
            int totalPile = StackSize;
            float xOffset = 0f;
            int localStackWidth = StackWidth;

            for (int iii = 0; iii < StackHeight; iii++)
            {
                if (ChessPosition)
                {
                    xOffset = iii % 2 == 0 ? 0 : InstanceWidth / 2;
                    localStackWidth = iii % 2 == 0 ? StackWidth : StackWidth - 1;
                }

                for (int ii = StackDepth; ii > 0; ii--)
                {
                    for (int i = 0; i < localStackWidth; i++)
                    {
                        if (totalPile > 0)
                        {
                            Transform resourcePile = Instantiate(ResourceInstance, transform);
                            resourcePile.localPosition = new Vector3(((InstanceWidth + GapSize) * i) + xOffset, (InstanceHeight + GapSize) * iii, (InstanceWidth + GapSize) * ii);

                            totalPile--;
                        }
                    }
                }
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        ClearPile();
        PlaceResources();
    }

    public void ClearPile()
    {
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!DoGizmosDrawn)
        {
            PlaceResources();

            DoGizmosDrawn = true;
        }

        if (ReleaseStack)
        {
            ClearPile();

            ReleaseStack = false;
        }
    }
#endif
}
