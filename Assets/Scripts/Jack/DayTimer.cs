using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public TMP_Text timer; // temp

    [SerializeField]
    float secondsPerDay;

    float currentDay;
    bool live;

    public float dayStartAngle;
    public float dayEndAngle;
    public float nightAngle;

    public float springDamp;
    public float accel;

	private void Awake()
	{
        Menu.StartDayAction += StartDay;
	}
	private void Start()
	{
        StartDay();
	}
	void StartDay() {
        live = true;
        currentDay = secondsPerDay;
    }
    void EndDay() {
        live = false;
        Menu.Instance.EndDay();
        StartCoroutine(nameof(NightBounce));
    }
    // Update is called once per frame
    void Update()
    {
        if (live) {
            currentDay -= Time.deltaTime;
            //timer.text = Mathf.Round(currentDay).ToString();
            float z = Mathf.Lerp(dayEndAngle, dayStartAngle, (currentDay / secondsPerDay));
            transform.eulerAngles = new Vector3(0, 0, z);
            if(currentDay <= 0) {
                EndDay();
	        }
	    }
    }


    IEnumerator NightBounce() {
        float v = 0;
        float dur = 10;

        while(dur > 0) {
            dur -= Time.unscaledDeltaTime;
            float z = transform.eulerAngles.z;
            float d = Vector2.SignedAngle(transform.right, -Vector2.right);
            v += Mathf.Sign(d) * Time.unscaledDeltaTime * accel;

            if(Mathf.Abs(d) < 10) {
				v *= 1 - Time.unscaledDeltaTime * springDamp * springDamp;
            }
            else {
				v *= 1 - Time.unscaledDeltaTime * springDamp;
			}
            transform.eulerAngles = new Vector3(0, 0, z + v);
            yield return new WaitForEndOfFrame();
	    }
        yield break;
    }
}
