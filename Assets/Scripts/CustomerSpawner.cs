using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }
    public Transform spawn;
    public GameObject customerBoat;

	[NonReorderable]
	public Sprite[] customers;
    
    public int maxBoatCount = 1;
    public int currentBoatCount = 0;

    [HideInInspector]
    public int numOrdersThisWave;

    public int targetScore;
    public Wave[] waves;
    public int currentWave;
    public bool waveLive;
    public float waveTime;

    private void Awake()
    {
        if(Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
		numOrdersThisWave = waves[currentWave].numOrders;
	}

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.DayGoing&& !GameManager.Instance.tutorial)
        {
            waveTime += Time.deltaTime;
            EvaluateWave();
        }

    }

    public void EvaluateWave() {
        if (waveLive) {
			if (currentBoatCount < 1) //player fed the customers, new wave
			{
				NextWave();
                return;
			}
			if (waveTime > waves[currentWave].orderTime) { //timer ran out
                NextWave();
				return;
			}
  
        }
        else { 
	        if(waveTime > waves[currentWave].preDelay) {
                waveLive = true;
                SpawnBoat();
                waveTime = 0;
	        }
	    }
    }
    void NextWave() {
		waveLive = false;
        currentWave++;
        if(currentWave >= waves.Length) {
            currentWave = 0;
	    }
        waveTime = 0;
		numOrdersThisWave = waves[currentWave].numOrders;
	}

    public void SpawnBoat()
    {
		currentBoatCount++;
		Vector3 spawnPos = spawn.position;
		GameObject c = Instantiate(customerBoat);
		c.transform.position = spawnPos;
		OrderManager.Instance.totalOrders += waves[currentWave].numOrders;
		c.GetComponent<CustomerBoat>().timer = waves[currentWave].orderTime;
	}

    public Sprite GetCustomerState(int customer, int state) {
        if (state > 3) state = 3;
        if (customer > 9) customer = 9;
        //0 happy, etc
        return customers[customer * 4 + state];
    }
}
