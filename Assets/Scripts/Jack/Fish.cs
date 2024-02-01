using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    float swimSpeed;

    public int direction;

    FishState state;

	private void Awake()
	{
        state = FishState.Swimming;
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
