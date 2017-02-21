//using UnityEngine;
//using UnityEditor;

//public class ItemListView
//{
//    private const string kDEFAULT_ENTRY_NAME = "New Inventory Item";
//    private readonly GUIContent[] _entryContextOptions = new GUIContent[] { new GUIContent("Create New Item"), new GUIContent("Delete") };
//    private readonly GUIContent[] _panelContextOptions = new GUIContent[] { new GUIContent("Create New Item") };

//    private ItemDatabase _itemDatabase;

//    private int _windowId;
//    private Vector2 _scrollPosition;
//    private string _newEntryName = "";

//    public Item SelectedItem
//    {
//        get;
//        private set;
//    }

//    public ItemListView(ItemDatabase items)
//    {
//        _itemDatabase = items;
//    }

//    public void OnGUI(int windowId)
//    {
//        _windowId = windowId;

//        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

//        foreach (InventoryItem entry in _itemDatabase.Items)
//        {
//            if (entry == null)
//            {
//                continue;
//            }
//            DrawInventoryItem(entry);
//        }

//        GUILayout.Space(10);
//        DrawCreateNewEntryButton();

//        GUILayout.EndScrollView();
//    }

//    private void DrawInventoryItem(InventoryItem entry)
//    {
//        if (GUILayout.Button(entry.ItemName))
//        {
//            if (Event.current.button == 0)
//            {
//                SelectedItem = entry;
//            }
//            else if (Event.current.button == 1)
//            {
//                Vector2 mousePosition = Event.current.mousePosition;
//                EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _entryContextOptions, -1, ContextMenuCallback, entry);
//            }
//            GUI.FocusWindow(_windowId);
//        }
//    }

//    private void CheckPanelContextMenu()
//    {
//        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
//        {
//            Vector2 mousePosition = Event.current.mousePosition;
//            EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _panelContextOptions, -1, PanelContextMenuCallback, null);
//            Event.current.Use();
//        }
//    }

//    private void PanelContextMenuCallback(object obj, string[] items, int selection)
//    {
//        switch (selection)
//        {
//            case 0:
//                CreateNewEntry("");
//                break;
//        }
//    }

//    private void DrawCreateNewEntryButton()
//    {
//        GUI.SetNextControlName("ItemListView.ItemNameTextField");
//        _newEntryName = GUILayout.TextField(_newEntryName);
//        if (GUILayout.Button("Create New Item") || GUI.GetNameOfFocusedControl() == "ItemListView.ItemNameTextField" && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
//        {
//            CreateNewEntry(_newEntryName);
//            _newEntryName = "";
//            Event.current.Use();
//        }
//    }

//    private void ContextMenuCallback(object obj, string[] items, int selection)
//    {
//        InventoryItem entry = obj as InventoryItem;
//        switch (selection)
//        {
//            case 0:
//                CreateNewEntry("");
//                break;
//            case 1:
//                if (entry != null)
//                {
//                    _itemDatabase.Items.Remove(entry);
//                    SelectedItem = null;
//                    //_biologData.Entries.Remove(entry);
//                    //_model.SelectedEntry = null;
//                }
//                break;
//        }
//    }

//    private void CreateNewEntry(string name)
//    {
//        if (name == "")
//        {
//            name = kDEFAULT_ENTRY_NAME;
//        }
//        InventoryItem item = new InventoryItem();
//        item.ItemName = name;
//        _itemDatabase.Items.Add(item);

//        EditorUtility.SetDirty(_itemDatabase);
//        EditorApplication.SaveAssets();
//        AssetDatabase.SaveAssets();
//    }
//}