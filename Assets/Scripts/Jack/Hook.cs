using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hook : MonoBehaviour
{
    public static Hook ins;
    public Transform fishCaughtAnchor;
	public bool active;
    
    public KeyCode up;
    public KeyCode down;

	public bool goinDown;

	public float reelAccel, velocity, drag;
	float unhookHeight;

    public bool fishOn;
    public Fish onHook;
	float floor;
	public float distFromFloor;

	private AudioSource audioSource;

	public AudioClip hooked;

	public AudioClip fishOutofWater;
	FishAnimInfo fishAnim;
	float xorigin;


	private CoolerSlot[] slots;
	public AudioClip coolerFull;
	private void Awake()
	{
		xorigin = transform.position.x;

		fishAnim = FindObjectOfType<FishAnimInfo>(false);
		ins = this;
        Menu.StartDayAction += ResetPos;
		unhookHeight = transform.position.y;
		audioSource = GetComponent<AudioSource>();
		var f = GameObject.FindGameObjectWithTag("Finish");
		if (f != null)
		{
			floor = f.transform.position.y + distFromFloor;
		}
		slots = (CoolerSlot[])FindObjectsByType(typeof(CoolerSlot), FindObjectsSortMode.None);
	}
    void ResetPos() {
        transform.position = new Vector3(transform.position.x, -1, 0);
    }

	// Update is called once per frame
	void Update()
    {
		//if (!active) return;
		transform.position = new Vector3(xorigin, transform.position.y, 0);
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
			//float dir = Input.GetKey(up)? 1 : 0  + (Input.GetKey(down)? -1 : 0);
			////sound effect
			//if (dir == 0)
			//{
			// audioSource.Stop();
			//}

			//if (Input.GetKeyDown(up) || Input.GetKeyDown(down))
			//{
			// audioSource.Play();
			//}
			////Moving the hook
			//velocity += (dir * reelAccel * Time.deltaTime);
			//velocity *= 1 - Time.deltaTime * drag;
			//transform.Translate(Time.deltaTime * velocity * Vector2.up, Space.World);

			Vector2 pnaught = transform.position;

			float dir = goinDown ? -1 : 1;

			if (fishOn) {
				dir = 2;
			}

			if (Input.GetMouseButtonDown(0) && active) {
				goinDown = !goinDown;
				audioSource.Play();
			}
			velocity += (dir * reelAccel * Time.deltaTime);
			velocity *= 1 - Time.deltaTime * drag;
			transform.Translate(Time.deltaTime * velocity * Vector2.up, Space.World);

			//Code to handle unhooking the fish
			if (transform.position.y > unhookHeight) {
			    velocity = 0;
				audioSource.Stop();
				Vector3 pos = transform.position;
			    transform.position = new Vector3(pos.x, unhookHeight, pos.z);
			    if (fishOn) {
				    fishOn = false;
					Sprite sp = onHook.GetComponent<SpriteRenderer>().sprite;
				    InventoryManager.Instance.AddItem(new ItemInstance(onHook.data.fishItem, onHook.quality, sp));
				    onHook.Despawn();
				    //SoundManager.Instance.PlaySoundEffect(fishOutofWater);
				    if(CoolerIsFull())
					    SoundManager.Instance.PlaySoundEffect(coolerFull);
				    else
				    {
					    SoundManager.Instance.PlaySoundEffect(fishOutofWater);
				    }
					goinDown = false;
					audioSource.Play();

				}
		    }
			//Don't go past floor
			if (transform.position.y < floor + distFromFloor){
				//Debug.Log(transform.position.y + " " + floor + distFromFloor + " " + distFromFloor);
				velocity = 0;
				Vector3 pos = transform.position;
				transform.position = new Vector3(pos.x, floor + distFromFloor, pos.z);
				audioSource.Stop();
			}

			if (Vector2.Distance(pnaught, transform.position) > 0.01f && !audioSource.isPlaying) {
				audioSource.Play();
			}
		}

    }
	bool CoolerIsFull() { 
		foreach(CoolerSlot cs in slots) { 
			if(cs.itemOnStation == null) {
				return false;
			}
		}
		return true;
	}
}
