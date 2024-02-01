using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	Transform target;
	public Vector3 camoffset;

	public float accel, velo, damp, followDist;

	void Start() {
		target = Hook.ins.transform;
    }

	private void Update()
	{
		//Sorry for not commenting this shit

		Vector3 pos = transform.position - camoffset;
		Vector2 delta = target.position - transform.position;
		if(delta.y > 0) {
			//pos.y = target.position.y;
			//velo = 0;
			float dterm = Mathf.Min(1, delta.magnitude - followDist);
			velo += accel * Time.deltaTime * dterm;
			velo *= 1 - Time.deltaTime * damp;
		}
		else {
			float dterm = Mathf.Min(1, delta.magnitude - followDist);
			velo -= accel * Time.deltaTime * dterm;
			velo *= 1 - Time.deltaTime * damp;
		}
		pos.y += velo * Time.deltaTime;
		if (pos.y > 0) {
			pos.y = 0;
		}
		transform.position = pos + camoffset;
		//Camera.main.orthographicSize = 5 + Mathf.Abs(velo) * 0.5f;

	}
}
