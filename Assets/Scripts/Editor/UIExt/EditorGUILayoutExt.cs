using System;
using System.Collections.Generic;
using UnityEngine;

public static class EditorGUILayoutExt
{
    public static void BeginLabelStyle(int? fontSize, FontStyle? fontStyle, Color? fontColor, RectOffset margin)
    {
        BeginStyle(GUI.skin.label, fontSize, fontStyle, fontColor, margin);
    }

    public static void EndLabelStyle()
    {
        EndStyle();
    }

    #region Style Customizer
    private struct FontState
    {
        public GUIStyle GUIStyle;
        public bool WordWrap;
        public int FontSize;
        public FontStyle FontStyle;
        public Color FontColor;
        public TextAnchor TextAnchor;
        public RectOffset Margin;

        public FontState(GUIStyle style, int fontSize, FontStyle fontStyle, Color fontColor, TextAnchor textAnchor, RectOffset margin, bool wordWrap)
        {
            GUIStyle = style;
            WordWrap = wordWrap;
            FontSize = fontSize;
            FontStyle = fontStyle;
            FontColor = fontColor;
            Margin = margin;
            TextAnchor = textAnchor;
        }
    }

    private static Stack<FontState> _fontStateStack = new Stack<FontState>();

    public static void BeginStyle(GUIStyle style, int? fontSize, FontStyle? fontStyle, Color? fontColor, RectOffset margin)
    {
        _fontStateStack.Push(GetFontState(style));
        SetFontState(style, fontSize, fontStyle, fontColor, null, margin, null);
    }

    public static void BeginStyle(GUIStyle style, int? fontSize, FontStyle? fontStyle, Color? fontColor, RectOffset margin, bool wordWrap)
    {
        _fontStateStack.Push(GetFontState(style));
        SetFontState(style, fontSize, fontStyle, fontColor, null, margin, wordWrap);
    }

    public static void BeginStyle(GUIStyle style, int? fontSize, FontStyle? fontStyle, Color? fontColor, RectOffset margin, TextAnchor textAnchor)
    {
        _fontStateStack.Push(GetFontState(style));
        SetFontState(style, fontSize, fontStyle, fontColor, textAnchor, margin, null);
    }

    public static void EndStyle()
    {
        FontState fontState = _fontStateStack.Pop();
        SetFontState(fontState.GUIStyle, fontState.FontSize, fontState.FontStyle, fontState.FontColor, fontState.TextAnchor, fontState.Margin, fontState.WordWrap);
    }

    private static void SetFontState(GUIStyle guiStyle, int? fontSize, FontStyle? fontStyle, Color? fontColor, TextAnchor? textAnchor, RectOffset margin, bool? wordWrap)
    {
        if (fontSize.HasValue)
        {
            guiStyle.fontSize = fontSize.Value;
        }

        if (fontStyle.HasValue)
        {
            guiStyle.fontStyle = fontStyle.Value;
        }

        if (fontColor.HasValue)
        {
            guiStyle.normal.textColor = fontColor.Value;
        }
        if (margin != null)
        {
            guiStyle.margin = margin;
        }
        if (textAnchor.HasValue)
        {
            guiStyle.alignment = textAnchor.Value;
        }
        if (wordWrap.HasValue)
        {
            guiStyle.wordWrap = wordWrap.Value;
        }
    }

    private static FontState GetFontState(GUIStyle guiStyle)
    {
        return new FontState(guiStyle, guiStyle.fontSize, guiStyle.fontStyle, guiStyle.normal.textColor, guiStyle.alignment, guiStyle.margin, guiStyle.wordWrap);
    }

    #endregion


    #region Custom Controls

