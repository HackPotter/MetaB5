using UnityEditor;
using UnityEngine;

public class DeleteSelected : EditorWindow
{
    [MenuItem("Metablast/Utility/Delete Random Selected")]
    private static void ShowDeleteSelectedWindow()
    {
        DeleteSelected window = EditorWindow.GetWindow<DeleteSelected>();
        window.ShowUtility();
    }

    private int _percentToDelete = 10;

    private System.Random random;

    void OnGUI()
    {
        if (random == null)
        {
            random = new System.Random();
        }
        
        _percentToDelete = EditorGUILayout.IntSlider("Percent to Delete", _percentToDelete, 0, 100);

        if (GUILayout.Button("Delete"))
        {
            //Undo.RegisterSceneUndo("Delete Random Selection");
            
            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            foreach (GameObject obj in Selection.gameObjects)
            {
                if (random.Next(100) <= _percentToDelete)
                {
                    Undo.DestroyObjectImmediate(obj);
                }
            }

            Undo.CollapseUndoOperations(group);
        }

    }
}

