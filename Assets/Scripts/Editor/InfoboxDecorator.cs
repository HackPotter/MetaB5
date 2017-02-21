using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InfoboxAttribute))]
public class InfoboxDecorator : DecoratorDrawer
{

    public override float GetHeight()
    {
        var attrib = this.attribute as InfoboxAttribute;
        GUIStyle style = GUI.skin.GetStyle("HelpBox");
        return Mathf.Max(40f, style.CalcHeight(new GUIContent(attrib.Message), EditorGUIUtility.currentViewWidth));
    }

    public override void OnGUI(Rect position)
    {
        var attrib = this.attribute as InfoboxAttribute;
        EditorGUI.HelpBox(position, attrib.Message, MessageType.Info);
    }

}
