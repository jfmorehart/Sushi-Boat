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
    float maxTime;

    public int orderCount;
    public Color maxRed;

    public bool finished = false;

    int customer;
    int state;
    SpriteRenderer ren;

    bool steaming;
    public ParticleSystem[] steamers;

    public int singleBubbleTimerLength, doubleBubbleTimerLength;
    // Start is called before the first frame update
    void Start()
    {
        orderCount = Random.Range(1, CustomerSpawner.Instance.maxOrderCountPerPerson+1);
        ren = GetComponent<SpriteRenderer>();
        ren.material.SetFloat("_rh", 0);
		if (GameManager.Instance.boss) return;
		customer = Random.Range(0, 10);
        ren.sprite = CustomerSpawner.Instance.GetCustomerState(customer, state);
    }

    void StartSteam() { 
        foreach(ParticleSystem ps in steamers) {
            ps.Play();
	    }
        steaming = true;
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
                if (!finished) {
					if (GameManager.Instance.boss) return;
                    float l = (timer > maxTime * 0.5f) ? 0 :1 - (timer / (maxTime * 0.5f));
                    if(l > 0) {
						ren.material.SetFloat("_rh", l);
					}
                    if(l > 0.5 && !steaming) {
                        StartSteam();
		            }
					if ((state + 1) < (1 - (timer / maxTime)) * 4)
					{
						state++;
						if (state > 3) state = 3;
						ren.sprite = CustomerSpawner.Instance.GetCustomerState(customer, state);
					}
				}


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
            timer = doubleBubbleTimerLength + Random.Range(-1, 1f); //for variance
			maxTime = timer;
		}
        else
        {
            bubble.SetActive(true);
            bubble.transform.GetChild(0).GetComponent<ThoughtBubble>().Init();
            bubble.SetActive(true);
            orderCount -= 1;
            timer = singleBubbleTimerLength + Random.Range(-1, 1f);//for variance
			maxTime = timer;
        }
    }
    public void SetThoughtBubble()
    {
        //tutorial
        if (GameManager.Instance.tutorial)
        {
            if (leftCustomer)
            {
                bubble.SetActive(true);
                orderCount -= 1;
                timer = 1000f;
                maxTime = timer;
            }
            else
            {
                bubble.SetActive(false);
            }

		}
        else
        {
            if (CustomerSpawner.Instance.doubleOrders&&leftCustomer&&orderCount>=2&&(Random.Range(0, 2) == 0))
            {
                doubleBubble.SetActive(true);
                orderCount -= 2;
                timer = Mathf.Max(CustomerSpawner.Instance.minimumOrderTime, doubleBubbleTimerLength);
				maxTime = timer;
			}
            else
            {
                bubble.SetActive(true);
                orderCount -= 1;
                timer = Mathf.Max(CustomerSpawner.Instance.minimumOrderTime, singleBubbleTimerLength);
				maxTime = timer;
			}
        }

    }
}
