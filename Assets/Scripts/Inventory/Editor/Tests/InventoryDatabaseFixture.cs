#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
using uTest;

[uTestFixture]
public class InventoryDatabaseFixture
{
    ItemDatabase myDatabase;
    Inventory myInventory;
    Item water;
    Item bubble;
    Item leaf;
    Item rareCandy;

    [uSetup]
    void Setup()
    {
        myDatabase = ItemDatabase.CreateInstance<ItemDatabase>();
        myInventory = new Inventory(myDatabase);

        //Items
        water = Item.CreateInstance<Item>(); ;
        bubble = new Item("Bubble", false);
        leaf = new Item("Leaf", false);
        rareCandy = new Item("RareCandy", true, 99);
    }

    //Database Specific Tests

    //Basic Funcitons
    [uTest]
    void AddAndFindItem()
    {
        myDatabase.AddItem(water);
        Assert.AreEqual(1, myDatabase.Count);
        Assert.AreEqual(water, myDatabase.GetItemFromDatabase("WaterDrop"));
    }

    [uTest]
    void AddAndRemove()
    {
        myDatabase.AddItem(bubble);
        Assert.AreEqual(1, myDatabase.Count);
        myDatabase.RemoveItem("Bubble");
        Assert.AreEqual(0, myDatabase.Count);
    }

    //Expected Fails
    [uTest]
    void AddItemAlreadyInDatabase()
    {
        myDatabase.AddItem(bubble);
        myDatabase.AddItem(bubble);
        Assert.AreEqual(1, myDatabase.Count);
    }

    [uTest]
    void RemoveFromEmptyDatabase()
    {
        Assert.False(myDatabase.RemoveItem("water"));
    }

    [uTest]
    void RemoveTwice()
    {
        myDatabase.AddItem(bubble);
        myDatabase.RemoveItem(bubble.name);
        Assert.False(myDatabase.RemoveItem(bubble.name));
    }

    //Inventory Specific Tests

    //Basics
    [uTest]
    void AddItemFromBaseAndFindItem()
    {
        myDatabase.AddItem(water);
        myDatabase.AddItem(bubble);
        myInventory.AddItem(myDatabase.GetItemFromDatabase("WaterDrop"));
        ItemInstance test = myInventory.GetItem(water);
        Assert.AreEqual(myDatabase.GetItemFromDatabase("WaterDrop"), test.Item);
        Assert.AreEqual(1, myInventory.Count);
    }

    [uTest]
    void AddAndRemoveItem()
    {
        myDatabase.AddItem(water);
        myDatabase.AddItem(bubble);
        myInventory.AddItem(myDatabase.GetItemFromDatabase("Bubble"));
        myInventory.RemoveItem(bubble);
        Assert.AreEqual(0, myInventory.Count);
        Assert.AreEqual(2, myDatabase.Count);
        Assert.AreEqual(2, myDatabase.GetItems().Count);
    }

    [uTest]
    void CheckStacking()
    {
        myDatabase.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        myInventory.AddItem(rareCandy);
        Assert.AreEqual(8, myInventory.GetItem(rareCandy).Count);
    }

    [uTest]
    void CheckNoStacking()
    {
        myDatabase.AddItem(bubble);
        myInventory.AddItem(bubble);
        myInventory.AddItem(bubble);
        Assert.AreEqual(2, myInventory.Count);
    }


    //Expected Fails
    [uTest]
    void RemoveFromInventoryTwice()
    {
        myDatabase.AddItem(water);
        myDatabase.AddItem(bubble);
        myInventory.AddItem(myDatabase.GetItemFromDatabase("Bubble"));
        myInventory.RemoveItem(bubble);
        Assert.False(myInventory.RemoveItem(bubble));
    }

    [uTest]
    void RemoveFromEmptyInventory()
    {
        Assert.False(myInventory.RemoveItem(water));
    }


}
