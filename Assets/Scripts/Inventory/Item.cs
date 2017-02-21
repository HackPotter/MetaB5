using System;
using UnityEngine;

[Serializable]
public class Item : ScriptableObject
{
    //Bunch o' attributes
    [SerializeField]
    private string _itemName = "";
    [SerializeField]
    private string _description = "";
    [SerializeField]
    private Texture2D _icon;
    [SerializeField]
    private bool stackable;
    [SerializeField]
    private int maxStackSize;

    public bool Usable;
    public bool ConsumedOnUse;

    public static Item CreateItem(string name, bool canStack, int maxSize = 1)
    {
        var item = ScriptableObject.CreateInstance<Item>();
        item._itemName = name;
        item.stackable = canStack;
        item.maxStackSize = maxSize;


        return item;
    }

    public Item()
    {
    }

    public Item(string name, bool canStack, int maxStackSize = 1)
    {
        _itemName = name;
        stackable = canStack;
        this.maxStackSize = maxStackSize;
    }


    //Visuals
    [SerializeField]
    private GameObject _onTheGround;
    [SerializeField]
    private GameObject _used;

    //Accessors
    public GameObject OnTheGround
    {
        get { return _onTheGround; }
        set { _onTheGround = value; }
    }

    public GameObject Used
    {
        get { return _used; }
        set { _used = value; }
    }

    public string ItemName
    {
        get { return _itemName; }
        set { _itemName = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public Texture2D Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }

    public bool Stackable
    {
        get { return stackable; }
    }

    public int MaxStackSize
    {
        get { return maxStackSize; }
    }
}

