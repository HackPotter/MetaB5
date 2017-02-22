#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BiologEntryDetailView
{
    private BiologData _data;
    private BiologEditorModel _model;

    private int _windowId;
    private string _newTag = "";

    private bool _descriptionCollapsed = false;
    private bool _tagsCollapsed = false;
    private bool _galleryItemsCollapsed = false;
    private bool _detailItemsCollapsed = false;

    private List<string> _tagDeleteCache = new List<string>();
    private List<BiologEntryPreview> _previewsToRemove = new List<BiologEntryPreview>();
    private List<BiologEntryPreview> _hiddenPreviews = new List<BiologEntryPreview>();

    private List<BiologEntryGalleryItem> _galleryItemsToRemove = new List<BiologEntryGalleryItem>();
    private List<BiologEntryGalleryItem> _hiddenGalleryItems = new List<BiologEntryGalleryItem>();
    private Vector2 _scrollPosition;

    private const float kMAX_IMAGE_HEIGHT = 512;
    private const float kMAX_IMAGE_WIDTH = 512;
    private bool _saved = true;

    public BiologEntryDetailView(BiologEditorModel model, BiologData data)
    {
        _data = data;
        _model = model;

        _model.SelectedEntryChanged += new Action<BiologEntry>(_model_SelectedEntryChanged);

        foreach (var entry in data.Entries)
        {
            //if (entry.Preview3D)
            //{
            //    BiologEntryGalleryItem galleryItem3D = new BiologEntryGalleryItem();

            //    galleryItem3D.Is3D = true;
            //    galleryItem3D.Scene3D = entry.Preview3D;
            //    entry.GalleryItems.Insert(0, galleryItem3D);
            //    entry.Preview3D = null;
            //}
            foreach (var gal in entry.GalleryItems)
            {
                //try
                //{
                //    string path = AssetDatabase.GetAssetPath(gal.GalleryImage);
                //    Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                //    if (!sprite)
                //    {
                //        Debug.Log("Tried to load sprite at " + gal.GalleryImageResourcePath + ", but nothing was returned.", gal.GalleryImage);
                //    }
                //    else
                //    {
                //        gal.GalleryImage = sprite;
                //    }
                //}
                //catch (Exception e)
                //{
                //    Debug.LogError("Failed to load image at " + gal.GalleryImageResourcePath + " as sprite!");
                //    continue;
                //}

                //try
                //{
                //    string path = AssetDatabase.GetAssetPath(gal.GalleryPreview);
                //    Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                //    if (!sprite)
                //    {
                //        Debug.Log("Tried to load sprite at " + gal.GalleryPreviewResourcePath + ", but nothing was returned.", gal.GalleryPreview);
                //    }
                //    else
                //    {
                //        gal.GalleryPreview = sprite;
                //    }

                //}
                //catch (Exception e)
                //{
                //    Debug.LogError("Failed to load image at " + gal.GalleryPreviewResourcePath + " as sprite!");
                //    continue;
                //}
            }
        }
    }

    void _model_SelectedEntryChanged(BiologEntry obj)
    {
        GUI.FocusControl(null);
        if (_data)
        {
            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
        }
        _hiddenGalleryItems.Clear();
        _hiddenPreviews.Clear();
    }

    public void OnGUI(int windowId)
    {
        _windowId = windowId;

        if (_model.SelectedEntry == null)
        {
            GUILayout.Label("Select a BiologEntry to edit in the panel to the left!");
        }
        else
        {
            if (GUILayout.Button((_saved ? "" : "*") + "Save", GUILayout.ExpandWidth(false)))
            {
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
                AssetDatabase.SaveAssets();
                _saved = true;
            }
            EditorGUI.BeginChangeCheck();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            DrawNameGUI();
            //Draw3DPreviewGUI();
            DrawTagsGUI();
            DrawDescriptionGUI();
            //DrawPreviewItemsGUI();
            DrawGalleryItemsGUI();
            DrawDetailsGUI();

            GUILayout.EndScrollView();

            if (EditorGUI.EndChangeCheck())
            {
                _saved = false;
            }
        }

        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
            Event.current.Use();
        }
    }

    private void DrawNameGUI()
    {
        // Name
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Label("Biolog Entry Name:", GUILayout.ExpandWidth(false));
        _model.SelectedEntry.EntryName = GUILayout.TextField(_model.SelectedEntry.EntryName);
        GUILayout.EndHorizontal();
    }

    //private void Draw3DPreviewGUI()
    //{
    //    // Name
    //    GUILayout.BeginHorizontal(GUI.skin.box);
    //    GUILayout.Label("3D Preview:", GUILayout.ExpandWidth(false));
    //    _model.SelectedEntry.Preview3D = (GameObject)EditorGUILayout.ObjectField(_model.SelectedEntry.Preview3D, typeof(GameObject), false);
    //    GUILayout.EndHorizontal();
    //}

    private void DrawTagsGUI()
    {
        // Tags
        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Tags");
        _tagsCollapsed = GUILayout.Button(_tagsCollapsed ? "Expand" : "Collapse", GUILayout.ExpandWidth(false)) ? !_tagsCollapsed : _tagsCollapsed;
        GUILayout.EndHorizontal();

        if (!_tagsCollapsed)
        {
            foreach (string s in _model.SelectedEntry.Tags)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(s, GUI.skin.textField, GUILayout.ExpandWidth(false)) && Event.current.isMouse && Event.current.button == 1)
                {
                    // Display context menu.

                    // Add "Rename", "Remove"
                }
                if (GUILayout.Button("x", GUILayout.ExpandWidth(false)))
                {
                    _tagDeleteCache.Add(s);
                }
                GUILayout.EndHorizontal();
            }

            foreach (string s in _tagDeleteCache)
            {
                _model.SelectedEntry.Tags.Remove(s);
            }
            _tagDeleteCache.Clear();

            GUILayout.BeginHorizontal();
            string[] tags = _model.AllTags.ToArray();
            Rect addTagRect = GUILayoutUtility.GetRect(new GUIContent("New Tag"), GUI.skin.button, GUILayout.ExpandWidth(false));

            if (EditorGUI.EndChangeCheck())
            {
                _saved = false;
            }
            if (GUI.Button(addTagRect, "New Tag"))
            {
                BiologEntry selectedEntry = _model.SelectedEntry;
                Action<string> callback = (s) =>
                {
                    selectedEntry.Tags.Add(s.ToLower());
                    _saved = false;
                    _model.Repaint();
                };
                Vector2 offset = GUIUtility.GUIToScreenPoint(new Vector2(addTagRect.xMin, addTagRect.yMin));
                TagSelectorPopupWindow.ShowAsDropDown(new Rect(offset.x, offset.y, addTagRect.width, addTagRect.height),
                    new Vector2(200, 300), callback, _model.AllTags);

            }
            EditorGUI.BeginChangeCheck();
            //_newTag = GUILayout.TextField(_newTag);

            //string[] existingTags = _model.AllTags.Where((s) => s.StartsWith(_newTag, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            //int selectedIndex = EditorGUILayout.Popup(-1, existingTags);
            //if (selectedIndex != -1)
            //{
            //    _model.SelectedEntry.Tags.Add(existingTags[selectedIndex].ToLower());
            //    _newTag = "";
            //}
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scale Text: ");
        _model.SelectedEntry.Scale = GUILayout.TextField(_model.SelectedEntry.Scale);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void DrawDescriptionGUI()
    {
        //Description
        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description");
        _descriptionCollapsed = GUILayout.Button(_descriptionCollapsed ? "Expand" : "Collapse", GUILayout.ExpandWidth(false)) ? !_descriptionCollapsed : _descriptionCollapsed;
        GUILayout.EndHorizontal();


        if (!_descriptionCollapsed)
        {
            EditorStyles.textField.wordWrap = true;
            _model.SelectedEntry.DescriptionText = EditorGUILayout.TextArea(_model.SelectedEntry.DescriptionText);
        }
        GUILayout.EndVertical();
    }
    /*
    private void DrawPreviewItemsGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Preview Items");
        _previewItemsCollapsed = GUILayout.Button(_previewItemsCollapsed ? "Expand" : "Collapse", GUILayout.ExpandWidth(false)) ? !_previewItemsCollapsed : _previewItemsCollapsed;
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Add New Preview Item", GUILayout.ExpandWidth(false)))
        {
            _model.SelectedEntry.EntryPreviews.Add(new BiologEntryPreview());
        }
        if (!_previewItemsCollapsed)
        {
            foreach (BiologEntryPreview preview in _model.SelectedEntry.EntryPreviews)
            {

                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Image Name:", GUILayout.ExpandWidth(false));
                preview.PreviewImageName = GUILayout.TextField(preview.PreviewImageName, GUILayout.ExpandWidth(false), GUILayout.MinWidth(75));

                if (_hiddenPreviews.Contains(preview))
                {
                    if (GUILayout.Button("Show Image", GUILayout.ExpandWidth(false)))
                    {
                        _hiddenPreviews.Remove(preview);
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        _previewsToRemove.Add(preview);
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    if (GUILayout.Button("Hide Image", GUILayout.ExpandWidth(false)))
                    {
                        _hiddenPreviews.Add(preview);
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        _previewsToRemove.Add(preview);
                    }
                    GUILayout.EndHorizontal();
                    try
                    {
                        if (preview.PreviewImage != null)
                        {

                            float height, width;
                            CalculateScaledDimensionsForImage(preview.PreviewImage, out height, out width);
                            preview.PreviewImage = (Texture)EditorGUILayout.ObjectField(preview.PreviewImage, typeof(Texture), false, GUILayout.Height(height), GUILayout.Width(width));
                        }
                        else
                        {
                            preview.PreviewImage = (Texture)EditorGUILayout.ObjectField(preview.PreviewImage, typeof(Texture), false, GUILayout.ExpandWidth(false));
                        }
                    }
                    catch (ExitGUIException)
                    {
                        // I dunno it always throws this.
                    }
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();

        foreach (BiologEntryPreview preview in _previewsToRemove)
        {
            _model.SelectedEntry.EntryPreviews.Remove(preview);
            _hiddenPreviews.Remove(preview);
        }
    }
    */

    private void DrawGalleryItemsGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Gallery Items");
        _galleryItemsCollapsed = GUILayout.Button(_galleryItemsCollapsed ? "Expand" : "Collapse", GUILayout.ExpandWidth(false)) ? !_galleryItemsCollapsed : _galleryItemsCollapsed;
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Add New Gallery Item", GUILayout.ExpandWidth(false)))
        {
            _model.SelectedEntry.GalleryItems.Add(new BiologEntryGalleryItem());
        }
        if (!_galleryItemsCollapsed)
        {
            foreach (BiologEntryGalleryItem galleryItem in _model.SelectedEntry.GalleryItems)
            {

                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Caption:", GUILayout.ExpandWidth(false));
                galleryItem.Caption = GUILayout.TextField(galleryItem.Caption, GUILayout.ExpandWidth(false), GUILayout.MinWidth(75));

                if (_hiddenGalleryItems.Contains(galleryItem))
                {
                    if (GUILayout.Button("Show Image", GUILayout.ExpandWidth(false)))
                    {
                        _hiddenGalleryItems.Remove(galleryItem);
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        _galleryItemsToRemove.Add(galleryItem);
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    if (GUILayout.Button("Hide Image", GUILayout.ExpandWidth(false)))
                    {
                        _hiddenGalleryItems.Add(galleryItem);
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        _galleryItemsToRemove.Add(galleryItem);
                    }
                    GUILayout.EndHorizontal();

                    try
                    {
                        Sprite newTexture;
                        string path;
                        int resourcesIndex;
                        GUILayout.Label("Gallery Image");
                        galleryItem.Is3D = GUILayout.Toggle(galleryItem.Is3D, "Is 3D Preview");
                        if (galleryItem.Is3D)
                        {
                            GUILayout.BeginHorizontal(GUI.skin.box);
                            GUILayout.Label("3D Preview:", GUILayout.ExpandWidth(false));
                            galleryItem.Scene3D = (GameObject)EditorGUILayout.ObjectField(galleryItem.Scene3D, typeof(GameObject), false);
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            if (galleryItem.GalleryImage != null)
                            {
                                //CalculateScaledDimensionsForImage(galleryItem.GalleryImage, out height, out width);
                                newTexture = (Sprite)EditorGUILayout.ObjectField(galleryItem.GalleryImage, typeof(Sprite), false);
                            }
                            else
                            {
                                newTexture = (Sprite)EditorGUILayout.ObjectField(galleryItem.GalleryImage, typeof(Sprite), false, GUILayout.ExpandWidth(false));
                            }

                            path = AssetDatabase.GetAssetPath(newTexture);

                            resourcesIndex = path.LastIndexOf("Resources/", StringComparison.CurrentCultureIgnoreCase);
                            if (resourcesIndex >= 0)
                            {
                                int extensionIndex = path.LastIndexOf('.');
                                int startSubstring = resourcesIndex + "Resources/".Length;
                                path = path.Substring(startSubstring, extensionIndex - startSubstring);

                                if (galleryItem.GalleryImage == galleryItem.GalleryPreview)
                                {
                                    galleryItem.GalleryPreview = galleryItem.GalleryImage;
                                    //galleryItem.GalleryPreviewResourcePath = path;
                                }

                                galleryItem.GalleryImage = newTexture;
                                //galleryItem.GalleryImageResourcePath = path;
                            }
                        }

                        GUILayout.Label("Gallery Preview");
                        if (galleryItem.GalleryPreview != null)
                        {
                            newTexture = (Sprite)EditorGUILayout.ObjectField(galleryItem.GalleryPreview, typeof(Sprite), false);//, GUILayout.Height(96), GUILayout.Width(98));
                        }
                        else
                        {
                            newTexture = (Sprite)EditorGUILayout.ObjectField(galleryItem.GalleryPreview, typeof(Sprite), false);//, GUILayout.ExpandWidth(false));
                        }

                        path = AssetDatabase.GetAssetPath(newTexture);

                        resourcesIndex = path.LastIndexOf("Resources/", StringComparison.CurrentCultureIgnoreCase);
                        if (resourcesIndex >= 0)
                        {
                            int extensionIndex = path.LastIndexOf('.');
                            int startSubstring = resourcesIndex + "Resources/".Length;
                            path = path.Substring(startSubstring, extensionIndex - startSubstring);

                            //Debug.Log(path);
                            galleryItem.GalleryPreview = newTexture;
                            //galleryItem.GalleryPreviewResourcePath = path;
                        }
                    }
                    catch (ExitGUIException)
                    {
                        // I dunno it always throws this.
                    }
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();

        foreach (BiologEntryGalleryItem galleryItem in _galleryItemsToRemove)
        {
            _model.SelectedEntry.GalleryItems.Remove(galleryItem);
            _hiddenGalleryItems.Remove(galleryItem);
        }
    }

    private void DrawDetailsGUI()
    {

        if (!_detailItemsCollapsed)
        {
            foreach (BiologEntryDetailSection entryDetail in _model.SelectedEntry.DetailSections)
            {
                //Detail Items
            }
        }
    }

    private static void CalculateScaledDimensionsForImage(Sprite texture, out float height, out float width)
    {
        height = texture.texture.height;
        width = texture.texture.width;
        float heightFactor = 1.0f;
        float widthFactor = 1.0f;
        if (height > kMAX_IMAGE_HEIGHT)
        {
            heightFactor = kMAX_IMAGE_HEIGHT / height;
        }
        if (width * heightFactor > kMAX_IMAGE_WIDTH)
        {
            widthFactor = kMAX_IMAGE_WIDTH / width;
        }

        float scaleFactor = Mathf.Min(heightFactor, widthFactor);
        height = scaleFactor * height;
        width = scaleFactor * width;
    }
}
