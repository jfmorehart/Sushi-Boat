using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Clickable
{
    public List<Item> itemsOnStation;
    public int maxItems = 1;
    public GameObject DraggablePrefab;
    public void OnItemAdd(Item item)
    {
        itemsOnStation.Add(item);
    }

    public override void OnColliderClicked()
    {
        base.OnColliderClicked();
        GameObject item = Instantiate(DraggablePrefab);
        item.GetComponent<Draggable>().Initialize(this,itemsOnStation[0]);
        item.GetComponent<Draggable>().StartDrag();
    }
}
