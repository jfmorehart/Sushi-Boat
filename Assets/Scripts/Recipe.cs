using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public Sprite recipeSprie;
    public string recipeName;
    public List<Item> ingredients;
    public int price;
}
