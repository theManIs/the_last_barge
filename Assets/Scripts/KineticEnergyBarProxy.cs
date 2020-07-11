using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KineticEnergyBarProxy : MonoBehaviour
{
    public Image[] EnergyBlocks;
    public Color ActiveColor = Color.green;
    public Color NonactiveColor = Color.red;

    public void HighlightBlocks(int numBlocks)  
    {
        for (int i = 0; i < EnergyBlocks.Length; i++)
        {
            EnergyBlocks[i].color = numBlocks > i ? ActiveColor : NonactiveColor;
        }
    }
}
