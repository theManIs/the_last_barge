using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarNumberMono : MonoBehaviour
{
    public Slider WaveSlider;
    public Text WaveText;

    public void SetMax(int intMax) => WaveSlider.maxValue = intMax;
    public void SetMin(int intMin) => WaveSlider.minValue = intMin;
    public void SetPower(int truePower) => WaveSlider.value = truePower;

    public void DisplayText() => WaveText.text = $"{WaveSlider.value}/{WaveSlider.maxValue}";

    protected void Update()
    {
        DisplayText();
    }
}
