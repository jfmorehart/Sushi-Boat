using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }
    public Transform leftSpawn;

    public Transform rightSpawn;

    public GameObject customer;

    public int currentOrders = 0;
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
            //temporary measure for keeping track of order count
            if (currentOrders < OrderManager.Instance.maxOrdersCount-1)
            {
                SpawnCustomer();
            }
        }

    }

    public void SpawnCustomer()
    {
        int direction = Random.Range(0, 2);
        int orderCount = Random.Range(0, 3);
        currentOrders += orderCount;
        StartCoroutine(DelaySpawn(direction, orderCount));
    }

    IEnumerator DelaySpawn(int direction, int orderCount)
    {
        yield return new WaitForSeconds(Random.Range(1f,5f));
        Vector3 spawnPos = leftSpawn.position;
        if (direction == 0)
        {
            spawnPos = rightSpawn.position;
        }
        else if (direction == 1)
        {
            spawnPos = leftSpawn.position;
        }
        GameObject c = Instantiate(customer);
        c.transform.position = spawnPos;
        c.GetComponent<Rowboat>().Init(orderCount*30f,direction,orderCount);
    }
}
