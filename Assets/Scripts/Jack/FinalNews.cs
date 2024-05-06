using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalNews : MonoBehaviour
{
	public TMP_Text finalScoreText;
	private void Awake()
	{
		Progress.SetScoreOnLevel(1, 3);
		Progress.SetScoreOnLevel(2, 2);
		Progress.SetScoreOnLevel(3, 1);
		Progress.Save();
	}
	public void EndOfDayScreen() {
		int finalscore = 0;
        for(int i =0; i < 5; i++) {
            Transform row = transform.GetChild(i);
			int score = Progress.GetScoreOnLevel(SceneManager.GetActiveScene().buildIndex - 5 + i);
			finalscore += score;
			Debug.Log("sc " + i + " " + score);
			for (int s = 0; s < 3; s++) {
				Transform star = row.GetChild(s);
				if (s < score) {
					Debug.Log("starON");
					star.GetComponent<SpriteRenderer>().color = Color.white;
                }
	        }
	    }
		finalScoreText.text = finalscore.ToString();
    }
}
