using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public bool leftCustomer = false;
    public GameObject bubble;
    public GameObject doubleBubble;

    private bool ready = false;

    public float timer = 0;

    public int orderCount;

    public bool finished = false;
    
    // Start is called before the first frame update
    void Start()
    {
        orderCount = Random.Range(1, CustomerSpawner.Instance.maxOrderCountPerPerson+1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<CustomerBoat>().ready&& !ready)
        {
            ready = true;
            SetThoughtBubble();
        }

        if (ready)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (bubble.activeSelf)
                {
                    ThoughtBubble t = bubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                    t.orderFailed = true;
                }
                else if (leftCustomer)
                {
                    if (doubleBubble.activeSelf)
                    {
                        ThoughtBubble t1 = doubleBubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                        ThoughtBubble t2 = doubleBubble.transform.GetChild(1).GetComponent<ThoughtBubble>();
                        t1.orderFailed = true;
                        t2.orderFailed = true;
                    }
                }
            }

            if (bubble.activeSelf)
            {
                ThoughtBubble t = bubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                if (t.orderComplete || t.orderFailed)
                {
                    if (orderCount<=0)
                    {
                        finished = true;
                    }
                    else
                    {
                        NextOrder();
                    }
                }
            }
            else if (leftCustomer)
            {
                if (doubleBubble.activeSelf)
                {
                    ThoughtBubble t1 = doubleBubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                    ThoughtBubble t2 = doubleBubble.transform.GetChild(1).GetComponent<ThoughtBubble>();
                    if ((t1.orderComplete || t1.orderFailed)&&(t2.orderComplete || t2.orderFailed))
                    {
                        if (orderCount<=0)
                        {
                            finished = true;
                        }
                        else
                        {
                            NextOrder();
                        }
                    }
                }
            }
            

        }
    }

    public void NextOrder()
    {
        if (leftCustomer)
        {
            doubleBubble.SetActive(false);
        }
        bubble.SetActive(false);
        if (CustomerSpawner.Instance.doubleOrders&&leftCustomer&&orderCount>=2&&(Random.Range(0, 2) == 0))
        {
            doubleBubble.SetActive(true);
            doubleBubble.transform.GetChild(0).GetComponent<ThoughtBubble>().Init();
            doubleBubble.transform.GetChild(1).GetComponent<ThoughtBubble>().Init();
            orderCount -= 2;
            timer = 60f;
        }
        else
        {
            bubble.SetActive(true);
            bubble.transform.GetChild(0).GetComponent<ThoughtBubble>().Init();
            bubble.SetActive(true);
            orderCount -= 1;
            timer = 30f;
        }
    }
    public void SetThoughtBubble()
    {
        //tutorial
        if (GameManager.Instance.tutorial)
        {
            bubble.SetActive(true);
            orderCount -= 1;
            timer = 1000f;
        }
        else
        {
            if (CustomerSpawner.Instance.doubleOrders&&leftCustomer&&orderCount>=2&&(Random.Range(0, 2) == 0))
            {
                doubleBubble.SetActive(true);
                orderCount -= 2;
                timer = CustomerSpawner.Instance.minimumOrderTime*1.5f;
            }
            else
            {
                bubble.SetActive(true);
                orderCount -= 1;
                timer = CustomerSpawner.Instance.minimumOrderTime;
            }
        }

    }
}