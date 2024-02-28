using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PrepStation : Station
{
    public List<Recipe> validRecipes;

    public List<Item> currentItems;
    public int maxItemCount = 3;
    public Item incompleteFood;

	private void Awake()
	{
        currentItems = new List<Item>();
	}
	public override bool OnItemAdd(Item item)
    {
        if(currentItems.Count < maxItemCount&& item.tags.Contains(Item.ItemTags.Combinable)) {
            currentItems.Add(item);
            UpdateSprite();
            return true;
        }
        else {
			return false;
        }
    }

    public override bool OnColliderClicked()
    {
        if(currentItems.Count==0) {
            return false;
        }
        //base.OnColliderClicked();
        SpawnDraggableItem(GetCurrentPreppedItem());
		UpdateSprite();
		return true;
    }
    public override void UpdateSprite() {
	    if (onStation == null) return;

        if (currentItems.Count < 1)
		{
			onStation.sprite = null;
			return;
        }

        onStation.sprite = GetCurrentPreppedItem().sprite;
	}
    public Item GetCurrentPreppedItem()
    {
        if (currentItems.Count == 1)
        {
            return currentItems[0];
        }
        for (int i = 0; i < validRecipes.Count; i++)
        {
            Debug.Log(CheckRecipe(validRecipes[i]));
            if (CheckRecipe(validRecipes[i]))
            {
                
                return validRecipes[i].recipeItem;
            }
            
        }
        return incompleteFood;
    }

    public bool CheckRecipe(Recipe recipe)
    {
        if (currentItems.Count <= 1)
        {
            return false;
        }
        return Enumerable.SequenceEqual(recipe.ingredients.OrderBy(e => e), currentItems.OrderBy(e => e));
    }
    
    public override void SpawnDraggableItem(Item item) {
        GameObject drag = Instantiate(DraggablePrefab);
        drag.GetComponent<Draggable>().StartDrag();
        drag.GetComponent<Draggable>().Initialize(this, item,currentItems);
        if (!isSpawner)
        {
            currentItems.Clear();
            itemOnStation = null;
        }
    }
    public void ReturnItem(List<Item> list)
    {
        currentItems.Clear();
        foreach (var item in list)
        {
            currentItems.Add(item);
        }
        UpdateSprite();
    }
}
