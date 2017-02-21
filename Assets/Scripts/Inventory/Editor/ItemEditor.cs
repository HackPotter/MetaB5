//using System.IO;
//using UnityEditor;
//using UnityEngine;

//public class ItemEditor : EditorWindow
//{
//    private const string kInventoryItemDataPath = "Assets/Resources/Inventory/Database/";
//    [MenuItem("Metablast/Item Editor")]
//    public static void OpenItemEditor()
//    {
//        var window = EditorWindow.GetWindow<ItemEditor>();
//    }

//    private ItemDatabase _itemDatabase;

//    private ItemListView _itemListView;
//    private ItemDetailView _itemDetailView;

//    void OnEnable()
//    {
//        _itemDatabase = (ItemDatabase)AssetDatabase.LoadAssetAtPath(kInventoryItemDataPath + "InventoryDatabase.asset", typeof(ItemDatabase));

//        if (_itemDatabase == null)
//        {
//            Directory.CreateDirectory(kInventoryItemDataPath);
//            _itemDatabase = ScriptableObject.CreateInstance<ItemDatabase>();
//            AssetDatabase.CreateAsset(_itemDatabase, kInventoryItemDataPath + "InventoryDatabase.asset");
//        }

//        _itemListView = new ItemListView(_itemDatabase);
//        _itemDetailView = new ItemDetailView(_itemDatabase);

//        minSize = new UnityEngine.Vector2(1000, 600);
//    }

//    private Rect _listViewDimensions = new Rect(0, 0, 300, 600);
//    private Rect _detailViewDimensions = new Rect(301, 0, 700, 600);

//    void OnGUI()
//    {
//        _listViewDimensions.height = this.position.height;

//        _detailViewDimensions.height = this.position.height;
//        _detailViewDimensions.width = this.position.width - _listViewDimensions.width;

//        BeginWindows();
//        GUILayout.Window(1, _listViewDimensions, _itemListView.OnGUI, "");
//        _itemDetailView.Item = _itemListView.SelectedItem;
//        GUILayout.Window(2, _detailViewDimensions, _itemDetailView.OnGUI, "");

//        EndWindows();
//    }
//}

