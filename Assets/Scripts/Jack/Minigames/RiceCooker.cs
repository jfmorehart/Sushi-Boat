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
	public bool cooking;

	public float riceTimer;
	public float minUsableTime = 1;
	public float timeUntilCooked = 5;
	public float gracePeriod;
	public float timeUntilBurned = 10;

	public Transform progressBar;
	public Transform middle;
	public Vector2 _initPos;
	public float maxScale;

	private AudioSource audioSource;
	public AudioClip doneCooking;
	public AudioClip overCooked;

	bool beep;


	public override void Awake()
	{
		base.Awake();
		_initPos = progressBar.transform.localPosition;
		riceRenderer = progressBar.gameObject.GetComponent<SpriteRenderer>();
		riceRenderer.enabled = false;
		audioSource = GetComponent<AudioSource>();
	}
	public override bool OnItemAdd(ItemInstance item)
	{
		if (!(item.itemData.tags.Contains(Item.ItemTags.Rice) && item.itemData.tags.Contains(Item.ItemTags.Ingredient))) return false;

		bool canAdd = base.OnItemAdd(item);
		if (canAdd) {
			cooking = true;
			riceRenderer.enabled = true;
			riceRenderer.color = Color.yellow;
			riceTimer = 0;
			audioSource.Play();
			beep = false;
		}
		return canAdd;
	}
	public override void ReturnItem(ItemInstance item)
	{
		base.ReturnItem(item);
		cooking = true;
		riceRenderer.enabled = true;
		if (item.itemData.tags.Contains(Item.ItemTags.Combinable)) {
			riceRenderer.color = Color.green;
		}
		else {
			riceRenderer.color = Color.red;
		}

	}
	private void Update()
	{

		if (!cooking) {
			audioSource.Stop();
			return;
		}
		DrawProgressBar();
		riceTimer += Time.deltaTime;
		if (riceTimer > timeUntilCooked && riceTimer < timeUntilCooked + gracePeriod) {
			if (!beep) {
				audioSource.PlayOneShot(doneCooking);
				beep = true;
			}
			return;
		}
		if (riceTimer < minUsableTime)
		{
			if (stage != CookingStage.uncooked)
			{
				stage = CookingStage.uncooked;
			}
		}
		else if (riceTimer < timeUntilCooked)
		{
			if (stage != CookingStage.ready)
			{
				stage = CookingStage.ready;
				itemOnStation = new ItemInstance(goodRice, itemOnStation.quality);
			}
			float p = riceTimer / timeUntilCooked;
			riceRenderer.color = Vector4.Lerp((Vector4)Color.yellow, ((Vector4)Color.green), p);
		}
		else if (riceTimer < timeUntilBurned + gracePeriod)
		{
			float p = (riceTimer - (timeUntilCooked + gracePeriod)) / (timeUntilBurned - (timeUntilCooked + gracePeriod));
			riceRenderer.color = Vector4.Lerp((Vector4)Color.green, ((Vector4)Color.red), p);
			if (stage != CookingStage.ready)
			{
				stage = CookingStage.ready;
				itemOnStation = new ItemInstance(goodRice, itemOnStation.quality);
				//riceRenderer.color = Color.green;
			}
		}
		else
		{
			if (stage != CookingStage.burnt)
			{
				stage = CookingStage.burnt;
				//itemOnStation = new ItemInstance(burntRice, itemOnStation.quality);
				riceRenderer.color = Color.red;
				audioSource.Stop();
				audioSource.PlayOneShot(overCooked);
			}
		}
	}
	void DrawProgressBar() {
		float p;
		if (riceTimer < timeUntilCooked) {
			p = riceTimer / timeUntilBurned;
		}
		else if(riceTimer > timeUntilCooked + gracePeriod) {
			p = ((riceTimer - gracePeriod) / (timeUntilBurned));
		}
		else {
			p = 0.5f;
		}
		if (p > 1) p = 1;
		itemOnStation.quality = 1 - Mathf.Abs(2 * (p - 0.5f));
		if (p > 1) p = 1;
		progressBar.localScale = new Vector3(p * (maxScale), 0.51f, 1);
		progressBar.localPosition = new Vector3(
			Mathf.Lerp(_initPos.x, middle.localPosition.x, p),
			_initPos.y, 0);
    }

	
	public override bool OnColliderClicked()
	{
		if(riceTimer > minUsableTime || riceTimer == 0) {
			return base.OnColliderClicked();
		}
		return false;
	}
	public override void SpawnDraggableItem(ItemInstance item)
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
