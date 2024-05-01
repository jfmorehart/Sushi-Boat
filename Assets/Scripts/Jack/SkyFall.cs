using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFall : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = start;
    }

    // Update is called once per frame
    void Update()
    {
        float lpos = DayTimer.secondsRemainingToday / DayTimer.ins.secondsPerDay;
        transform.localPosition = Vector3.Lerp(start, end, 1 - lpos);
    }
}
