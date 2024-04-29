using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public GameObject credit;
	private void Start()
	{
		
	}
	public void StartGame()
    {
		if (!Progress.HasBeatenTutorial())
		{
			SceneManager.LoadScene("Tutorial");
            return;
		}
		SceneManager.LoadScene("MapScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits()
    {
	    credit.SetActive(!credit.activeSelf);
    }
    
    
}
