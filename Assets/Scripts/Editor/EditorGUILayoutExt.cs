using UnityEditor;
using UnityEngine;

public static class EditorStylesExt
{
    #region SpecialCharacters

    public static readonly string Checkmark = "✓";
    public static readonly string LeftTriangle = "◀";
    public static readonly string RightTriangle = "▶";
    public static readonly string UpTriangle = "▲";
    public static readonly string DownTriangle = "▼";


    #endregion

    #region Colors

    public static readonly Color EditorRed = new Color(1.0f, 0.72f, 0.72f);
    public static readonly Color EditorDarkRed = new Color(1.0f, 0.5f, 0.5f);

    public static readonly Color EditorGreen = new Color(0.72f, 1.0f, 0.72f);
    public static readonly Color EditorDarkGreen = new Color(0.6f, 0.85f, 0.6f);

    public static readonly Color EditorBlue = new Color(0.72f, 0.72f, 1.0f);

    public static readonly Color EditorDarkGray = new Color(0.5f, 0.5f, 0.5f);
    public static readonly Color EditorGray = new Color(0.72f, 0.72f, 0.72f);
    public static readonly Color EditorLightGray = new Color(0.85f, 0.85f, 0.85f);

    #endregion

    #region Styles

    public static GUIStyle ButtonLeft
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("ButtonLeft"); }
    }

    public static GUIStyle ButtonMiddle
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("ButtonMid"); }
    }

    public static GUIStyle ButtonRight
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("ButtonRight"); }
    }

    public static GUIStyle LockedHeaderButton
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("LockedHeaderButton"); }
    }

    public static GUIStyle ToolbarDropDown
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("ToolbarDropDown"); }
    }

    public static GUIStyle ToolbarButton
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("toolbarbutton"); }
    }

    public static GUIStyle ObjectField
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("ObjectField"); }
    }

    public static GUIStyle LODBlackBox
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("LODBlackBox"); }
    }

    public static GUIStyle PreBackground
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("PreBackground"); }
    }

    public static GUIStyle OLElem
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("OL Elem"); }
    }

    public static GUIStyle OLHeader
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("OL header"); }
    }

    public static GUIStyle OLPlus
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("OL Plus"); }
    }

    public static GUIStyle OLToggle
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("OL Toggle"); }
    }

    public static GUIStyle PRInsertion
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("PR Insertion"); }
    }

    public static GUIStyle GreyBorder
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("grey_border"); }
    }

    public static GUIStyle SearchTextField
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("SearchTextField"); }
    }

    public static GUIStyle SearchCancelButton
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("SearchCancelButton"); }
    }

    public static GUIStyle MiniButtonLeft
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("minibuttonleft"); }
    }

    public static GUIStyle MiniButtonMiddle
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("minibuttonmid"); }
    }

    public static GUIStyle MiniButtonRight
    {
        get { return EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("minibuttonright"); }
    }

    #endregion
}

