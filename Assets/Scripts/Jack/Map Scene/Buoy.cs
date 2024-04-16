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

	public Transform[] stars;

	private void Awake()
	{
		if (levelToLoad == 1)
		{
			Progress.Load();

			//	//Progress.SetScoreOnLevel(1, 3);
			//	Progress.SetScoreOnLevel(2, 3);
			//	//Progress.maxUnlockedLevel = 3;
			//	Progress.Save();
			
		}
	}
	private void Start()
	{
		ren = GetComponent<SpriteRenderer>();
		regScale = transform.localScale;
		bigScale = regScale * 1.25f;

		if(Progress.maxUnlockedLevel >= levelToLoad) {
			unlocked = true;
			int score = Progress.GetScoreOnLevel(levelToLoad);
			//Debug.Log(levelToLoad + " " + score);
			for(int i = 0; i < stars.Length; i++) {
				stars[i].GetComponent<SpriteRenderer>().color = (i < score)? Color.white : Color.black;
				stars[i].GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else {
			ren.color = Color.gray;
			foreach (Transform t in stars)
			{
				t.GetComponent<SpriteRenderer>().color = Color.black;
				t.GetComponent<SpriteRenderer>().enabled = false;
			}
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
			//foreach (Transform t in stars)
			//{
			//	t.GetComponent<SpriteRenderer>().color = Color.white;
			//}
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
