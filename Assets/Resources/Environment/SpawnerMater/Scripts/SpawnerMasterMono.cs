using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMasterMono : MonoBehaviour
{
    public int TotalWavePower = 100;
    public int ZeroWavePower = 0;
    public int TrueWavePower = 50;
    public BarNumberMono BarNumberMono;

    // Start is called before the first frame update
    void Start()
    {
        BarNumberMono?.SetMax(TotalWavePower);
        BarNumberMono?.SetMin(ZeroWavePower);

        TrueWavePower = TotalWavePower;
    }

    // Update is called once per frame
    void Update()
    {
        TrueWavePower -= Random.value < Time.deltaTime ? 1 : 0;

        BarNumberMono?.SetPower(TrueWavePower);
    }
}
