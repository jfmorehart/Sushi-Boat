using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public TMP_Text timer; // temp

    [SerializeField]
    float secondsPerDay;

    public static float secondsRemainingToday;
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
        secondsRemainingToday = secondsPerDay;
    }
    void EndDay() {
        live = false;
		Debug.Log("calling nbounce");
		StartCoroutine(nameof(NightBounce));
		Menu.Instance.EndDay();
    }
    // Update is called once per frame
    void Update()
    {
        if (live) {
            secondsRemainingToday -= Time.deltaTime;
            //timer.text = Mathf.Round(currentDay).ToString();
            float z = Mathf.Lerp(dayEndAngle, dayStartAngle, (secondsRemainingToday / secondsPerDay));
            transform.eulerAngles = new Vector3(0, 0, z);
            if(secondsRemainingToday <= 0) {
                EndDay();
	        }
	    }
    }

    public void StartDayBounce(){
	    StartCoroutine(nameof(DayBounce));
	}
    IEnumerator NightBounce() {
        float v = 0;
        float dur = 2;
        Debug.Log("nbounce bby");
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
            yield return null;
			Debug.Log(dur);
		}
        yield break;
    }
	IEnumerator DayBounce()
	{
		float v = 0;
		float dur = 1f;
		Debug.Log("dbounce bby");
		while (dur > 0)
		{
			dur -= Time.unscaledDeltaTime;
			float z = transform.eulerAngles.z;
			float d = Vector2.SignedAngle(transform.right, Vector2.up);
			v += Mathf.Sign(d) * Time.unscaledDeltaTime * accel;

			if (Mathf.Abs(d) < 10)
			{
				v *= 1 - Time.unscaledDeltaTime * springDamp * springDamp;
			}
			else
			{
				v *= 1 - Time.unscaledDeltaTime * springDamp;
			}
			transform.eulerAngles = new Vector3(0, 0, z + v);
			yield return null;
			Debug.Log(dur);
		}
		yield break;
	}
}
