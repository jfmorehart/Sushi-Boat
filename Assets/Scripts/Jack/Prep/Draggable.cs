using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Draggable : Clickable
{
	public Item item;
	public bool beingDragged;
	public bool onStation;

	Station prevStation;
	Station hoveringOver;

	public void Initialize(Station st, Item it) {
		prevStation = st;
		item = it;
    }

	private void Update()
	{
		if (Input.GetMouseButtonUp(0)) {
			EndDrag();
		}

		if (beingDragged) {
			transform.position = Selection.instance.mouseWorldPosition;
		}
	}
	public void StartDrag() {
		beingDragged = true;
    }
	public void EndDrag() {
		if (hoveringOver != null)
		{
			hoveringOver.OnItemAdd(item);
			prevStation = hoveringOver;
			hoveringOver = null;
			Destroy(gameObject);
		}
		else {
			prevStation.OnItemAdd(item);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!beingDragged) return;
		if(collision.TryGetComponent(out Station stat)) {
			hoveringOver = stat;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!beingDragged) return;
		if (collision.TryGetComponent(out Station stat))
		{
			hoveringOver = null;
		}
	}
	public override void OnColliderClicked()
	{
		StartDrag();
	}
}
