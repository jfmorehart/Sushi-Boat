using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWave : MonoBehaviour
{
	public float speed_base;
	public float sinFreq;
	public float sinAmp;
	public bool ready = false;
	private bool leaving = false;


	private void Start()
	{
		Init();
		StartCoroutine(PullUp());
	}

	public void Init()
	{
		transform.GetChild(0).gameObject.SetActive(true);
	}

	private void Update()
	{
		transform.Translate(Mathf.Sin(Time.time / sinFreq) * sinAmp * Time.deltaTime * transform.up, Space.World);
		if (ready)
		{
			if (transform.GetChild(0).GetComponent<Customer>().finished)
			{
				if (!leaving)
				{
					leaving = true;
					StartCoroutine(Leave());
				}
			}
		}

	}

	public IEnumerator PullUp()
	{
		float timeElapsed = 0;
		Vector3 startPos = Vector3.zero;

		Vector3 targetPos = new Vector3(2, 2, 1);
		while (timeElapsed < 2f)
		{
			transform.localPosition = Vector3.Lerp(startPos, targetPos, timeElapsed / 2f);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		transform.localPosition = targetPos;
		ready = true;

	}


	public IEnumerator Leave()
	{
		float timeElapsed = 0;
		Vector3 startPosition = transform.position;

		Vector3 targetPosition =
			new Vector3(FishSpawner.ins.rightBound + 2, transform.position.y, transform.position.z);
		while (timeElapsed < 4f)
		{
			transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / 4f);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		transform.position = targetPosition;
		CustomerSpawner.Instance.currentBoatCount = Mathf.Max(0, CustomerSpawner.Instance.currentBoatCount - 1);
		Destroy(gameObject);
	}
}
