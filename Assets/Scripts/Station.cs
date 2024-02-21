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

    public override void OnColliderClicked()
    {
        if(itemOnStation== null) {
            return;
	    }
        base.OnColliderClicked();
        SpawnDraggableItem(itemOnStation);
 
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
}
