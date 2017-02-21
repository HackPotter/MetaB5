using UnityEngine;
using UnityEditor;
using System.Collections;

class MB2_ImportWarning:AssetPostprocessor {

    static void OnPostprocessAllAssets (
         string[] importedAssets,
         string[] deletedAssets,
         string[] movedAssets,
         string[] movedFromAssetPaths) {
			foreach(string s in importedAssets){
				if (s.Equals("Assets/MeshBaker/scripts/MB2_MeshBaker.cs")){
					EditorUtility.DisplayDialog("Important MeshBaker Changes", 
												"Version 2.5 changes the way MB2_MeshBaker components store information internally.\n\n" +
												"If you have pre-existing combined meshes in your scenes that were created with a previous version " +
												" of MeshBaker, you will need to re-bake MB2_MeshBaker Components in your scenes to restore their internal state.\n\n" +
												"This is only necessary if you use the API to modify these combined meshes at runtime. ", "ok");
				}
			}
		}
    }
