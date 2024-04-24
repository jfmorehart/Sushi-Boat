using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Station : Clickable
{
    public bool isSpawner;

	public Item toSpawn;//
    public ItemInstance itemOnStation;
    public int maxItems = 1;
    public GameObject DraggablePrefab;
    public SpriteRenderer onStation;

    public GameObject blinker;

    
	public virtual void Start()
	{
		UpdateSprite();
		if (toSpawn != null) {
			itemOnStation = new ItemInstance(toSpawn, 1, toSpawn.sprite);
		}
	}

    public void Blink() {
        Invoke(nameof(BlinkOn), 0);
		Invoke(nameof(BlinkOff), 0.25f);
		Invoke(nameof(BlinkOn), 0.5f);
		Invoke(nameof(BlinkOff), 0.75f);
		Invoke(nameof(BlinkOn), 1f);
		Invoke(nameof(BlinkOff), 1.25f);
	}

    public void BlinkOn() {
        blinker.SetActive(true);
    }
	public void BlinkOff()
	{
		blinker.SetActive(false);
	}


	public virtual bool OnItemAdd(ItemInstance item)
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
		onStation.sprite = itemOnStation.uniqueSprite;// itemOnStation.itemData.sprite;

		if (onStation != null)
		{
			onStation.material.SetFloat("_qual", itemOnStation.quality);
		}
	}

    public override bool OnColliderClicked()
    {
		Debug.Log("click " + itemOnStation);
		if (itemOnStation== null) {
            return false;
	    }
        base.OnColliderClicked();
        SpawnDraggableItem(itemOnStation);
        return true;
    }
    public virtual void SpawnDraggableItem(ItemInstance item) {
	    if (GetComponent<SquashStretch>())
	    {
		    GetComponent<SquashStretch>().SS(1.2f);
	    }
		GameObject drag = Instantiate(DraggablePrefab);
		drag.GetComponent<Draggable>().StartDrag();
		drag.GetComponent<Draggable>().Initialize(this, item);
		if (!isSpawner)
		{
			itemOnStation = null;
		}
		else {

			if (toSpawn != null)
			{
				itemOnStation = new ItemInstance(toSpawn, 1, toSpawn.sprite);
			}
		}

		if (item.itemData.pickUpSound != null)
		{
			SoundManager.Instance.PlaySoundEffect(item.itemData.pickUpSound);
		}
		else
		{
			SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.defaultPickupSound);
		}

        UpdateSprite();
	}

    public virtual void ReturnItem(ItemInstance item) {
        itemOnStation = item;
		Debug.Log(item.quality);
		UpdateSprite();
    }
}
