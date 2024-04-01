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


	private void Start()
	{
		Init();
		StartCoroutine(PullUp());
	}

	public void Init()
	{
		if (CustomerSpawner.Instance.customerPerBoat == 1)
		{
			transform.GetChild(0).gameObject.SetActive(true);	
		}
		if (CustomerSpawner.Instance.customerPerBoat == 2)
		{
			transform.GetChild(1).gameObject.SetActive(true);	
			transform.GetChild(2).gameObject.SetActive(true);	
		}
	}
    
	private void Update()
	{
		transform.Translate(Mathf.Sin(Time.time / sinFreq) * sinAmp * Time.deltaTime * transform.up, Space.World);
		if (ready)
		{
			if (CustomerSpawner.Instance.customerPerBoat == 1)
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
			if (CustomerSpawner.Instance.customerPerBoat == 2)
			{
				if (transform.GetChild(1).GetComponent<Customer>().finished &&
				    transform.GetChild(2).GetComponent<Customer>().finished)
				{
					if (!leaving)
					{
						leaving = true;
						StartCoroutine(Leave());
					}
				}
			}

		}

	}

	public IEnumerator PullUp()
	{
		float timeElapsed = 0;
		Vector3 startScale = transform.localScale;

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
		CustomerSpawner.Instance.currentBoatCount--;
		Destroy(gameObject);
	}
}
