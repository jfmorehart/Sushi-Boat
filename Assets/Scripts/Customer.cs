using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public bool leftCustomer = false;
    public GameObject bubble;
    public GameObject doubleBubble;

    private bool ready = false;

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

    CustomerBoat parentBoat;

    // Start is called before the first frame update
    void Start()
    {
        lpos = transform.localPosition;
        if (leftCustomer) {
            orderCount = Mathf.CeilToInt(CustomerSpawner.Instance.numOrdersThisWave / 2f);
        }
        else {
			orderCount = Mathf.FloorToInt(CustomerSpawner.Instance.numOrdersThisWave / 2f);
		}
        if (orderCount < 1) {
            ready = true;
            gameObject.SetActive(false);
	    }

        Debug.Log(leftCustomer + " " + orderCount + " " + CustomerSpawner.Instance.numOrdersThisWave);
        ren = GetComponent<SpriteRenderer>();
        ren.material.SetFloat("_rh", 0);

		parentBoat = GetComponentInParent<CustomerBoat>();
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
        if (parentBoat.ready&& !ready)
        {
            ready = true;
            SetThoughtBubble();
        }

        if (ready)
        {
            if (parentBoat.timer >= 0)
            {
                if (!finished) {
					if (!GameManager.Instance.boss) {
						float l = Mathf.Max(0, ((1 - (parentBoat.timer / maxTime)) - 0.75f)) * 4;
						if (l > 0)
						{
							ren.material.SetFloat("_rh", l);
						}
						if (l > 0.5 && !steaming)
						{
							StartSteam();
						}
						if ((state + 1) < (1 - (parentBoat.timer / maxTime)) * 4)
						{
							state++;
							if (state > 3) state = 3;
							ren.sprite = CustomerSpawner.Instance.GetCustomerState(customer, state);
						}
                        transform.localPosition = lpos + Random.insideUnitCircle * l * l * l* 0.1f;
					}
                    else{
                        float l = 1 - (parentBoat.timer / maxTime);
						ren.material.SetFloat("_rh", l);
						transform.localPosition = lpos + Random.insideUnitCircle * l * l * l * 0.25f;
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
                ThoughtBubble t = bubble.transform.GetChild(0).GetComponent<ThoughtBubble>();
                if (t.orderComplete || t.orderFailed)
                {
					finished = true;
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
                    }
                }
            }
            

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
                maxTime = parentBoat.timer;
            }
            else
            {
                bubble.SetActive(false);
            }

		}
        else
        {
            if (orderCount == 2)
            {
                doubleBubble.SetActive(true);
				bubble.SetActive(false);
				orderCount -= 2;
				maxTime = parentBoat.timer;
			}
            else if(orderCount == 1)
            {
                bubble.SetActive(true);
                orderCount -= 1;
				maxTime = parentBoat.timer;
			}
        }

    }
}
