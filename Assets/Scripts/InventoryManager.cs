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
    }

    public List<Item> items;

    public GameObject inventory;
    
    public void AddItem(Item item)
    {
        items.Add(item);
        UpdateInventoryUI();
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            inventory.transform.GetChild(i).GetComponent<InventorySlot>().itemOnStation = null;
			inventory.transform.GetChild(i).GetComponent<InventorySlot>().item = null;

			if (i < items.Count)
            {
                //inventory.transform.GetChild(i).GetComponent<InventorySlot>().item = items[i];
                inventory.transform.GetChild(i).GetComponent<InventorySlot>().OnItemAdd(items[i]);
				inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = items[i].sprite;
            }
            else
            {
                inventory.transform.GetChild(i).GetComponent<InventorySlot>().item = null;
                inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
    }
    
}
