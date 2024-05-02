using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StarProgress : MonoBehaviour
{
    public RawImage[] stars;
	float score = 0;

	// Update is called once per frame
	private void Start()
	{
		for(int i = 0; i < 3; i++) {
			stars[i].material = new Material(stars[i].material);
		}
	}
	void Update()
    {
		if(GameManager.Instance.gameState == GameManager.GameState.DayGoing) {
			float rscore = GameManager.Instance.money / (float)CustomerSpawner.Instance.targetScore;
			score = Mathf.Lerp(score, rscore, Time.deltaTime);

			for (int i = 0; i < 3; i++)
			{
				float progress = 0;

				if (i == 0)
				{
					progress = score / 0.1f;
				}
				if (i == 1)
				{
					if(score > 0.1f) {
						progress = score / 0.5f;
					}
	
				}
				if (i == 2)
				{
					if (score > 0.5f)
					{
						progress = score / 1f;
					}

				}
				stars[i].material.SetFloat("_prog", progress);
			}
		}
    }
}
