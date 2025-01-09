using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Transform inventoryParent; // Tempat di mana item akan ditempatkan

    public void AddItem(Item item)
    {
        items.Add(item);
        Instantiate(item.itemPrefab, inventoryParent);
        Debug.Log("Item ditambahkan: " + item.itemName);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("Item dihapus: " + item.itemName);

            // Hapus GameObject dari inventaris
            foreach (Transform child in inventoryParent)
            {
                if (child.gameObject.name == item.itemPrefab.name)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Item tidak ditemukan di inventaris.");
        }
    }

    public void ListItems()
    {
        foreach (Item item in items)
        {
            Debug.Log("Item: " + item.itemName);
            Debug.Log("Count All Item: " + items.Count);
        }
    }
}
