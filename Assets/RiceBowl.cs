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
    }

    public override bool OnItemAdd(Item item)
    {
        if(itemOnStation == null&& item.tags.Contains(Item.ItemTags.Rice)&&item.tags.Contains(Item.ItemTags.Combinable)) {
            itemOnStation = item;
            UpdateSprite();
            return true;
        }
        else {
            UpdateSprite();
            return false;
        }
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
