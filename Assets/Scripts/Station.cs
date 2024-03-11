using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Station : Clickable
{
    public bool isSpawner;

    public Item itemOnStation;
    public int maxItems = 1;
    public GameObject DraggablePrefab;
    public SpriteRenderer onStation;
    
	public virtual void Start()
	{
		UpdateSprite();
	}
	public virtual bool OnItemAdd(Item item)
    {
        if(itemOnStation == null) {
			itemOnStation = item;
            UpdateSprite();
			return true;
		}
        else {
            UpdateSprite();
            return false;
	    }
    }

    public virtual void UpdateSprite() {
        if (onStation == null) return;
        if(itemOnStation == null) {
            onStation.sprite = null;
            return;
	    }
        onStation.sprite = itemOnStation.sprite;
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

		if (item.pickUpSound != null)
		{
			SoundManager.Instance.PlaySoundEffect(item.pickUpSound);
		}
		else
		{
			SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.defaultPickupSound);
		}

        UpdateSprite();
	}

    public virtual void ReturnItem(Item item) {
        itemOnStation = item;
        UpdateSprite();
    }
}
