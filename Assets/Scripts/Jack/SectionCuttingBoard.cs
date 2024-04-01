using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCuttingBoard : Station
{
	public Station[] stations;
	public bool recieving;

	Collider2D col;
	bool waitframe = true;

	private void Awake()
	{
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

	public override bool OnItemAdd(Item item)
	{
		foreach(Station s in stations) {
			if (s.OnItemAdd(item)) {
				return true;
			}
		}
		return false;
	}
}
