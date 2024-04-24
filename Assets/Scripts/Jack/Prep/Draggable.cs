using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Draggable : Clickable
{
	public ItemInstance item;
	public List<ItemInstance> items;
	public bool beingDragged;
	public bool onStation;

	Station prevStation;
	SpriteRenderer ren;

	public override void Awake()
	{
		base.Awake();
		ren = GetComponent<SpriteRenderer>();
	}
	public void Initialize(Station st, ItemInstance it) {
		prevStation = st;
		item = it;
		ren.sprite = it.uniqueSprite;
		ren.material.SetFloat("_qual", it.quality);
		if (beingDragged)
		{
			transform.position = Selection.instance.mouseWorldPosition;
		}
		//tutorialcheck
		if(!GameManager.Instance.tutorial)
			AnticipateBlink();
	}
	public void Initialize(Station st, ItemInstance it,List<ItemInstance> components) {
		prevStation = st;
		item = it;
		ren.sprite = it.itemData.sprite;
		ren.material.SetFloat("_qual", it.quality);
		items = new List<ItemInstance>(components);

		if (beingDragged)
		{
			transform.position = Selection.instance.mouseWorldPosition;
		}
		//tutorialcheck
		if(!GameManager.Instance.tutorial)
			AnticipateBlink();
	}

	void AnticipateBlink() {

		GameObject[] gos = GameObject.FindGameObjectsWithTag("Station");

		if (item.itemData.tags.Contains(Item.ItemTags.Fish) && (item.itemData.tags.Contains(Item.ItemTags.Ingredient)))
		{
			foreach (GameObject go in gos)
			{
				if (go.TryGetComponent(out SectionCuttingBoard scb))
				{
					Debug.Log("blinking board");
					scb.Blink();
					return;
				}
			}
		}
		else if (item.itemData.tags.Contains(Item.ItemTags.Rice) && item.itemData.tags.Contains(Item.ItemTags.Ingredient))
		{
			foreach (GameObject go in gos)
			{
				if (go.TryGetComponent(out RiceCooker scb))
				{
					scb.Blink();
					return;
				}
			}
		}else if (item.itemData.tags.Contains(Item.ItemTags.Combinable)) {
			foreach (GameObject go in gos)
			{
				if (go.TryGetComponent(out PrepStation prep))
				{
					prep.Blink();
					return;
				}
			}
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

		Station st = CheckForStation();
		if (st != null)
		{
			if (st.OnItemAdd(item)) {
				prevStation = st;
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

	Station CheckForStation() {
		Vector3 mposQuery = Input.mousePosition; //mouse pos in pixel coords
		mposQuery.z = 10; // distance from camera to geometry
		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mposQuery);

		Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);
		if (hit != null)
		{
			if (hit.TryGetComponent(out Station c))
			{
				return c;	
			}
		}
		return null;
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

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	if (!beingDragged) return;
	//	if(collision.TryGetComponent(out Station stat)) {
	//		hoveringOver = stat;
	//		//if(hoveringOver != null) {
	//		//	Debug.Log(stat is ThoughtBubble);
	//		//	if(!(stat is ThoughtBubble)){

	//		//		return;
	//		//	}
	//		//}
	//		//else {
	//		//	hoveringOver = stat;
	//		//}
	//	}
	//}

	//private void OnTriggerExit2D(Collider2D collision)
	//{
	//	if (!beingDragged) return;
	//	if (collision.TryGetComponent(out Station stat))
	//	{
	//		hoveringOver = null;
	//	}
	//}
	//public override bool OnColliderClicked()
	//{
	//	StartDrag();
	//	return base.OnColliderClicked();
	//}
}
