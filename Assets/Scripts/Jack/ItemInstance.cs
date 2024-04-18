using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
	public Item itemData;
	public Sprite uniqueSprite;
	public float quality;

	//public ItemInstance(Item item, float q) {
	//	itemData = item;
	//	quality = q;
 //   }

	public ItemInstance(Item item, float q, Sprite sp)
	{
		itemData = item;
		quality = q;
		uniqueSprite = sp;
	}
}
