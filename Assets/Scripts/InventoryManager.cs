using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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

    public List<InventorySlot> slots;
    
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
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].item = items[i];
                slots[i].GetComponent<Image>().sprite = items[i].sprite;
            }
            else
            {
                slots[i].item = null;
            }
        }
    }
    
}
