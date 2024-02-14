using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rowboat : MonoBehaviour
{
	public float speed_base;
	public float sinFreq;
	public float sinAmp;

	public float timer;
	float timerMax = 30;

	public int direction;

	private void Start()
	{
		timer = timerMax;
	}

	private void Update()
	{
		timer -= Time.deltaTime;

		transform.Translate(Mathf.Sin(Time.time / sinFreq) * sinAmp * Time.deltaTime * transform.up, Space.World);
		Vector3 pos = transform.position;
		transform.position = new Vector3(Mathf.Lerp(FishSpawner.ins.leftBound - 2,
			FishSpawner.ins.rightBound + 2, timer/timerMax),
			pos.y, 0);

		if(timer < 0) {
			Destroy(gameObject);
		}
	}
}
