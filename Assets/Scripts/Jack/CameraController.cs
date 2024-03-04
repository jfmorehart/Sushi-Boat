using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform boatTarget;
	public Transform skyTarget;
	Transform target;
	public Vector3 camoffset;

	public float accel, velo, damp, followDist, sizeMult, distFromFloor;
	public float floor;

	public bool trackingHook;

	void Start() {
		trackingHook = false;
		target = boatTarget;
		transform.position = target.transform.position - camoffset;
		Menu.EndDayAction += EndDay;
		Menu.StartDayAction += StartDay;
		var f = GameObject.FindGameObjectWithTag("Finish");
		if(f != null) {
			floor = f.transform.position.y + distFromFloor;
		}
	}
    void StartDay(){
		target = boatTarget;
		
	}
	void EndDay(){
		trackingHook = false;
		target = skyTarget;
	}
	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space) && DayTimer.secondsRemainingToday > 0.5f) {
			if (trackingHook) {
				trackingHook = false;
				target = boatTarget;
			}
			else { 
				target = Hook.ins.transform;
				trackingHook = true;
			}

		}
		//Sorry for not commenting this shit

		Vector3 pos = transform.position - camoffset;
		Vector2 delta = target.position - transform.position;
		if(delta.y > 1) {
			//pos.y = target.position.y;
			//velo = 0;
			float dterm = Mathf.Max(1, delta.magnitude - followDist);
			velo += accel * Time.deltaTime * dterm;
			velo *= 1 - Time.deltaTime * damp;
		}
		else if (delta.y < -1){
			float dterm = Mathf.Max(1, delta.magnitude - followDist);
			velo -= accel * Time.deltaTime * dterm;
			velo *= 1 - Time.deltaTime * damp;
		}
		else {
			velo *= 1 - Time.deltaTime * damp * 2;
		}
		if (pos.y > 0 && trackingHook) {
			velo -= accel * Time.deltaTime;
		}
		if (pos.y < floor)
		{
			pos.y = floor;
			velo = 0;
		}
		pos.y += velo * Time.deltaTime;
		transform.position = pos + camoffset;
		//float sz = Camera.main.orthographicSize;
		//float sz2 = Mathf.Max(5, 4 + Mathf.Pow(Mathf.Abs(Hook.ins.velocity), 1.2f) * sizeMult);
		//Camera.main.orthographicSize = (sz * 0.99f + sz2 * 0.01f);

	}
}
