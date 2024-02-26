using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
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
    
    public void Collect()
    {
        
    }
    
}
