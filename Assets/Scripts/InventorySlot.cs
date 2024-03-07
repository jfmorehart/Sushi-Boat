using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Station, IPointerDownHandler
{
    public Item item;
	

	public override bool OnItemAdd(Item newItem)
	{
		bool ret = base.OnItemAdd(newItem);
		if (ret) {
			item = newItem;
		}
		return ret;
	}

	public override void ReturnItem(Item newitem)
	{
		Debug.Log("returning " + newitem.itemName);
		InventoryManager.Instance.AddItem(newitem);
		InventoryManager.Instance.UpdateInventoryUI();
		//item = newitem;
		//transform.GetChild(0).GetComponent<Image>().sprite = newitem.sprite;
		//base.ReturnItem(newitem);
	}

	public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            if (OnColliderClicked()) {
				InventoryManager.Instance.RemoveItem(item);
				item = null;
				InventoryManager.Instance.UpdateInventoryUI();
			}
        }
    }
}
