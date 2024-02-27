using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Clickable
{
    public bool isSpawner;

    public Item itemOnStation;
    public int maxItems = 1;
    public GameObject DraggablePrefab;


    public virtual bool OnItemAdd(Item item)
    {
        if(itemOnStation == null) {
			itemOnStation = item;
			return true;
		}
        else {
            return false;
	    }
    }

    public override bool OnColliderClicked()
    {
        if(itemOnStation== null) {
            return false;
	    }
        base.OnColliderClicked();
        SpawnDraggableItem(itemOnStation);
        return true;
    }
    public virtual void SpawnDraggableItem(Item item) {
		GameObject drag = Instantiate(DraggablePrefab);
		drag.GetComponent<Draggable>().StartDrag();
		drag.GetComponent<Draggable>().Initialize(this, item);
		if (!isSpawner)
		{
			itemOnStation = null;
		}
	}

    public virtual void ReturnItem(Item item) {
        itemOnStation = item;
    }
}
