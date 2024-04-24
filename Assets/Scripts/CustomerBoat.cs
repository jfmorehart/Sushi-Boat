using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBoat : MonoBehaviour
{
	public float speed_base;
	public float sinFreq;
	public float sinAmp;
	public bool ready = false;
	private bool leaving = false;

	public bool bossLevel;

	public float timer;

	private void Start()
	{
		Init();
		StartCoroutine(PullUp());
	}

	public void Init()
	{
		if (CustomerSpawner.Instance.numOrdersThisWave < 2)
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			transform.GetChild(1).gameObject.SetActive(true);
			transform.GetChild(2).gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		timer -= Time.deltaTime;

		transform.Translate(Mathf.Sin(Time.time / sinFreq) * sinAmp * Time.deltaTime * transform.up, Space.World);
		if (ready)
		{
			if (CustomerSpawner.Instance.numOrdersThisWave < 2)
			{
				if (transform.GetChild(0).GetComponent<Customer>().finished)
				{
					Debug.Log("fin");
					if (!leaving)
					{
						Debug.Log("going");
						leaving = true;
						StartCoroutine(Leave());
						//transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
					}
				}
			}
			else
			{
				if (transform.GetChild(1).GetComponent<Customer>().finished &&
					transform.GetChild(2).GetComponent<Customer>().finished)
				{
					if (!leaving)
					{
						leaving = true;
						StartCoroutine(Leave());
						//transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
						//transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
					}
				}
			}

		}

	}

	public IEnumerator PullUp()
	{
		if (bossLevel)
		{
			StartCoroutine(Rise());
			yield break;
		}

		float timeElapsed = 0;
		Vector3 startScale = Vector3.zero;

		Vector3 targetScale = new Vector3(2, 2, 1);
		while (timeElapsed < 2f)
		{
			transform.localScale = Vector3.Lerp(startScale, targetScale, timeElapsed / 2f);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		transform.localScale = targetScale;
		ready = true;

	}


	public IEnumerator Leave()
	{
		if (bossLevel)
		{
			StartCoroutine(Fall());
			yield break;
		}
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

	public IEnumerator Rise()
	{
		float timeElapsed = 0;
		Vector3 startPos = new Vector3(0, 20, 0);

		Vector3 targetPos = new Vector3(0, 32, 0);
		while (timeElapsed < 2f)
		{
			transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / 2f);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		ready = true;
		//transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
		//transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
	}
	public IEnumerator Fall()
	{
		float timeElapsed = 0;
		Vector3 startPos = new Vector3(0, 32, 0);

		Vector3 targetPos = new Vector3(0, 20, 0);
		while (timeElapsed < 2f)
		{
			transform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / 2f);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		CustomerSpawner.Instance.currentBoatCount = Mathf.Max(0, CustomerSpawner.Instance.currentBoatCount - 1);
		Destroy(gameObject);
	}
}
