using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
	public FishData data;
	
	[SerializeField]
	string fishName;
    [SerializeField]
    float swimSpeed;

    public int direction;

    FishState state;


    private SpriteRenderer sr;
    public void Init(FishData fData)
    {
	    data = fData;
	    fishName = data.itemName;
	    swimSpeed = data.swimSpeed;
	    GetComponent<SpriteRenderer>().sprite = data.fishSprite;
    }
	private void Awake()
	{
        state = FishState.Swimming;
        sr = GetComponent<SpriteRenderer>();
	}
	// Update is called once per frame
	void Update()
    {
        switch (state) {
            case FishState.Unassigned:
                break;
            case FishState.Swimming:
                SwimmingUpdate();
                break;
			case FishState.Caught:
                //wiggle
				break;
		}
    }

    public void Hooked() {
        state = FishState.Caught;
        transform.parent = Hook.ins.transform;
        transform.position = Hook.ins.fishCaughtAnchor.position;
    }

    void SwimmingUpdate() {
		transform.Translate(direction * swimSpeed * Time.deltaTime * Vector3.right, Space.World);
		if (direction == 1)
		{
			sr.flipX = true;
		}
		else
		{
			sr.flipX = false;
		}
		if (transform.position.x > FishSpawner.ins.rightBound)
		{
			Destroy(gameObject);
		}
		if (transform.position.x < FishSpawner.ins.leftBound)
		{
			Destroy(gameObject);
		}

	}

    enum FishState{ 
        Unassigned, 
        Swimming, 
        OnTheHook, 
        Caught
    }
}
