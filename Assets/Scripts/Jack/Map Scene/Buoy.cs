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

	private void Awake()
	{
		ren = GetComponent<SpriteRenderer>();
		regScale = transform.localScale;
		bigScale = regScale * 2;
		if(Progress.maxUnlockedLevel >= levelToLoad) {
			unlocked = true;
		}
		else {
			ren.color = Color.gray;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("enter " + gameObject.name);
		transform.localScale = bigScale;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("exit " + gameObject.name);
		transform.localScale = regScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (unlocked) {
			PlayerPrefs.SetInt("level", levelToLoad);
			SceneManager.LoadScene(levelToLoad);
		}
	}
}
