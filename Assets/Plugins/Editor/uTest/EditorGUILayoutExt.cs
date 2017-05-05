using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public static class EditorGUILayoutExt
{
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

    #region Resizable Areas

    public static Rect BeginVerticalResize(Rect area, ref float value, GUIStyle style, RectOffset border, string label = "")
    {
        Rect areaRect = new Rect(0 + border.left, 0 + border.top, area.width - border.horizontal, value - border.vertical);
        Rect visibleRect = new Rect(0, 0, area.width, value);
        GUI.Box(visibleRect, "", style);
        GUILayout.BeginArea(areaRect, label, GUIStyle.none);
        return areaRect;
    }

    public static Rect VerticalResizeDivider(Rect area, ref float value, ref bool resizing, float topPanelMinimumSize, float bottomPanelMinimumSize, GUIStyle dividerStyle, GUIStyle areaStyle, RectOffset border)
    {
        GUILayout.EndArea();
        Rect cursorChangeRect = new Rect(0, value - 3, area.width, 6);
        Rect visibleRect = new Rect(0, value - 1, area.width, 2);

        GUI.Box(cursorChangeRect, "", GUIStyle.none);
        GUI.Box(visibleRect, "", dividerStyle);

        EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);

        if (Event.current.type == EventType.mouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
        {
            resizing = true;
            Event.current.Use();
        }
        if (resizing && Event.current.type == EventType.MouseDrag)
        {
            value = Event.current.mousePosition.y;
            Event.current.Use();
        }
        if (Event.current.type == EventType.MouseUp)
        {
            resizing = false;
        }

        value = Mathf.Clamp(value, topPanelMinimumSize, area.height - bottomPanelMinimumSize);

        GUI.Box(new Rect(0, value + 1, area.width, area.height - value), "", areaStyle);
        Rect areaRect = new Rect(0 + border.left, value + border.top + 1, area.width - border.horizontal, area.height - value - border.vertical);
        GUILayout.BeginArea(areaRect, GUIStyle.none);
        return areaRect;
    }

    public static void EndVerticalResize()
    {
        GUILayout.EndArea();
    }

    public static Rect BeginHorizontalResize(Rect area, ref float value, GUIStyle style, RectOffset border)
    {
        Rect areaRect = new Rect(0 + border.left, 0 + border.top, value - border.horizontal, area.height - border.vertical);
        Rect visibleRect = new Rect(0, 0, value, area.height);
        GUI.Box(visibleRect, "", style);
        GUILayout.BeginArea(areaRect, GUIStyle.none);
        return areaRect;
    }

    public static Rect HorizontalDivider(Rect area, ref float value, ref bool resizing, float leftPanelMinimumSize, float rightPanelMinimumSize, GUIStyle dividerStyle, GUIStyle areaStyle, RectOffset border)
    {
        GUILayout.EndArea();
        Rect cursorChangeRect = new Rect(value - 3, 0, 6, area.height);
        Rect visibleRect = new Rect(value - 1, 0, 2, area.height);

        GUI.Box(cursorChangeRect, "", GUIStyle.none);
        GUI.Box(visibleRect, "", dividerStyle);

        EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeHorizontal);

        if (Event.current.type == EventType.mouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
        {
            resizing = true;
            Event.current.Use();
        }
        if (resizing && Event.current.type == EventType.MouseDrag)
        {
            value = Event.current.mousePosition.x;
            Event.current.Use();
        }
        if (Event.current.type == EventType.MouseUp)
        {
            resizing = false;
        }

        value = Mathf.Clamp(value, leftPanelMinimumSize, area.width - rightPanelMinimumSize);
        GUI.Box(new Rect(value, 0, area.width - value, area.height), "", areaStyle);
        Rect areaRect = new Rect(value + border.left, 0 + border.top, area.width - value - border.horizontal, area.height - border.vertical);
        GUILayout.BeginArea(areaRect, GUIStyle.none);
        return areaRect;
    }

    public static void EndHorizontalResize()
    {
        GUILayout.EndArea();
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



    /*
    public static FieldInfo LastControlIdField = typeof(EditorGUI).GetField("lastControlID", BindingFlags.Static | BindingFlags.NonPublic);
    public static int GetLastControlId()
    {
        if (LastControlIdField == null)
        {
            Debug.LogError("Compatibility with Unity broke: can't find lastControlId field in EditorGUI");
            return 0;
        }
        return (int)LastControlIdField.GetValue(null);
    }*/
}

