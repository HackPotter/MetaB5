using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemInstance
{
    private Item _item;
    private bool isStack = false;
    private int count = 1;

    public ItemInstance(Item item)
    {
        _item = item;
    }


    public bool CanAddToStack()
    {
        return _item.Stackable && count != _item.MaxStackSize;
    }

    public int AddToStack()
    {
        if (CanAddToStack())
        {
            count++;
            isStack = true;
        }
        else
            throw new InvalidOperationException("Item cannot be added to the stack because the item is either not stackable or is at max stacks");

        return count;
    }

    public int RemoveFromStack()
    {
        if (count != 0)
        {
            count--;
        }

        return count;
    }

    public Item Item
    {
        get { return _item; }
    }

    public bool IsStack
    {
        get { return isStack; }
    }

    public int Count
    {
        get { return count; }
    }





}
