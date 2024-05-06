using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
	public static Menu Instance { get; private set; }

	public bool gamePaused;
	bool queuedRestart;
	public static Action StartDayAction;
	public static Action EndDayAction;
	public Sprite[] rankings;
	public SpriteRenderer rankingPage;

	public SpriteRenderer[] stars;

	public TMP_Text quality;
	public TMP_Text peoplefed;
	public TMP_Text peoplemissed;
	public TMP_Text scoretext;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		Progress.Load();
		StartDayAction = null;
		EndDayAction = null;

	}
	
	//Defunct
	public void StartDay()
	{
		//apparently this doesnt get called?
		UnPause();
		StartDayAction.Invoke();
		GameManager.Instance.gameState = GameManager.GameState.DayGoing;

	}
	public void EndDay() {
		queuedRestart = false;
		GameManager.Instance.gameState = GameManager.GameState.DayEnded;
		//EndDayAction.Invoke();
		Camera.main.GetComponent<CameraController>().EndDay();
		Debug.Log(OrderManager.Instance.completed + " " + OrderManager.Instance.totalOrders);
		//float ratio = (1 + (float)OrderManager.Instance.numOrdersEaten) / (1f + (float)OrderManager.Instance.totalOrders);
		float score = GameManager.Instance.money / (float)CustomerSpawner.Instance.targetScore;
		Debug.Log(score + " score");


		int starCount = 0;
		if(score > 0.2f) {
			starCount = 1;
		}
		if (score > 0.75f)
		{
			starCount = 2;
		}
		if (score > 0.95f)
		{
			starCount = 3;
		}
		for (int i = 0; i < 3; i++) {
			stars[i].color =  i < starCount ? Color.white : Color.black;
		}

		int fed = OrderManager.Instance.numOrdersEaten;
		int missed = OrderManager.Instance.totalOrders - OrderManager.Instance.numOrdersEaten;
		missed = Mathf.Max(missed, 0);
		peoplefed.text = fed.ToString();
		peoplemissed.text = missed.ToString();
		quality.text = ((int)(OrderManager.Instance.averageOrderQuality * 100)).ToString() + "%";
		scoretext.text = "$" + GameManager.Instance.money.ToString();
		int prev = Progress.GetScoreOnLevel(SceneManager.GetActiveScene().buildIndex - 1);
		Progress.SetScoreOnLevel(SceneManager.GetActiveScene().buildIndex - 1, Mathf.Max(starCount, prev));
		Progress.Save();
	}
	//Defunct?
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
		SceneManager.LoadScene("MapScene");
	}
	public void ReloadScene()
	{
		Progress.Save();
		SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex - 1).ToString() );
	}
}
