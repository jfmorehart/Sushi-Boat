using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RiceCooker : Station
{
	public SpriteRenderer riceRenderer;
	CookingStage stage;
	public Item goodRice;
	public Item burntRice;
	[SerializeField] bool cooking;
	
	public float riceTimer;
	float timeUntilCooked = 5;
	float timeUntilBurned = 10;

	private void Awake()
	{
		riceRenderer.enabled = false;
	}
	public override bool OnItemAdd(Item item)
	{
		if (item.itemName != "uncooked rice") return false;
		bool canAdd = base.OnItemAdd(item);
		cooking = true;
		riceRenderer.enabled = true;
		riceRenderer.color = Color.grey;
		riceTimer = 0;
		return canAdd;
	}
	public override void ReturnItem(Item item)
	{
		base.ReturnItem(item);
		cooking = true;
		riceRenderer.enabled = true;
		if (item.itemName == "rice") {
			riceRenderer.color = Color.white;
		}
		else {
			riceRenderer.color = Color.black;
		}

	}
	private void Update()
	{
		if (!cooking) {
			return;
		}
		riceTimer += Time.deltaTime;
		if(riceTimer < timeUntilCooked) { 
			if(stage != CookingStage.uncooked) {
				stage = CookingStage.uncooked;
				riceRenderer.color = Color.grey;
			}
		}
		else if (riceTimer < timeUntilBurned)
		{
			if (stage != CookingStage.ready)
			{
				stage = CookingStage.ready;
				itemOnStation = goodRice;
				riceRenderer.color = Color.white;
			}
		}
		else {
			if (stage != CookingStage.burnt)
			{
				stage = CookingStage.burnt;
				itemOnStation = burntRice;
				riceRenderer.color = Color.black;
			}
		}
	}
	public override void OnColliderClicked()
	{
		if(riceTimer > timeUntilCooked || riceTimer == 0) {
			base.OnColliderClicked();
		}

	}
	public override void SpawnDraggableItem(Item item)
	{
		base.SpawnDraggableItem(item);
		cooking = false;
		riceRenderer.enabled = false;
	}

	enum CookingStage { 
		uncooked, 
		ready, 
		burnt
    }
}
