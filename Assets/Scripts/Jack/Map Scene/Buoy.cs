using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Buoy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public int levelToLoad;
	public bool unlocked;

	Vector3 regScale;
	Vector3 bigScale;

	SpriteRenderer ren;

	public float tilt;
	float baseTilt;
	public float freq;

	float seed;
	private void Awake()
	{
		ren = GetComponent<SpriteRenderer>();
		regScale = transform.localScale;
		bigScale = regScale * 1.25f;
		if(Progress.maxUnlockedLevel >= levelToLoad) {
			unlocked = true;
		}
		else {
			ren.color = Color.gray;
		}
		baseTilt = tilt;
		seed = Random.Range(0, 100f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		tilt += 1;
		Debug.Log("enter " + gameObject.name);
		transform.localScale = bigScale;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tilt += 1;
		Debug.Log("exit " + gameObject.name);
		transform.localScale = regScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (unlocked) {
			PlayerPrefs.SetInt("level", levelToLoad);
			SceneManager.LoadScene(levelToLoad.ToString());
		}
	}
	private void Update()
	{
		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x, pos.y + 0.001f * Mathf.Sin(Time.time * freq + tilt + seed), 0);
		transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-tilt, tilt, 0.5f * (1 + Mathf.Sin(Time.time * freq + tilt + seed))));
		if(tilt > baseTilt) {
			tilt *= 1 - Time.deltaTime * 0.25f;
		}
	}
}
