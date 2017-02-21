using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveCubeMapToPngWizard : ScriptableWizard
{
#pragma warning disable 0649
    Cubemap cubemap;
#pragma warning restore 0649

    void OnWizardUpdate()
    {
        helpString = "Select cubemap to save to individual png";
        isValid = (cubemap != null);
    }

    void OnWizardCreate()
    {
        int width = cubemap.width;
        int height = cubemap.height;

        Debug.Log(Application.dataPath + "/" + cubemap.name + "_PositiveX.png");
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture        
        tex.SetPixels(cubemap.GetPixels(CubemapFace.PositiveX));
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_PositiveX.png", bytes);

        tex.SetPixels(cubemap.GetPixels(CubemapFace.NegativeX));
        bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_NegativeX.png", bytes);

        tex.SetPixels(cubemap.GetPixels(CubemapFace.PositiveY));
        bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_PositiveY.png", bytes);

        tex.SetPixels(cubemap.GetPixels(CubemapFace.NegativeY));
        bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_NegativeY.png", bytes);

        tex.SetPixels(cubemap.GetPixels(CubemapFace.PositiveZ));
        bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_PositiveZ.png", bytes);

        tex.SetPixels(cubemap.GetPixels(CubemapFace.NegativeZ));
        bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + cubemap.name + "_NegativeZ.png", bytes);


        DestroyImmediate(tex);
    }

    [MenuItem("Metablast/Utility/Save CubeMap To Png ")]
    static void SaveCubeMapToPng()
    {
        ScriptableWizard.DisplayWizard(
        "Save CubeMap To Png", typeof(SaveCubeMapToPngWizard), "Save");
    }
}
