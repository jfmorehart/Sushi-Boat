using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Draggable : Clickable
{
	public Item item;
	public List<Item> items;
	public bool beingDragged;
	public bool onStation;

	Station prevStation;
	Station hoveringOver;
	SpriteRenderer ren;

	private void Awake()
	{
		ren = GetComponent<SpriteRenderer>();
	}
	public void Initialize(Station st, Item it) {
		prevStation = st;
		item = it;
		ren.sprite = it.sprite;

		if (beingDragged)
		{
			transform.position = Selection.instance.mouseWorldPosition;
		}
	}
	public void Initialize(Station st, Item it,List<Item> components) {
		prevStation = st;
		item = it;
		ren.sprite = it.sprite;
		items = new List<Item>(components);

		if (beingDragged)
		{
			transform.position = Selection.instance.mouseWorldPosition;
		}
	}

	private void Update()
	{
		if (!Input.GetMouseButton(0) && beingDragged) {
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
		if (!beingDragged) return;

		if (hoveringOver != null)
		{
			if (hoveringOver.OnItemAdd(item)) {
				prevStation = hoveringOver;
				hoveringOver = null;
				Destroy(gameObject);
			}
			else {
				ReturnToLastStation();
			}
		}
		else {
			
			ReturnToLastStation();
		}
    }
	void ReturnToLastStation() {
		
		if (prevStation.GetComponent<PrepStation>())
		{
			prevStation.GetComponent<PrepStation>().ReturnItem(items);
		}
		else
		{
			prevStation.ReturnItem(item);
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!beingDragged) return;
		if(collision.TryGetComponent(out Station stat)) {
			hoveringOver = stat;
			//if(hoveringOver != null) {
			//	Debug.Log(stat is ThoughtBubble);
			//	if(!(stat is ThoughtBubble)){

			//		return;
			//	}
			//}
			//else {
			//	hoveringOver = stat;
			//}
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
	public override bool OnColliderClicked()
	{
		StartDrag();
		return base.OnColliderClicked();
	}
}
