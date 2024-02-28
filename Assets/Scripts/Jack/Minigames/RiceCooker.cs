using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RiceCooker : Station
{
	SpriteRenderer riceRenderer;
	CookingStage stage;
	public Item goodRice;
	public Item burntRice;
	[SerializeField] bool cooking;

	public float riceTimer;
	float timeUntilCooked = 5;
	float timeUntilBurned = 10;

	public Transform progressBar;
	public Transform middle;
	public Vector2 _initPos;
	public float maxScale;

	private void Awake()
	{
		_initPos = progressBar.transform.localPosition;
		riceRenderer = progressBar.gameObject.GetComponent<SpriteRenderer>();
		riceRenderer.enabled = false;
	}
	public override bool OnItemAdd(Item item)
	{

		Debug.Log(item.tags[0]);
		if (!(item.tags.Contains(Item.ItemTags.Rice) && item.tags.Contains(Item.ItemTags.Ingredient))) return false;

		bool canAdd = base.OnItemAdd(item);
		cooking = true;
		riceRenderer.enabled = true;
		riceRenderer.color = Color.yellow;
		riceTimer = 0;
		return canAdd;
	}
	public override void ReturnItem(Item item)
	{
		base.ReturnItem(item);
		cooking = true;
		riceRenderer.enabled = true;
		if (item.itemName == "rice") {
			riceRenderer.color = Color.green;
		}
		else {
			riceRenderer.color = Color.red;
		}

	}
	private void Update()
	{
		DrawProgressBar();
		if (!cooking) {
			return;
		}
		riceTimer += Time.deltaTime;
		if (riceTimer < timeUntilCooked) {
			if (stage != CookingStage.uncooked) {
				stage = CookingStage.uncooked;
				riceRenderer.color = Color.yellow;
			}
		}
		else if (riceTimer < timeUntilBurned)
		{
			if (stage != CookingStage.ready)
			{
				stage = CookingStage.ready;
				itemOnStation = goodRice;
				riceRenderer.color = Color.green;
			}
		}
		else {
			if (stage != CookingStage.burnt)
			{
				stage = CookingStage.burnt;
				itemOnStation = burntRice;
				riceRenderer.color = Color.red;
			}
		}
	}
	void DrawProgressBar() {
		float p = riceTimer / timeUntilCooked;
		if (p > 1) p = 1;
		progressBar.localScale = new Vector3(p * (maxScale), 0.51f, 1);
		progressBar.localPosition = new Vector3(
			Mathf.Lerp(_initPos.x, middle.localPosition.x, p),
			_initPos.y, 0);
    }

	
	public override bool OnColliderClicked()
	{
		if(riceTimer > timeUntilCooked || riceTimer == 0) {
			return base.OnColliderClicked();
		}
		return false;
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
