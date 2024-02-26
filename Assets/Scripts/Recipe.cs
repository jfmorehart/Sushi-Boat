using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public Item recipeItem;
    public List<Item> ingredients;
    public int price;
}
