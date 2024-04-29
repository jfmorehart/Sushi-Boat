using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public static DayTimer ins;

    [SerializeField]
    public float secondsPerDay;

    public static float secondsRemainingToday;
    [SerializeField] bool live;

    public float dayStartAngle;
    public float dayEndAngle;
    public float nightAngle;

    public float springDamp;
    public float accel;

    public Material mat;
    

	private void Awake()
	{
        if(ins != this && ins != null) {
            Destroy(gameObject);
	    }
        ins = this;
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
		StartCoroutine(nameof(NightBounce));
		Menu.Instance.EndDay();
    }
    // Update is called once per frame
    void Update()
    {
        if (live) {
            secondsRemainingToday -= Time.deltaTime;
            if(CustomerSpawner.Instance.currentBoatCount == 0 && secondsRemainingToday < 30) {
				secondsRemainingToday -= 3 * Time.deltaTime;
                //4x speed after end
			}
            mat.SetFloat("_clerp", 1 - secondsRemainingToday / secondsPerDay);
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
		}

        yield break;
    }
	IEnumerator DayBounce()
	{
		float v = 0;
		float dur = 1f;
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
		}
		StartDay();
		yield break;
	}
}