    public static string SearchFilter(string text, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(options);
        string defaultString = "Search...";
        GUI.SetNextControlName("SearchFilterTextField");
        bool focused = GUI.GetNameOfFocusedControl() == "SearchFilterTextField" || !string.IsNullOrEmpty(text);

        if (focused)
        {
            defaultString = text;
        }

        Color? color = focused ? null : new Nullable<Color>(EditorStylesExt.EditorDarkGray);
        FontStyle? style = focused ? null : new Nullable<FontStyle>(FontStyle.Italic);
        EditorGUILayoutExt.BeginStyle(EditorStylesExt.SearchTextField, null, style, color, null);
        string newText = GUILayout.TextField(defaultString, EditorStylesExt.SearchTextField);
        EditorGUILayoutExt.EndStyle();

        if (focused)
        {
            text = newText;
        }

        if (GUILayout.Button("", EditorStylesExt.SearchCancelButton))
        {
            text = "";
        }
        GUILayout.EndHorizontal();
        return text;
    }

    public static string SearchFilter(string text, string defaultString, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(options);

        GUI.SetNextControlName("SearchFilterTextField");
        bool focused = GUI.GetNameOfFocusedControl() == "SearchFilterTextField";

        if (focused)
        {
            defaultString = text;
        }

        Color? color = focused ? null : new Nullable<Color>(EditorStylesExt.EditorDarkGray);
        FontStyle? style = focused ? null : new Nullable<FontStyle>(FontStyle.Italic);
        EditorGUILayoutExt.BeginStyle(EditorStylesExt.SearchTextField, null, style, color, null);
        string newText = GUILayout.TextField(defaultString, EditorStylesExt.SearchTextField);
        EditorGUILayoutExt.EndStyle();

        if (focused)
        {
            text = newText;
        }

        if (GUILayout.Button("", EditorStylesExt.SearchCancelButton))
        {
            text = "";
        }
        GUILayout.EndHorizontal();
        return text;
    }

    public static bool EnterableTextField(ref string text)
    {
        bool toReturn = false;
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(""), EditorStylesExt.OLPlus, GUILayout.ExpandWidth(false));
        buttonRect.yMin += 3;
        Rect textFieldRect = GUILayoutUtility.GetRect(new GUIContent(""), GUI.skin.textField, GUILayout.ExpandWidth(true));
        textFieldRect.xMin -= 2;


        GUI.SetNextControlName("CreateObjectTextField");
        text = GUI.TextField(textFieldRect, text);
        if (GUI.Button(buttonRect, "", EditorStylesExt.OLPlus) ||
            GUI.GetNameOfFocusedControl() == "CreateObjectTextField" && UnityEngine.Event.current.isKey && UnityEngine.Event.current.keyCode == KeyCode.Return)
        {
            GUI.FocusControl(null);
            toReturn = true;
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndVertical();
        return toReturn;
    }

    public static bool AddItemTextField(ref string text)
    {
        bool toReturn = false;
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(""), EditorStylesExt.OLPlus, GUILayout.ExpandWidth(false));
        buttonRect.yMin += 3;
        Rect textFieldRect = GUILayoutUtility.GetRect(new GUIContent(""), GUI.skin.textField, GUILayout.ExpandWidth(true));
        textFieldRect.xMin -= 2;


        GUI.SetNextControlName("CreateObjectTextField");
        text = GUI.TextField(textFieldRect, text);
        if (GUI.Button(buttonRect, "", EditorStylesExt.OLPlus) ||
            GUI.GetNameOfFocusedControl() == "CreateObjectTextField" && UnityEngine.Event.current.isKey && UnityEngine.Event.current.keyCode == KeyCode.Return)
        {
            GUI.FocusControl(null);
            toReturn = true;
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndVertical();
        return toReturn;
    }

    public static int Button(string content, GUIStyle style, out Rect r, params GUILayoutOption[] options)
    {
        r = GUILayoutUtility.GetRect(new GUIContent(content), style, options);

        if (GUI.Button(r, content, style))
        {
            if (Event.current.button == 0)
            {
                return 0;
            }
            else if (Event.current.button == 1)
            {
                return 1;
            }
        }
        return -1;
    }

    #endregion
}

