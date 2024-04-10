using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCuttingBoard : Station
{
	public Station[] stations;
	public bool recieving;

	Collider2D col;
	bool waitframe = true;

	public override void Awake()
	{
		base.Awake();
		col = GetComponent<Collider2D>();
		Disable();
	}
	private void Update()
	{
		if (recieving && !Input.GetMouseButton(0)){
			if (waitframe) {
				waitframe = false;
			}
			else {
				Disable();
			}
		}
		else if(!recieving && Input.GetMouseButton(0)){
			if (waitframe)
			{
				waitframe = false;
			}
			else
			{
				Enable();
			}
		}
	}

	public void EmptyChildClicked() {
		//this function is called by one of the board sections
		// in the event that it is clicked but has nothing on it
		// so we check the other squares, in case one of them has one
		// and only give them that one if theres only one, otherwise we could give the wrong one
		int itemcount = 0;
		Station hasItem = null;

		foreach (Station s in stations)
		{
			if(s.itemOnStation != null) {
				itemcount++;
				hasItem = s;
			}
		}
		if (itemcount == 1) {
			hasItem.OnColliderClicked();
		}
	}

	void Enable()
	{
		waitframe = true;
		recieving = true;
		col.enabled = true;
		foreach (Station s in stations)
		{
			s.GetComponent<Collider2D>().enabled = false;
		}
	}

	void Disable() {
		waitframe = true;
		recieving = false;
		col.enabled = false;
		foreach(Station s in stations) {
			s.GetComponent<Collider2D>().enabled = true;
		}
    }

	public override bool OnItemAdd(ItemInstance item)
	{
		foreach(Station s in stations) {
			if (s.OnItemAdd(item)) {
				return true;
			}
		}
		return false;
	}
}
