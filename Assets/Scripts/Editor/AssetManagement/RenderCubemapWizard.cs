using UnityEditor;
using UnityEngine;

// Render scene from a given point into a static cube map.
// Place this script in Editor folder of your project.
// Then use the cubemap with one of Reflective shaders!
class RenderCubemapWizard : ScriptableWizard
{
#pragma warning disable 0649
    public Transform renderFromPosition;
    public Cubemap cubemap;
#pragma warning restore 0649

    void OnWizardUpdate()
    {
        helpString = "Select transform to render from and cubemap to render into";
        isValid = (renderFromPosition != null) && (cubemap != null);
    }

    void OnWizardCreate()
    {
        // create temporary camera for rendering
        GameObject go = new GameObject("CubemapCamera", typeof(Camera));
        // place it on the object
        go.transform.position = renderFromPosition.position;
        go.transform.rotation = Quaternion.identity;

        // render into cubemap
        go.GetComponent<Camera>().RenderToCubemap(cubemap);

        // destroy temporary camera
        DestroyImmediate(go);
    }

    [MenuItem("Metablast/Utility/Render into Cubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard<RenderCubemapWizard>(
        "Render cubemap", "Render!");
    }
}
