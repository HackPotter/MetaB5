using UnityEditor;
using UnityEngine;

// A class to to return the total triangle count of selected GameObjects
public class GetTriangles : ScriptableWizard
{
    private int triCount = 0;
    private int subMeshes = 0;

    void OnWizardUpdate()
    {
        helpString = "Select Game Obects";
    }

    void OnWizardCreate()
    {
        //Save the selected GameObjects
        GameObject[] gos = Selection.gameObjects;

        //for each GameObject selected...
        foreach (var go in gos)
        {
            //if this GameObject or any children has a MeshFilter
            if (go.GetComponentsInChildren<MeshFilter>().Length > 0)
            {

                //store an array of the children that have a MeshFilter
                Component[] meshes;
                meshes = go.GetComponentsInChildren<MeshFilter>();

                //add the triangles of each mesh in the array
                for (int i = 0; i < meshes.Length; i++)
                {
                    Mesh mesh = meshes[i].GetComponent<MeshFilter>().sharedMesh;
                    subMeshes += mesh.subMeshCount;
                    triCount += mesh.triangles.Length / 3;
                }
            }
        }
        EditorUtility.DisplayDialog("Triangle count = " + triCount.ToString(), "From " + subMeshes + " meshes", "OK");
    }

    [MenuItem("Metablast/Utility/Get Triangle Count", false)]
    static void getTriangles()
    {
        ScriptableWizard.DisplayWizard(
            "GetTriangles", typeof(GetTriangles), "Count!");
    }
}
