using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public static Menu Instance { get; private set; }

	public bool gamePaused;
	bool queuedRestart;
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
		StartDayAction = null;
		EndDayAction = null;
	}

	public void StartDay()
	{
		Debug.Log("menu start day");
		UnPause();
		StartDayAction.Invoke();
		GameManager.Instance.gameState = GameManager.GameState.DayGoing;
	}
	public void EndDay() {
		queuedRestart = false;
		Debug.Log("menu end day");
		Progress.Save();
		//Pause();
		EndDayAction.Invoke();
    }
    public void PreStart(){
		if (queuedRestart) return;
		queuedRestart = true;
		Invoke(nameof(StartDay), 1f);
		DayTimer.ins.StartDayBounce();
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

	public void SwitchToMap()
	{
		Progress.Save();
		SceneManager.LoadScene(0);
	}
}
