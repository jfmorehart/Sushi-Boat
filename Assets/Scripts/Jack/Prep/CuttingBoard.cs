using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Station
{

	public AudioClip cuttingBoard;

	public override bool OnItemAdd(ItemInstance item)
	{
		if (item.itemData.tags.Contains(Item.ItemTags.Fish) && item.itemData.tags.Contains(Item.ItemTags.Ingredient)) {
			//SoundManager.Instance.PlaySoundEffect(cuttingBoard);
			//ItemInstance newitem = new ItemInstance(item.itemData.processed, item.quality);
			//transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_qual", item.quality);
			if(itemOnStation == null) {
				PopUp(item);
				if (GetComponent<SquashStretch>())
				{
					GetComponent<SquashStretch>().SS(1.2f);
				}
				return true;
			}
			else {
				UpdateSprite();
				return false;
			}
		}
		else {
			Debug.Log("invalid item " + item.itemData.itemName);
			return false;
		}
	}


	public void PopUp(ItemInstance item)
	{
		GetComponentInParent<SectionCuttingBoard>().cuttingBoardUI.gameObject.SetActive(true);
		GetComponentInParent<SectionCuttingBoard>().cuttingBoardUI.Init(item,this);
	}
	
	public override void SpawnDraggableItem(ItemInstance item)
	{
		base.SpawnDraggableItem(item);
	}

	public override bool OnColliderClicked()
	{
		bool baseHasItem = base.OnColliderClicked();
		if (baseHasItem) return baseHasItem;

		//otherwise we should ask bossman
		if(transform.parent.TryGetComponent(out SectionCuttingBoard scb)){
			scb.EmptyChildClicked();
		}
		return false;
	}
}
