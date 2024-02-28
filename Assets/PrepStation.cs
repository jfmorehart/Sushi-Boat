using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        return true;
    }
    public Item GetCurrentPreppedItem()
    {
        for (int i = 0; i < validRecipes.Count; i++)
        {
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
        currentItems = new List<Item>(list);
    }
}
