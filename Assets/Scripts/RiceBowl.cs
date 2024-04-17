using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RiceBowl : Station
{
    public Sprite empty;
    public Sprite full;
    private SpriteRenderer sr;

    public override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
		sr.material.SetFloat("_qual", 1);
	}

    public override bool OnItemAdd(ItemInstance item)
    {
        if(itemOnStation == null&& item.itemData.tags.Contains(Item.ItemTags.Rice)&&item.itemData.tags.Contains(Item.ItemTags.Combinable)) {
            itemOnStation = item;
            sr.material.SetFloat("_qual", item.quality);
            UpdateSprite();
            return true;
        }
        else {
            UpdateSprite();
            return false;
        }
    }


	public override void SpawnDraggableItem(ItemInstance item)
	{
		base.SpawnDraggableItem(item);
		sr.material.SetFloat("_qual", 1);
	}
	public override void UpdateSprite()
    {
        if (itemOnStation == null)
        {
            sr.sprite = empty;
        }
        else
        {
            sr.sprite = full;
        }
    }
}
