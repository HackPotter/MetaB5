using System.Collections.Generic;
using UnityEngine;


public class ItemDatabase : ScriptableObject
{

    [SerializeField]
    private List<Item> _items;

    private Dictionary<string, Item> _itemsByName;

    //Field Accessors
    public List<Item> GetItems()
    {
        return _items;
    }

    public int Count
    {
        get { return _items.Count; }
    }

    //Constructor
    public ItemDatabase(
        )
    {
        if (_items == null)
        {
            _items = new List<Item>();
        }

        _itemsByName = new Dictionary<string, Item>();

        foreach (Item item in _items)
        {
            _itemsByName.Add(item.ItemName, item);
        }
    }

    //Basic functions
    public void AddItem(Item item)
    {
        if (!(_items.Contains(item)))
        {
            _items.Add(item);
            _itemsByName.Add(item.ItemName, item);
        }
        else
            Debug.LogError("Database already contains that item");
    }

    //// CodeReview: Standard behavior in C# for removing elements from a collection is to return a bool instead
    //// of throwing an exception.
    //// If the item is found, it is removed and true is returned.
    //// If the item is not found, nothing changes and false is returned.
    public bool RemoveItem(string itemName)
    {
        Item remove;
        if (_itemsByName.TryGetValue(itemName, out remove))
        {
            _items.Remove(remove);
            _itemsByName.Remove(itemName);
            return true;
        }
        else
            Debug.LogError("Item not found in database");
        return false;
    }

    public Item GetItemFromDatabase(string ItemName)
    {
        if (_itemsByName.ContainsKey(ItemName))
        {
            return _itemsByName[ItemName];
        }
        else
        {
            Debug.LogError("Item does not exist in database");
            return null;
        }
    }
}

