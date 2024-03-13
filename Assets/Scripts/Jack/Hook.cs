using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static Hook ins;
    public Transform fishCaughtAnchor;
    
    public KeyCode up;
    public KeyCode down;

	public float reelAccel, velocity, drag;
	float unhookHeight;

    public bool fishOn;
    public Fish onHook;
	float floor;
	public float distFromFloor;

	private AudioSource audioSource;

	public AudioClip hooked;

	public AudioClip fishOutofWater;
	private void Awake()
	{
        ins = this;
        Menu.StartDayAction += ResetPos;
		unhookHeight = transform.position.y;
		audioSource = GetComponent<AudioSource>();
		var f = GameObject.FindGameObjectWithTag("Finish");
		if (f != null)
		{
			floor = f.transform.position.y + distFromFloor;
		}
	}
    void ResetPos() {
        transform.position = new Vector3(transform.position.x, -1, 0);
    }

	// Update is called once per frame
	void Update()
    {
	    if (GameManager.Instance.gameState == GameManager.GameState.DayGoing)
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
		    //sound effect
		    if (dir == 0)
		    {
			    audioSource.Stop();
		    }

		    if (Input.GetKeyDown(up) || Input.GetKeyDown(down))
		    {
			    audioSource.Play();
		    }
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
				    InventoryManager.Instance.AddItem(onHook.data.fishItem);
				    onHook.Despawn();
				    SoundManager.Instance.PlaySoundEffect(fishOutofWater);
			    }
		    }
			//Don't go past floor
			if (transform.position.y < floor + distFromFloor)
			{
				velocity = 0;
				Vector3 pos = transform.position;
				transform.position = new Vector3(pos.x, floor + distFromFloor, pos.z);
			}
		}

    }
}
