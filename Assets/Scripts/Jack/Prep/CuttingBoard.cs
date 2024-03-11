using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Station
{

	public AudioClip cuttingBoard;
	public override bool OnItemAdd(Item item)
	{
		Debug.Log(item + " " + item.itemName);
		if (item.tags.Contains(Item.ItemTags.Fish) && item.tags.Contains(Item.ItemTags.Ingredient)) {
			SoundManager.Instance.PlaySoundEffect(cuttingBoard);
			Item newitem = item.processed;
			return base.OnItemAdd(newitem);
		}
		else {
			Debug.Log("invalid item " + item.itemName);
			return false;
		}
	}

	public override void SpawnDraggableItem(Item item)
	{
		base.SpawnDraggableItem(item);
	}
}
