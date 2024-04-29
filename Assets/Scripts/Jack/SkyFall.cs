using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFall : MonoBehaviour
{
    public Vector2 start;
    public Vector2 end;
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = start;
    }

    // Update is called once per frame
    void Update()
    {
        float lpos = DayTimer.secondsRemainingToday / DayTimer.ins.secondsPerDay;
        transform.localPosition = Vector2.Lerp(start, end, 1 - lpos);
    }
}
