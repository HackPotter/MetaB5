//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEditor;

//public class ItemDetailView
//{
//    private const float kMAX_IMAGE_HEIGHT = 512;
//    private const float kMAX_IMAGE_WIDTH = 512;

//    public Item Item
//    {
//        get;
//        set;
//    }

//    private bool _saved;
//    private ItemDatabase _items;

//    public ItemDetailView(ItemDatabase items)
//    {
//        _items = items;
//    }

//    public void OnGUI(int windowId)
//    {
//        if (!_saved)
//        {
//            if (GUILayout.Button("Save"))
//            {
//                EditorUtility.SetDirty(_items);
//                EditorApplication.SaveAssets();
//                AssetDatabase.SaveAssets();
//                _saved = true;
//            }
//        }
//        if (Item != null)
//        {
//            EditorGUI.BeginChangeCheck();
//            try
//            {
//                GUILayout.BeginHorizontal();
//                GUILayout.Label("Item Name", GUILayout.ExpandWidth(false));
//                Item.ItemName = GUILayout.TextField(Item.ItemName);
//                GUILayout.EndHorizontal();

//                GUILayout.Label("Description", GUILayout.ExpandWidth(false));
//                Item.Description = GUILayout.TextArea(Item.Description);

//                GUILayout.Label("Icon", GUILayout.ExpandWidth(false));
//                if (Item.Icon != null)
//                {
//                    float displayHeight, displayWidth;
//                    CalculateScaledDimensionsForImage(Item.Icon, out displayHeight, out displayWidth);
//                    Item.Icon = (Texture2D)EditorGUILayout.ObjectField(Item.Icon, typeof(Texture2D), GUILayout.Width(displayWidth), GUILayout.Height(displayHeight));
//                }
//                else
//                {
//                    Item.Icon = (Texture2D)EditorGUILayout.ObjectField(Item.Icon, typeof(Texture2D), GUILayout.ExpandWidth(false));
//                }
//            }
//            catch (ExitGUIException)
//            {
//                // sigh
//            }
//            if (EditorGUI.EndChangeCheck())
//            {
//                _saved = false;
//            }
//        }
//    }

//    private static void CalculateScaledDimensionsForImage(Texture texture, out float height, out float width)
//    {
//        height = texture.height;
//        width = texture.width;
//        float heightFactor = 1.0f;
//        float widthFactor = 1.0f;
//        if (height > kMAX_IMAGE_HEIGHT)
//        {
//            heightFactor = kMAX_IMAGE_HEIGHT / height;
//        }
//        if (width * heightFactor > kMAX_IMAGE_WIDTH)
//        {
//            widthFactor = kMAX_IMAGE_WIDTH / width;
//        }

//        float scaleFactor = Mathf.Min(heightFactor, widthFactor);
//        height = scaleFactor * height;
//        width = scaleFactor * width;
//    }
//}

