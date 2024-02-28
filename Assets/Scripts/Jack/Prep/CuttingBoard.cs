using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Station
{
	public Item cutfish;

	public override bool OnItemAdd(Item item)
	{
		Debug.Log(item + " " + item.itemName);
		if (item.tags.Contains(Item.ItemTags.Fish) && item.tags.Contains(Item.ItemTags.Ingredient)) {
			cutfish.itemName = item.itemName;
			return base.OnItemAdd(cutfish);
		}
		else {
			return false;
		}
	}

	public override void SpawnDraggableItem(Item item)
	{
		base.SpawnDraggableItem(item);
		item.tags.Clear();
		item.tags.Add(Item.ItemTags.Fish);
		item.tags.Add(Item.ItemTags.Combinable);
	}
}
