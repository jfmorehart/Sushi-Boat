using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform boatTarget;
	public Transform skyTarget;
	public Transform fishTarget;
	[HideInInspector] public Transform target;
	public Vector3 camoffset;

	public float accel, velo, damp, followDist, sizeMult, distFromFloor;
	public float floor;

	public bool trackingHook;

	public Material dist;

	public bool locked;

	private void Awake()
	{
		Menu.EndDayAction += EndDay;
		Menu.StartDayAction += StartDay;
	}
	void Start() {
		trackingHook = false;
		target = boatTarget;
		transform.position = target.transform.position - camoffset;
		var f = GameObject.FindGameObjectWithTag("Finish");
		if(f != null) {
			floor = f.transform.position.y + distFromFloor;
		}
	}
    void StartDay(){
		target = boatTarget;
		
	}
	public void EndDay(){
		trackingHook = false;
		target = skyTarget;
		Hook.ins.active = false;
	}
	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space) && DayTimer.secondsRemainingToday > 0.5f 
	    && GameManager.Instance.gameState == GameManager.GameState.DayGoing && !locked) {

			if (trackingHook) {
				Hook.ins.active = false;
				trackingHook = false;
				target = boatTarget;
			}
			else {
				Hook.ins.active = true;
				//target = Hook.ins.transform;
				target = fishTarget;
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

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		dist.SetFloat("_yval", transform.position.y);
		Graphics.Blit(source, destination, dist);
	}
}
