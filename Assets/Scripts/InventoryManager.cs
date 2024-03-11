using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        //UpdateInventoryUI();
    }
    

    public GameObject inventory;
    
    public void AddItem(Item item)
    {
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            if (inventory.transform.GetChild(i).GetComponent<Station>().itemOnStation== null)
            {
                inventory.transform.GetChild(i).GetComponent<Station>().OnItemAdd(item);
                break;
            }
        }
        //UpdateInventoryUI();
    }

    /*public void UpdateInventoryUI()
    {
        
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            inventory.transform.GetChild(i).GetComponent<InventorySlot>().itemOnStation = null;
			inventory.transform.GetChild(i).GetComponent<InventorySlot>().item = null;

			if (i < items.Count)
            {
                //inventory.transform.GetChild(i).GetComponent<InventorySlot>().item = items[i];
                inventory.transform.GetChild(i).GetComponent<InventorySlot>().OnItemAdd(items[i]);
				inventory.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = items[i].sprite;
            }
            else
            {
                inventory.transform.GetChild(i).GetComponent<InventorySlot>().item = null;
                inventory.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }*/
    
}
