using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject, IComparable<Item>
{
    public string itemName;

    public Sprite sprite;

    public List<ItemTags> tags;
    
    public enum ItemTags
    {
        //if the thing is combinable,please just have one of these
        Ingredient,
        Combinable,
        Finished,
        //other tags
        Cookable,
        Rice,
        Fish,
    }

    public Item(Item i) {
        name = i.name;
        itemName = i.itemName;
        sprite = i.sprite;
        tags = new List<ItemTags>();
    }
    


    public void Collect()
    {
        
    }

    public int CompareTo(Item other)
    {
        // If other is not a valid object reference, this instance is greater.
        if (other == null) return 1;

        // The temperature comparison depends on the comparison of
        // the underlying Double values.
        return itemName.CompareTo(other.itemName);
    }
}
