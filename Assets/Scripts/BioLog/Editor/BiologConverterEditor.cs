//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using System;

//public class BiologConverterEditor : EditorWindow
//{
//    private const string kBIOLOG_DATA_PATH = "Assets/Resources/Biolog/Database/BiologData_Converted.asset";

//    //[MenuItem("Metablast/Utility/Convert Biolog (Warning: Will Overwrite")]
//    private static void ShowWindow()
//    {
//        //var window = EditorWindow.GetWindow<BiologConverterEditor>();
//        //window.Show();
//    }

//    private BiologConverter converter;
//    private BiologData _data;


//    void OnGUI()
//    {
//        if (GUILayout.Button("Load"))
//        {
//            converter = new BiologConverter();
//            converter.load();
//        }

//        if (converter != null)
//        {
//            if (GUILayout.Button("Convert"))
//            {
//                _data = (BiologData)AssetDatabase.LoadAssetAtPath(kBIOLOG_DATA_PATH, typeof(BiologData));
//                if (_data == null)
//                {
//                    _data = ScriptableObject.CreateInstance<BiologData>();
//                    if (!Directory.Exists("Assets/Biolog"))
//                    {
//                        AssetDatabase.CreateFolder("Assets", "Biolog");
//                    }
//                    if (!Directory.Exists("Assets/Biolog/Data"))
//                    {
//                        AssetDatabase.CreateFolder("Assets/Biolog", "Data");
//                    }
//                    AssetDatabase.CreateAsset(_data, kBIOLOG_DATA_PATH);
//                }

//                foreach (BiologConverter.LogEntry entry in converter.entries)
//                {
//                    BiologEntry newEntry = new BiologEntry();

//                    newEntry.EntryName = entry.getName().Trim().Replace('_', ' ');
//                    newEntry.DescriptionText = entry.getDescription().Trim();
//                    newEntry.Tags.Add(entry.getType());

//                    if (entry.getSingleTexture())
//                    {
//                        BiologEntryGalleryItem galleryItem = new BiologEntryGalleryItem();
//                        //galleryItem.GalleryImage = entry.getSingleTexture();
//                        //galleryItem.GalleryPreview = entry.getSingleTexture();
//                        //SetResourcePaths(galleryItem);
//                        newEntry.GalleryItems.Add(galleryItem);
//                    }

//                    if (entry.getAtomTexture())
//                    {
//                        BiologEntryGalleryItem galleryItem = new BiologEntryGalleryItem();
//                        //galleryItem.GalleryImage = entry.getAtomTexture();
//                        //galleryItem.GalleryPreview = entry.getAtomTexture();
//                        //SetResourcePaths(galleryItem);
//                        newEntry.GalleryItems.Add(galleryItem);
//                    }
                    
//                    if (entry.getMeshTexture())
//                    {
//                        BiologEntryGalleryItem galleryItem = new BiologEntryGalleryItem();
//                        //galleryItem.GalleryImage = entry.getMeshTexture();
//                        //galleryItem.GalleryPreview = entry.getMeshTexture();
//                        //SetResourcePaths(galleryItem);
//                        newEntry.GalleryItems.Add(galleryItem);
//                    }

//                    if (entry.getRibbonTexture())
//                    {
//                        BiologEntryGalleryItem galleryItem = new BiologEntryGalleryItem();
//                        //galleryItem.GalleryImage = entry.getRibbonTexture();
//                        //galleryItem.GalleryPreview = entry.getRibbonTexture();
//                        //SetResourcePaths(galleryItem);
//                        newEntry.GalleryItems.Add(galleryItem);
//                    }

//                    _data.Entries.Add(newEntry);
//                }

//                EditorUtility.SetDirty(_data);
//                EditorApplication.SaveAssets();
//                AssetDatabase.SaveAssets();
//            }
//            foreach (BiologConverter.LogEntry entry in converter.entries)
//            {
//                GUILayout.Label(entry.getName());
//            }
//        }
//    }

//    //private void SetResourcePaths(BiologEntryGalleryItem item)
//    //{
//    //    item.GalleryImageResourcePath = GetResourcePath(item.GalleryImage);
//    //    item.GalleryPreviewResourcePath = GetResourcePath(item.GalleryPreview);
//    //}

//    private string GetResourcePath(Sprite texture)
//    {
//        if (!texture)
//        {
//            return "";
//        }
//        string path = AssetDatabase.GetAssetPath(texture);
//        int resourcesIndex = path.LastIndexOf("Resources/", StringComparison.CurrentCultureIgnoreCase);
//        if (resourcesIndex >= 0)
//        {
//            int extensionIndex = path.LastIndexOf('.');
//            int startSubstring = resourcesIndex + "Resources/".Length;
//            path = path.Substring(startSubstring, extensionIndex - startSubstring);
//            return path;
//        }
//        Debug.Log("Warning: Could not find resource path for texture " + texture.name);
//        return "";
//    }
//}

