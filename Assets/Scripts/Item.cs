using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject, IComparable<Item>
{
    public string itemName;
    public Item processed;

    public Sprite sprite;

    public List<ItemTags> tags;

    public AudioClip pickUpSound;

    public List<float> cutPositions;
    public Sprite cuttingBoardSprite;
    

    public float quality;//
    
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
