using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YachtRock : MonoBehaviour
{
    public float freq, amp, noisefreq, noiseamp;
    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.PerlinNoise1D(Time.time * noisefreq) * noiseamp;

        float sin = Mathf.Sin(Time.time * freq) * amp;
        float deg = (sin - 0.5f) + (noise - 0.5f);
        transform.eulerAngles = new Vector3(0, 0, deg);
    }
}
