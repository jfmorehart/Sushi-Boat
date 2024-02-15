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
	public int orderCount = 1;

	private SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		timer = timerMax;
	}

	public void Init(float tm,int dir,int ord)
	{
		timerMax = tm;
		timer = timerMax;
		direction = dir;
		orderCount = ord;
		for (int i = 0; i < orderCount; i++)
		{
			transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
		}
	}
	private void Update()
	{
		timer -= Time.deltaTime;


		transform.Translate(Mathf.Sin(Time.time / sinFreq) * sinAmp * Time.deltaTime * transform.up, Space.World);
		Vector3 pos = transform.position;
		if (direction == 0)
		{
			sr.flipX = false;
			transform.position = new Vector3(Mathf.Lerp(FishSpawner.ins.leftBound - 2,
					FishSpawner.ins.rightBound + 2, timer/timerMax),
				pos.y, 0);
		}
		else
		{
			sr.flipX = true;
			transform.position = new Vector3(Mathf.Lerp(FishSpawner.ins.rightBound + 2,
					FishSpawner.ins.leftBound - 2, timer/timerMax),
				pos.y, 0);
		}


		if(timer < 0) {
			Destroy(gameObject);
		}
	}
}
