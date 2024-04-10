using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
	public static FishSpawner ins;

	public List<FishData> fishDatas;
	List<int> fhistory = new List<int>();

	public List<GameObject> obstacleprefabs;
	List<int> ohistory = new List<int>();

	int historySize = 4;

	public float fishSpawnRate;
	float lastFishSpawn;

	public float obstacleSpawnRate;
	float lastObstacleSpawn;

	//Bounds are used for knowing where to spawn/despawn the fish
	[HideInInspector]
	public float leftBound;
	[HideInInspector]
	public float rightBound;

	public float upperBound;
	public float lowerBound;

	public GameObject fishPrefab;
	public GameObject backgroundfishPrefab;

	private void Awake()
	{
		if(ins != null) {
			Destroy(gameObject);
		}
		else {
			ins = this;
		}
		AssignFishBounds();

		GameObject floor = GameObject.FindGameObjectWithTag("Finish");
		if(floor != null) {
			lowerBound = floor.transform.position.y + 1;
		}
	}

	private void Update()
	{
		if (GameManager.Instance.gameState == GameManager.GameState.DayGoing)
		{
			if(Time.time - lastFishSpawn > (1 / fishSpawnRate)) {
				lastFishSpawn = Time.time;
				SpawnFish();
				SpawnBackgroundFish();
			}
			if (Time.time - lastObstacleSpawn > (1 / obstacleSpawnRate))
			{
				lastObstacleSpawn = Time.time;
				SpawnObstacle();
			}
		}

	}

	void SpawnFish() {
		bool facingRight = (Random.Range(0, 1000) % 2 == 0);
		Vector2 pos = new Vector2(facingRight? leftBound : rightBound, Random.Range(lowerBound, upperBound));
		int r = FishPseudoRandomizer();
		GameObject go = Instantiate(fishPrefab, pos, Quaternion.identity, transform);
		Fish f = go.GetComponent<Fish>();
		f.Init(fishDatas[r]);
		f.direction = facingRight ? 1 : -1;
		go.tag = "Fish";
    }
	void SpawnObstacle()
	{
		if (obstacleprefabs.Count < 1) return;
		bool facingRight = (Random.Range(0, 1000) % 2 == 0);
		Vector2 pos = new Vector2(facingRight ? leftBound : rightBound, Random.Range(lowerBound, upperBound));
		int r = ObstaclePseudoRandomizer();
		GameObject go = Instantiate(obstacleprefabs[r], pos, Quaternion.identity, transform);
		go.GetComponent<Fish>().direction = facingRight ? 1 : -1;
		go.tag = "Obstacle";
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

	int FishPseudoRandomizer() {
		int r = Random.Range(0, fishDatas.Count);
		if (fhistory.Contains(r)) {
			r = Random.Range(0, fishDatas.Count);
		}
		fhistory.Add(r);
		if (fhistory.Count > Mathf.Min(historySize, fishDatas.Count))
			fhistory.RemoveAt(0);
		return r;
    }
	int ObstaclePseudoRandomizer()
	{
		int r = Random.Range(0, obstacleprefabs.Count);
		if (ohistory.Contains(r))
		{
			r = Random.Range(0, obstacleprefabs.Count);
		}
		ohistory.Add(r);
		if (ohistory.Count > Mathf.Min(historySize, obstacleprefabs.Count))
			ohistory.RemoveAt(0);
		return r;
	}

	void SpawnBackgroundFish()
	{
		bool facingRight = (Random.Range(0, 1000) % 2 == 0);
		Vector2 pos = new Vector2(facingRight ? leftBound : rightBound, Random.Range(lowerBound, upperBound));
		int r = Random.Range(0, fishDatas.Count); //to not fuck with the pseudo

		GameObject go = Instantiate(backgroundfishPrefab, pos, Quaternion.identity, transform);
		go.GetComponent<Fish>().Init(fishDatas[r]);
		go.GetComponent<Fish>().direction = facingRight ? 1 : -1;
	}

}
