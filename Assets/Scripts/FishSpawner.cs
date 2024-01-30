using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
	public static FishSpawner ins;

	//Bounds are used for knowing where to spawn/despawn the fish
	[HideInInspector]
	public float leftBound;
	[HideInInspector]
	public float rightBound;

	public float upperBound;
	public float lowerBound;

	public GameObject fishPrefab;

	private void Awake()
	{
		if(ins != null) {
			Destroy(gameObject);
		}
		else {
			ins = this;
		}

		AssignFishBounds();

		InvokeRepeating(nameof(SpawnFish), 0.5f, 0.5f); //temp
	}

	void SpawnFish() {
		bool facingRight = (Random.Range(0, 1000) % 2 == 0);
		Vector2 pos = new Vector2(facingRight? leftBound : rightBound, Random.Range(lowerBound, upperBound));
		GameObject go = Instantiate(fishPrefab, pos, Quaternion.identity, transform);
		go.GetComponent<Fish>().direction = facingRight ? 1 : -1;
    }
	
	void AssignFishBounds() {
		//maybe doing it dynamically is dumb... 
		// just vaguely worried about larger screen sizes/ funky aspect ratios

		Vector3 pt = Camera.main.ScreenToWorldPoint(new Vector3(0 - Screen.width * 0.1f, 0, 0));
		leftBound = pt.x;
		Vector3 pt2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 0.1f * Screen.width, 0, 0)); ;
		rightBound = pt2.x;

		//Debug.Log(leftBound + " " + rightBound);
	}
}
