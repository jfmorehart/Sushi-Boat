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
    Vector2 lpos;
    // Start is called before the first frame update
    void Start()
    {
        lpos = transform.localPosition;
        if (leftCustomer) {
            orderCount = 2;// Random.Range(1, CustomerSpawner.Instance.maxOrderCountPerPerson + 1);
        }
        else {
            orderCount = 1;
		}
        
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

    public void ReactToFood(float quality) {
		ren.material.SetFloat("_rh", 0);
		if (GameManager.Instance.boss) return;
		state = Mathf.RoundToInt((1 - quality) * 3);
        state = Mathf.Clamp(state, 0, 3);
		if (state != 3) {
            StopSteam();

		}
        else {
			//ren.material.SetFloat("_rh", ((1 - quality) - 0.5f) / 0.5f);
		}

		ren.sprite = CustomerSpawner.Instance.GetCustomerState(customer, state);
	}
	public void StopSteam()
	{
		foreach (ParticleSystem ps in steamers)
		{
			ps.Stop();
		}
		steaming = false;
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
					if (!GameManager.Instance.boss) {
						//float l = (timer > maxTime * 0.5f) ? 0 : 1 - (timer / (maxTime * 0.5f));
						float l = 1 - (timer / maxTime);
						if (l > 0)
						{
							ren.material.SetFloat("_rh", l);
						}
						if (l > 0.5 && !steaming)
						{
							StartSteam();
						}
						if ((state + 1) < (1 - (timer / maxTime)) * 4)
						{
							state++;
							if (state > 3) state = 3;
							ren.sprite = CustomerSpawner.Instance.GetCustomerState(customer, state);
						}
                        transform.localPosition = lpos + Random.insideUnitCircle * l * l * l* 0.5f;
					}
                    else{
                        float l = 1 - (timer / maxTime);
						ren.material.SetFloat("_rh", l);
						transform.localPosition = lpos + Random.insideUnitCircle * l * l * l * 0.5f;
					}
				}
			}
            else
            {
                if (bubble.activeSelf)
                {
                    ThoughtBubble t = bubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
					t.orderFailed = true;

					//t.transform.parent.gameObject.SetActive(false);
				}
                else if (leftCustomer)
                {
                    if (doubleBubble.activeSelf)
                    {
                        ThoughtBubble t1 = doubleBubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                        ThoughtBubble t2 = doubleBubble.transform.GetChild(1).GetComponent<ThoughtBubble>();
		
						t1.orderFailed = true;
                        t2.orderFailed = true;

						//t1.transform.parent.gameObject.SetActive(false);
						//t2.transform.parent.gameObject.SetActive(false);
					}
                }
            }

            if (bubble.activeSelf)
            {
                Debug.Log("a");
                ThoughtBubble t = bubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                if (t.orderComplete || t.orderFailed)
                {
					Debug.Log("b");
					finished = true;

					if (orderCount<=0)
                    {
                        //finished = true;

					}
                    else
                    {
                        //NextOrder();
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
						finished = true;
						if (orderCount<=0)
                        {
                            //finished = true;

						}
                        else
                        {
                            //NextOrder();
                        }
                    }
                }
            }
            

        }
    }

  //  public void NextOrder()
  //  {
  //      if (leftCustomer)
  //      {
  //          doubleBubble.SetActive(false);
  //      }
  //      bubble.SetActive(false);
  //      if (CustomerSpawner.Instance.doubleOrders&&leftCustomer&&orderCount>=2&&(Random.Range(0, 2) == 0))
  //      {
  //          doubleBubble.SetActive(true);
  //          doubleBubble.transform.GetChild(0).GetComponent<ThoughtBubble>().Init();
  //          doubleBubble.transform.GetChild(1).GetComponent<ThoughtBubble>().Init();
  //          orderCount -= 2;
  //          timer = doubleBubbleTimerLength + Random.Range(-1, 1f); //for variance
		//	maxTime = timer;
		//}
  //      else
  //      {
  //          bubble.SetActive(true);
  //          bubble.transform.GetChild(0).GetComponent<ThoughtBubble>().Init();\
  //          orderCount -= 1;
  //          timer = singleBubbleTimerLength + Random.Range(-1, 1f);//for variance
		//	maxTime = timer;
  //      }
  //  }
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
            if (CustomerSpawner.Instance.doubleOrders&&leftCustomer&&orderCount>=2) //removed randomness
            {
                doubleBubble.SetActive(true);
				bubble.SetActive(false);
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
