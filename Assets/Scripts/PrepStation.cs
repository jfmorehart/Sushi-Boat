using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PrepStation : Station
{
    public List<Recipe> validRecipes;

    public List<ItemInstance> currentItems;
    public int maxItemCount = 3;
    public Item incompleteFood;

	public override void Awake()
	{
        base.Awake();
        currentItems = new List<ItemInstance>();
	}
	public override bool OnItemAdd(ItemInstance item)
    {
        if(currentItems.Count < maxItemCount&& item.itemData.tags.Contains(Item.ItemTags.Combinable)) {
            currentItems.Add(item);
            UpdateSprite();
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.defaultPickupSound);
            if (GetComponent<SquashStretch>())
            {
                GetComponent<SquashStretch>().SS(1.2f);
            }
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

        ItemInstance ii = GetCurrentPreppedItem();

		onStation.sprite = ii.itemData.sprite;
        onStation.GetComponent<Renderer>().material.SetFloat("_qual", ii.quality);
	}
    public ItemInstance GetCurrentPreppedItem()
    {
        if (currentItems.Count == 1)
        {
            return currentItems[0];
        }
        Item it = null;
        float qual = 0;
        for (int i = 0; i < currentItems.Count; i++)
        {
            qual += currentItems[i].quality;
        }
        qual /= currentItems.Count;
		for (int i = 0; i < validRecipes.Count; i++)
        {
            if (CheckRecipe(validRecipes[i]))
            {
               it = validRecipes[i].recipeItem;
			    return new ItemInstance(it, qual, it.sprite);
			}
            
        }
        return new ItemInstance(incompleteFood, 0, incompleteFood.sprite);
    }

    public bool CheckRecipe(Recipe recipe)
    {
        if (currentItems.Count <= 1)
        {
            return false;
        }
        List<Item> currentItemData = new List<Item>();
        for(int i = 0; i < currentItems.Count; i++) {
            currentItemData.Add(currentItems[i].itemData);
	    }
        return Enumerable.SequenceEqual(recipe.ingredients.OrderBy(e => e), currentItemData.OrderBy(e => e));
    }
    
    public override void SpawnDraggableItem(ItemInstance item) {
        GameObject drag = Instantiate(DraggablePrefab);
        drag.GetComponent<Draggable>().StartDrag();
        drag.GetComponent<Draggable>().Initialize(this, item, currentItems);
        if (item.itemData.pickUpSound != null)
        {
            SoundManager.Instance.PlaySoundEffect(item.itemData.pickUpSound);
        }
        else
        {
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.defaultPickupSound);
        }
        if (!isSpawner)
        {
            currentItems.Clear();
            itemOnStation = null;
        }
    }
    public void ReturnItem(List<ItemInstance> list)
    {
        currentItems.Clear();
        foreach (var item in list)
        {
            currentItems.Add(item);
        }
        UpdateSprite();
    }
}
