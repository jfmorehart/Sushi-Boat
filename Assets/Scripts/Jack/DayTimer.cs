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
    }
    // Update is called once per frame
    void Update()
    {
        if (live) {
            currentDay -= Time.deltaTime;
            timer.text = Mathf.Round(currentDay).ToString();
            if(currentDay <= 0) {
                EndDay();
	        }
	    }
    }
}
