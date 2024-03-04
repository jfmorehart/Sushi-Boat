using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Buoy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public int levelToLoad;

	Vector3 regScale;
	Vector3 bigScale;

	private void Awake()
	{
		regScale = transform.localScale;
		bigScale = regScale * 2;
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
		PlayerPrefs.SetInt("level", levelToLoad);
		SceneManager.LoadScene(0);
	}
}