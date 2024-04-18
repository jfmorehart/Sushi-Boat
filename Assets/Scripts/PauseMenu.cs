using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public void Continue()
    {
        GameManager.paused = false;
        Time.timeScale = 1;
        gameObject.SetActive(false);

    }

    public void MainMenu()
    {
		GameManager.paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
		GameManager.paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMap()
    {
		GameManager.paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene("MapScene");
    }

    public void EraseProgress() {
        Progress.NewLoad();
        Restart();
    }
}
