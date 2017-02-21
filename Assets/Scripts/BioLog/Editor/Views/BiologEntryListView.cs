using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;

public class BiologEntryListView
{
    private const string kDEFAULT_ENTRY_NAME = "New Biolog Entry";
    private readonly GUIContent[] _entryContextOptions = new GUIContent[] { new GUIContent("Create New Entry"), new GUIContent("Delete") };
    private readonly GUIContent[] _panelContextOptions = new GUIContent[] { new GUIContent("Create New Entry") };

    private BiologData _biologData;
    private BiologEditorModel _model;

    private int _windowId;
    private Vector2 _scrollPosition;
    private string _newEntryName = "";

    public BiologEntryListView(BiologEditorModel model, BiologData biologData)
    {
        _biologData = biologData;
        _model = model;
    }

    private class JSONBiologEntry
    {
        public string EntryName;
        public string[] Tags;
        public string ScaleText;
        public string Description;
        public JSONBiologGalleryItem[] GalleryItems;
    }

    private class JSONBiologGalleryItem
    {
        public string Caption;
        public string Image;
        public string Thumbnail;
    }

    private void WriteBiologJSON(JSONBiologEntry[] toWrite, StreamWriter writer)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        };
        JsonSerializer.Create(settings).Serialize(writer, toWrite);
    }

    private void WriteToJson()
    {
        JSONBiologEntry[] entries = _biologData.Entries.ConvertAll((e) =>
        {
            return new JSONBiologEntry()
            {
                EntryName = e.EntryName,
                Tags = e.Tags.ToArray(),
                ScaleText = e.Scale,
                Description = e.DescriptionText,
                GalleryItems = e.GalleryItems.ConvertAll((g) =>
                {
                    return new JSONBiologGalleryItem()
                    {
                        Caption = g.Caption,
                        Image = g.GalleryImage ?
                            new Func<string>(() =>
                            {
                                string path = AssetDatabase.GetAssetPath(g.GalleryImage);
                                Texture2D texture = new Texture2D(g.GalleryImage.texture.width, g.GalleryImage.texture.height);
                                texture.LoadImage(File.ReadAllBytes(path));
                                return Convert.ToBase64String(texture.EncodeToPNG());
                            })() : "",
                        Thumbnail = g.GalleryPreview && g.GalleryPreview != g.GalleryImage ?
                        new Func<string>(() =>
                        {
                            string path = AssetDatabase.GetAssetPath(g.GalleryPreview);
                            Texture2D texture = new Texture2D(g.GalleryPreview.texture.width, g.GalleryPreview.texture.height);
                            texture.LoadImage(File.ReadAllBytes(path));
                            return Convert.ToBase64String(texture.EncodeToPNG());
                        })() : "",
                    };
                }).ToArray(),

            };
        }).ToArray();

        using (StreamWriter writer = new StreamWriter(File.Create("BiologJSON.txt")))
        {
            WriteBiologJSON(entries, writer);
        }
    }

    public void OnGUI(int windowId)
    {
        _windowId = windowId;

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        if (GUILayout.Button("Write to JSON"))
        {
            WriteToJson();
        }
        foreach (BiologEntry entry in _biologData.Entries)
        {
            if (entry == null)
            {
                continue;
            }
            DrawBiologEntry(entry);
        }

        GUILayout.Space(10);
        DrawCreateNewEntryButton();

        GUILayout.EndScrollView();
    }

    private void DrawBiologEntry(BiologEntry entry)
    {
        if (GUILayout.Button(entry.EntryName))
        {
            if (Event.current.button == 0)
            {
                _model.SelectedEntry = entry;
            }
            else if (Event.current.button == 1)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _entryContextOptions, -1, ContextMenuCallback, entry);
            }
            GUI.FocusWindow(_windowId);
        }
    }

    //private void CheckPanelContextMenu()
    //{
    //    if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
    //    {
    //        Vector2 mousePosition = Event.current.mousePosition;
    //        EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _panelContextOptions, -1, PanelContextMenuCallback, null);
    //        Event.current.Use();
    //    }
    //}

    //private void PanelContextMenuCallback(object obj, string[] items, int selection)
    //{
    //    switch (selection)
    //    {
    //        case 0:
    //            CreateNewEntry("");
    //            break;
    //    }
    //}

    private void DrawCreateNewEntryButton()
    {
        GUI.SetNextControlName("BiologEntryListView.EntryNameTextField");
        _newEntryName = GUILayout.TextField(_newEntryName);
        if (GUILayout.Button("Create New Entry") || GUI.GetNameOfFocusedControl() == "BiologEntryListView.EntryNameTextField" && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            CreateNewEntry(_newEntryName);
            _newEntryName = "";
            Event.current.Use();
        }
    }

    private void ContextMenuCallback(object obj, string[] items, int selection)
    {
        BiologEntry entry = obj as BiologEntry;
        switch (selection)
        {
            case 0:
                CreateNewEntry("");
                break;
            case 1:
                if (entry != null)
                {
                    _biologData.Entries.Remove(entry);
                    _model.SelectedEntry = null;
                }
                break;
        }
    }

    private void CreateNewEntry(string name)
    {
        if (name == "")
        {
            name = kDEFAULT_ENTRY_NAME;
        }
        BiologEntry entry = new BiologEntry();
        entry.EntryName = name;
        _biologData.Entries.Add(entry);

        EditorUtility.SetDirty(_biologData);
        AssetDatabase.SaveAssets();
        AssetDatabase.SaveAssets();
    }
}

