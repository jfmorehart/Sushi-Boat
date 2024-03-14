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
	public Sprite[] rankings;
	public SpriteRenderer rankingPage;
	
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
		UnPause();
		StartDayAction.Invoke();
		GameManager.Instance.gameState = GameManager.GameState.DayGoing;
	}
	public void EndDay() {
		queuedRestart = false;
		Progress.Save();
		//Pause();
		EndDayAction.Invoke();
		Debug.Log(OrderManager.Instance.completed + " " + OrderManager.Instance.totalOrders);
		float ratio = (1 + (float)OrderManager.Instance.completed) / (1f + (float)OrderManager.Instance.totalOrders);
		Debug.Log(ratio);
		int index = 10 - Mathf.FloorToInt(ratio * 10);
		if (index < 0) index = 0;
		if (index > 4) index = 4;
		rankingPage.sprite = rankings[index];
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
		Progress.maxUnlockedLevel++;
		Progress.Save();
		SceneManager.LoadScene(0);
	}
	public void ReloadScene()
	{
		Progress.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
