using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
	public Item itemData;
	public float quality;

	public ItemInstance(Item item, float q) {
		itemData = item;
		quality = q;

    }
}
