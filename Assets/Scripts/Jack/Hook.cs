using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static Hook ins;
    public Transform fishCaughtAnchor;
    
    public KeyCode up;
    public KeyCode down;

    public float reelAccel, velocity, drag, unhookHeight;

    [SerializeField]
    bool fishOn;
    [SerializeField]
    Fish onHook;


	private void Awake()
	{
        ins = this;
	}

	// Update is called once per frame
	void Update()
    { 
        // mouse controls
        //Vector3 mposQuery = Input.mousePosition; //mouse pos in pixel coords
        //mposQuery.z = 10; // distance from camera to geometry
        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mposQuery);

        //Vector2 wpos = transform.position;
        //wpos.y = mousePosition.y;
        //transform.position = wpos;

        //keyboard controls
        float dir = Input.GetKey(up)? 1 : 0  + (Input.GetKey(down)? -1 : 0);

        //Moving the hook
        velocity += (dir * reelAccel * Time.deltaTime);
        velocity *= 1 - Time.deltaTime * drag;
        transform.Translate(Time.deltaTime * velocity * Vector2.up, Space.World);

        //Code to handle unhooking the fish
        if(transform.position.y > unhookHeight) {
            velocity = 0;
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, unhookHeight, pos.z);
            if (fishOn) {
				fishOn = false;
				Destroy(onHook.gameObject);
			}
	    }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if we can catch a fish
		if (!fishOn) { 

            //check if we hit a fish
            if (collision.CompareTag("Fish")) {
                if (collision.TryGetComponent<Fish>(out onHook))
                {
					//Catch the fish
					onHook.Hooked();
                    fishOn = true;
                }
                else {
					Debug.LogError("Non Fish tagged as fish!");
                    return;
				}
	        }
	    }

        //check if its an obstacle or whatever
	}
}
