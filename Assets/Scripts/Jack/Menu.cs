using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public static Menu Instance { get; private set; }

	public bool gamePaused;
	public static Action StartDayAction;
	public static Action EndDayAction;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		Debug.Log(Progress.CheckValidSave(0));
		Progress.Load();

	}

	public void StartDay()
	{
		Debug.Log("menu start day");
		UnPause();
		StartDayAction.Invoke();
	}
	public void EndDay() {
		Debug.Log("menu end day");
		Pause();
		EndDayAction.Invoke();
		Invoke(nameof(StartDay), 5f);
    }

	public void Pause() {
		gamePaused = true;
		Time.timeScale = 0;
    }
	public void UnPause()
	{
		gamePaused = false;
		Time.timeScale = 1;
	}

	private void OnApplicationQuit()
	{
		Progress.Save();
	}
}
