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

	[NonReorderable]
	public Sprite[] customers;
    
    bool lastSpawn;

    public int maxBoatCount = 1;
    public int currentBoatCount = 0;
    
    
    //Adjustable Settings
    public bool twoLanes = false;
    public bool doubleOrders = false;
    public int maxOrderCountPerPerson = 3; 
    public int customerPerBoat =1;

    public bool bossLock = false; //set by bosslogic on boss level to ensure customers dont spawn until we want
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
        if (GameManager.Instance.gameState == GameManager.GameState.DayGoing && !bossLock)
        {
            if (currentBoatCount < maxBoatCount)
            {
                Debug.Log("spawn " + bossLock);
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
        OrderManager.Instance.totalOrders += 2;
        c.transform.position = spawnPos;
	}

    public Sprite GetCustomerState(int customer, int state) {
        if (state > 3) state = 3;
        if (customer > 9) customer = 9;
        //0 happy, etc
        return customers[customer * 4 + state];
    }
}
