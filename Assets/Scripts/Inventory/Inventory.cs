using System.Collections.Generic;
using System;
using UnityEngine;

public delegate void InventoryItemAdded(Item item);
public delegate void InventoryItemRemoved(Item item);

public class Inventory
{
    private ItemDatabase _itemDatabase;

    private List<ItemInstance> _indexedInventory = new List<ItemInstance>();

    //public event InventoryItemAdded OnInventoryItemAdded;
    //public event InventoryItemRemoved OnInventoryItemRemoved;

    public Inventory(ItemDatabase itemDatabase)
    {
        if (itemDatabase != null)
            _itemDatabase = itemDatabase;
        else
            Debug.LogError("ItemDatabase was null when given to inventory");
    }

    public void AddItem(Item item)
    {
        //I double commented because I want to keep it for reference. -M
        //// CodeReview:
        ////      This isn't the wrong way to find the last index, but there's an easier way:
        ////      int index = _indexedInventory.FindLastIndex(0, (i) => i.Item == item);
        ////      However, it is will not work correctly if the index of the last item is a full stack and there
        ////          is a partial stack at a lower index.
        //// Instead, do this:
        ////      int index = _indexedInventory.FindIndex((i) => i.Item == item && i.CanAddToStack());
        //// Finally, there's not really any reason to deal with indices in this case:
        ////      ItemInstance item = _indexedInventory.Find((i) => i.Item == item && i.CanAddToStack());
        ////      item will be null if it's not found.

        // So basically, this whole method can be replaced with:
        var itemInstance = _indexedInventory.Find((i) => i.Item == item && i.CanAddToStack());
        if (itemInstance == null)
            _indexedInventory.Add(new ItemInstance(item));
        else
            itemInstance.AddToStack();

    }

    public bool RemoveItem(Item item)
    {

        var itemInstance = _indexedInventory.Find((i) => i.Item == item);
        if (itemInstance == null) return false;
        if (itemInstance.RemoveFromStack() == 0)
            _indexedInventory.Remove(itemInstance);
        return true;
    }

    //Difference is this one will remove no matter the stack number, saves time when there are many of an item?
    public bool CompletelyRemoveItem(Item item)
    {
        var itemInstance = _indexedInventory.Find((i) => i.Item == item);
        if (itemInstance == null) return false;
        _indexedInventory.Remove(itemInstance);
        return true;
    }

    public ItemInstance GetItem(Item item)
    {
        int index = -1;
        foreach (ItemInstance i in _indexedInventory)
        {
            if (i.Item == item)
            {
                index = _indexedInventory.IndexOf(i);
            }
        }
        if (index != -1)
            return _indexedInventory[index];
        else
        {
            Debug.LogError("Inventory does not contain an instance of this item");
            return null;
        }
    }

    public int Count
    {
        get { return _indexedInventory.Count; }
    }
}
