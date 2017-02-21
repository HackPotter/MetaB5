using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Window to choose and create an Expression object that can be assigned to a field of the given ScriptecaType fieldType.
/// </summary>
public class TagSelectorPopupWindow : EditorWindow
{
    public static void ShowAsDropDown(Rect buttonRect, Vector2 size, Action<string> selectionCallback, HashSet<string> allTags)
    {
        var window = EditorWindow.CreateInstance<TagSelectorPopupWindow>();
        window._selectionCallback = selectionCallback;
        window._allTags = allTags.ToList();
        window.ShowAsDropDown(buttonRect, size);
    }

    private List<string> _allTags;
    private string _searchField = "";
    private Action<string> _selectionCallback;
    private Vector2 _scroll;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
        GUILayout.BeginVertical(EditorStylesExt.GreyBorder);
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

        GUI.SetNextControlName("TagTextField");
        if (EditorGUILayoutExt.EnterableTextField(ref _searchField))
        {
            _selectionCallback(_searchField);
            Close();
        }



        _scroll = GUILayout.BeginScrollView(_scroll);


        foreach (string tagCandidate in _allTags)
        {
            if (!string.IsNullOrEmpty(_searchField))
            {
                if (!tagCandidate.StartsWith(_searchField, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
            }
            if (GUILayout.Button(tagCandidate, EditorStylesExt.MiniButtonMiddle))
            {
                _selectionCallback(tagCandidate);
                Close();
            }
        }

        GUILayout.EndScrollView();



        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add", EditorStylesExt.ButtonMiddle))
        {
            _selectionCallback(_searchField);
            Close();
        }
        if (GUILayout.Button("Cancel", EditorStylesExt.ButtonMiddle))
        {
            Close();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void Update()
    {
        Repaint();
    }
}
