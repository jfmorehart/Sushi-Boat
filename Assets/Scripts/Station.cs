using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Clickable
{
    public bool isSpawner;

    public Item itemOnStation;
    public int maxItems = 1;
    public GameObject DraggablePrefab;
    public virtual void OnItemAdd(Item item)
    {
        itemOnStation = item;
    }

    public override void OnColliderClicked()
    {
        if(itemOnStation== null) {
            return;
	    }
        base.OnColliderClicked();
        GameObject item = Instantiate(DraggablePrefab);
        item.GetComponent<Draggable>().StartDrag();
		item.GetComponent<Draggable>().Initialize(this, itemOnStation);
        if (!isSpawner) {
			itemOnStation = null;
		}
 
    }
}
