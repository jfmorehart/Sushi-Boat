using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

	public LayerMask hookmask;

    public void Init(FishData fData)
    {
	    data = fData;
	    //fishName = data.fishItem.itemName;
	    swimSpeed = data.swimSpeed;
	    GetComponent<SpriteRenderer>().sprite = data.fishSprite;
    }
	private void Awake()
	{
        state = FishState.Swimming;
        sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;
		Menu.EndDayAction += Despawn;
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
	public void Despawn() {
		Menu.EndDayAction -= Despawn;
		if(gameObject != null) {
			Destroy(gameObject);
		}
	}

    public void Hooked() {
		if (sr.flipX) {
			sr.flipX = false;
			sr.flipY = true;
		}

        state = FishState.Caught;
        transform.parent = Hook.ins.transform;

        transform.position = Hook.ins.fishCaughtAnchor.position;
		transform.localPosition = transform.localPosition - data.mouthDist * Vector3.up;
		transform.localEulerAngles = new Vector3(0, 0, -90);
		Hook.ins.fishOn = true;
		Hook.ins.onHook = this;
    }

    void SwimmingUpdate() {
		if(!sr.enabled)sr.enabled = true;
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
			Despawn();
		}
		if (transform.position.x < FishSpawner.ins.leftBound)
		{
			Despawn();
		}

		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction * Vector2.right, 1);
		if (hit && !Hook.ins.fishOn) {
			if (hit.collider.CompareTag("Hook")) {
				Hooked();
			}
		}
	}

    enum FishState{ 
        Unassigned, 
        Swimming, 
        OnTheHook, 
        Caught
    }
}
