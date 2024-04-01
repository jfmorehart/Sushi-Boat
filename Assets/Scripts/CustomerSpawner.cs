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

    public float minimumOrderTime;
    
    
    bool lastSpawn;

    public int maxBoatCount = 1;
    public int currentBoatCount = 0;
    
    
    //Adjustable Settings
    public bool twoLanes = false;
    public bool doubleOrders = false;
    public int maxOrderCountPerPerson = 3; 
    public int customerPerBoat =1;

    private void Awake()
    {
        if(Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.DayGoing)
        {
            if (currentBoatCount<maxBoatCount)
            {
                SpawnBoat();
            }
        }

    }

    public void SpawnBoat()
    {
        StartCoroutine(DelaySpawn());
    }

    IEnumerator DelaySpawn()
    {
        currentBoatCount++;
        yield return new WaitForSeconds(Random.Range(1f,5f));
        Vector3 spawnPos = spawn.position;
        GameObject c = Instantiate(customerBoat);
        c.transform.position = spawnPos;
	}
}
