//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Squid;

//public class InventoryView : Frame
//{
//    private UnityRenderer _unityRenderer;

//    private Frame[] _inventorySlots;

//    public InventoryView(Desktop inventoryDesktop)
//    {
//        inventoryDesktop.Dock = DockStyle.Fill;
//        Controls.Add(inventoryDesktop);


//        FlowLayoutFrame grid = (FlowLayoutFrame)GetControl("Grid");
//        _inventorySlots = new Frame[grid.Controls.Count];

//        int index = 0;
//        foreach (Frame f in grid.Controls)
//        {
//            _inventorySlots[index++] = f;
//        }

//        _unityRenderer = new UnityRenderer();
//        foreach (KeyValuePair<IInventoryItem, int> item in GameContext.Instance.Player.Inventory.HeldItems)
//        {
//            Frame itemFrame = CreateItemFrame(item.Key);
//        }
//    }

//    private Frame CreateItemFrame(IInventoryItem item)
//    {
//        Frame frame = new Frame();
//        frame.Tag = item;
//        frame.Dock = DockStyle.Fill;
        
//        ImageControl icon = new ImageControl();
//        icon.Dock = DockStyle.Fill;
//        _unityRenderer.InsertTexture(item.Icon, "idkwtf");
//        icon.Texture = "idkwtf";

//        return frame;
//    }
//}

