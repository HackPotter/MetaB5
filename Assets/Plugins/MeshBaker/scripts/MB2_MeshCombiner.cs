using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Text;

#if UNITY_EDITOR
	using UnityEditor;
#endif 
/*
//used to verify that MeshBaker and MeshCombiner implement similar functionality
public interface MB2_MeshCombinerInterface{
	string name { get; set; }
	MB2_TextureBakeResults textureBakeResults { get; set; }
	MB_RenderType renderType { get; set; }
	MB2_OutputOptions outputOption { get; set; }
	MB2_LightmapOptions lightmapOption { get; set; }
	bool doNorm { get; set; }
	bool doTan { get; set; }
	bool doCol { get; set; }
	bool doUV { get; set; }
	bool doUV1 { get; set; }
//	Mesh GetMesh();
//	bool doUV2();  
	List<GameObject> GetObjectsInCombined();
	int GetLightmapIndex();
	void Apply();
	void Apply(bool triangles,
						  bool vertices,
						  bool normals,
						  bool tangents,
						  bool uvs,
						  bool colors,
						  bool uv1,
						  bool uv2,
						  bool bones=false);
	void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true);	
	Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource, bool fixOutOfBoundUVs);
	bool CombinedMeshContains(GameObject go);
	void UpdateSkinnedMeshApproximateBounds();
	void ClearMesh();
	void DestroyMesh();	
}
*/
/// <summary>
/// Manages a single combined mesh.This class is the core of the mesh combining API.
/// 
/// It is not a component so it can be can be instantiated and used like a normal c sharp class.
/// </summary>
[System.Serializable]
public class MB2_MeshCombiner{
	static bool VERBOSE = false;

	[System.Serializable]
	class MB_DynamicGameObject:IComparable<MB_DynamicGameObject>{
		public GameObject go;
		public int vertIdx;
		public int numVerts;
		
		public int bonesIdx;
		public int numBones;
		
		public int lightmapIndex=-1;
		public Vector4 lightmapTilingOffset = new Vector4(1f,1f,0f,0f);
				
		/*
		 combined mesh will have one submesh per result material
		 source meshes can have any number of submeshes. They are mapped to a result submesh based on their material
		 if two different submeshes have the same material they are merged in the same result submesh  
		*/
		
		//These are result mesh submeshCount comine these into a class
		public int[] submeshTriIdxs;
		public int[] submeshNumTris;
		
		//These are source go mesh submeshCount todo combined these into a class
		///maps each submesh in sharedMesh to a target submesh in combined mesh
		public int[] targetSubmeshIdxs;  
		public Rect[] uvRects;
		public Rect[] obUVRects;		
		public List<int>[] _submeshTris;
		
		public Mesh sharedMesh;
		public bool _beingDeleted=false;
		public int  _triangleIdxAdjustment=0;
//		public Rect obUVRect = new Rect(0f,0f,1f,1f);
		
		public int CompareTo(MB_DynamicGameObject b){
			return this.vertIdx - b.vertIdx;
        }
	}	
	
	[SerializeField] bool _doNorm = true;
	[SerializeField] bool _doTan = true;
	[SerializeField] bool _doCol = true;
	[SerializeField] bool _doUV = true;
	[SerializeField] bool _doUV1 = true;
	
	[SerializeField] int lightmapIndex = -1;
	
	//this contains object instances that have been added to the combined mesh through AddDelete
	[SerializeField] List<GameObject> objectsInCombinedMesh = new List<GameObject>();
	[SerializeField] List<MB_DynamicGameObject> mbDynamicObjectsInCombinedMesh = new List<MB_DynamicGameObject>();
	Dictionary<GameObject,MB_DynamicGameObject> instance2combined_map = new Dictionary<GameObject, MB_DynamicGameObject>();
	
	[SerializeField] Vector3[] verts = new Vector3[0];
	[SerializeField] Vector3[] normals = new Vector3[0];
	[SerializeField] Vector4[] tangents = new Vector4[0];
	[SerializeField] Vector2[] uvs = new Vector2[0];
	[SerializeField] Vector2[] uv1s = new Vector2[0];
	[SerializeField] Vector2[] uv2s = new Vector2[0];
	[SerializeField] Color[] colors = new Color[0];
	[SerializeField] Matrix4x4[] bindPoses = new Matrix4x4[0];
	[SerializeField] Transform[] bones = new Transform[0];
	[SerializeField] Mesh _mesh; 
	
	//unity won't serialize these
	int[][] submeshTris = new int[0][];
	BoneWeight[] boneWeights = new BoneWeight[0];	
	
    GameObject[] empty = new GameObject[0];		
	
	[SerializeField] string __name;
	public string name { 
		get{return __name;}
		set{__name = value;}
	}
	
	[SerializeField] MB2_TextureBakeResults __textureBakeResults;
	public MB2_TextureBakeResults textureBakeResults { 
		get{return __textureBakeResults;} 
		set{
			if (objectsInCombinedMesh.Count > 0 && __textureBakeResults != value && __textureBakeResults != null){
				Debug.LogWarning("If material bake result is changed then objects currently in combined mesh may be invalid.");	
			}
			__textureBakeResults = value;
		}
	}
	
	[SerializeField] UnityEngine.Renderer __targetRenderer;
	public Renderer targetRenderer { 
		get{return __targetRenderer;}
		set{
			__targetRenderer = value;
		} 
	}
	
	[SerializeField] MB_RenderType __renderType;
	public MB_RenderType renderType { 
		get{return __renderType;} 
		set{
			if (value == MB_RenderType.skinnedMeshRenderer && __renderType == MB_RenderType.meshRenderer){
				if (boneWeights.Length != verts.Length) Debug.LogError("Can't set the render type to SkinnedMeshRenderer without clearing the mesh first. Try deleteing the CombinedMesh scene object.");
			}
			__renderType = value;
		} 
	}
	
	[SerializeField] MB2_OutputOptions __outputOption;
	public MB2_OutputOptions outputOption { 
		get{return __outputOption;} 
		set{__outputOption = value;} 
	}

	[SerializeField] MB2_LightmapOptions __lightmapOption;
	public MB2_LightmapOptions lightmapOption { 
		get{return __lightmapOption;} 
		set{
			if (objectsInCombinedMesh.Count > 0 && __lightmapOption != value){
				Debug.LogWarning("Can't change lightmap option once objects are in the combined mesh.");	
			}
			__lightmapOption = value;
		} 
	}
	
	[SerializeField] bool __doNorm;
	public bool doNorm { 
		get{return __doNorm;} 
		set{__doNorm = value;} 
	}
	
	[SerializeField] bool __doTan;
	public bool doTan { 
		get{return __doTan;} 
		set{__doTan = value;}
	}
	
	[SerializeField] bool __doCol;
	public bool doCol { 
		get{return __doCol;} 
		set{__doCol = value;}
	}
	
	[SerializeField] bool __doUV;
	public bool doUV { 
		get{return __doUV;} 
		set{__doUV = value;}
	}
	
	[SerializeField] bool __doUV1;
	public bool doUV1 { 
		get{return __doUV1;} 
		set{__doUV1 = value;}
	}		
	
	public bool doUV2(){
		return lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged || lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping;
	}

	public int GetNumObjectsInCombined(){
		return objectsInCombinedMesh.Count;		
	}	
	
	public List<GameObject> GetObjectsInCombined(){
		List<GameObject> outObs = new List<GameObject>();
		outObs.AddRange(objectsInCombinedMesh);
		return outObs;
	}
	
	public Mesh GetMesh(){
		if (_mesh == null) {
			_mesh = new Mesh();
//			_mesh.MarkDynamic();			
		}
		return _mesh;	
	}
	
	public Transform[] GetBones(){
		return bones;	
	}
	
	public int GetLightmapIndex(){
		if (lightmapOption == MB2_LightmapOptions.generate_new_UV2_layout || lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping){
			return lightmapIndex;
		} else {
			return -1;	
		}
	}
	
	public int GetNumVerticesFor(GameObject go){
		MB_DynamicGameObject dgo = null;
		if (instance2combined_map.TryGetValue(go, out dgo)){
			return dgo.numVerts;
		} else {
			return -1;
		}
	}
	
	void _initialize(){	
		if (objectsInCombinedMesh.Count == 0){
			lightmapIndex = -1;
		}
		if (_mesh == null){
			if (VERBOSE) Debug.Log("_initialize Creating new Mesh");
			_mesh = new Mesh();
		}		
		if (instance2combined_map.Keys.Count != objectsInCombinedMesh.Count){
			for (int i = 0; i < objectsInCombinedMesh.Count; i++){
				instance2combined_map.Add(objectsInCombinedMesh[i], mbDynamicObjectsInCombinedMesh[i]);
			}
			//BoneWeights are not serialized get from mesh
			boneWeights = _mesh.boneWeights;
			//submesh tris are not serialized either get from mesh
			submeshTris = new int[_mesh.subMeshCount][];
			for (int i = 0; i < submeshTris.Length; i++){
				submeshTris[i] = _mesh.GetTriangles(i);	
			}
		}
	}
	
	//todo add warnings if trying to use api with anything but scene object
	bool _collectMaterialTriangles(Mesh m, MB_DynamicGameObject dgo,Material[] sharedMaterials, OrderedDictionary sourceMats2submeshIdx_map){
		//everything here applies to the source object being added
		
		int numTriMeshes = m.subMeshCount;
		if (sharedMaterials.Length < numTriMeshes) numTriMeshes = sharedMaterials.Length;
		dgo._submeshTris = new List<int>[numTriMeshes];
		dgo.targetSubmeshIdxs = new int[numTriMeshes];
		for(int i = 0; i < numTriMeshes; i++){
			dgo._submeshTris[i] = new List<int>();
			if (textureBakeResults.doMultiMaterial){
				if (!sourceMats2submeshIdx_map.Contains(sharedMaterials[i])){
					Debug.LogError("Object " + dgo.go + " has a material that was not found in the result materials maping. " + sharedMaterials[i]);
					return false;
				}
				dgo.targetSubmeshIdxs[i] = (int) sourceMats2submeshIdx_map[sharedMaterials[i]];
			} else {
				dgo.targetSubmeshIdxs[i] = 0;
			}
			List<int> targTris = dgo._submeshTris[i];
			// add distinct triangles to master list
			int[] sourceTris = m.GetTriangles(i);
			for (int j = 0; j < sourceTris.Length; j++){
				targTris.Add(sourceTris[j]);	
			}
			if (VERBOSE) Debug.Log("Collecting triangles for: " + dgo.go.name + " submesh:" + i + " maps to submesh:" + dgo.targetSubmeshIdxs[i] + " added:" + sourceTris.Length);
		}
		return true;
	}
	
	bool _collectOutOfBoundsUVRects2(Mesh m, MB_DynamicGameObject dgo,Material[] sharedMaterials, OrderedDictionary sourceMats2submeshIdx_map){
		if (textureBakeResults == null){
			Debug.LogError("Need to bake textures into combined material");	
			return false;
		}
		int numTriMeshes = m.subMeshCount;
		if (sharedMaterials.Length < numTriMeshes) numTriMeshes = sharedMaterials.Length;
		dgo.obUVRects = new Rect[numTriMeshes];
		for(int i = 0; i < dgo.obUVRects.Length; i++){
			dgo.obUVRects[i] = new Rect(0f,0f,1f,1f);
		}		
		for(int i = 0; i < numTriMeshes; i++){
			Rect r = new Rect();
			MB_Utility.hasOutOfBoundsUVs(m,ref r,i);
			dgo.obUVRects[i] = r;
		}
		return true;		
	}	
	
	bool _validateTextureBakeResults(){
		if (textureBakeResults == null){
			Debug.LogError("Material Bake Results is null. Can't combine meshes.");	
			return false;
		}
		if (textureBakeResults.materials == null || textureBakeResults.materials.Length == 0){
			Debug.LogError("Material Bake Results has no materials in material to uvRect map. Try baking materials. Can't combine meshes.");	
			return false;			
		}
		if (textureBakeResults.doMultiMaterial){
			if (textureBakeResults.resultMaterials == null || textureBakeResults.resultMaterials.Length == 0){
				Debug.LogError("Material Bake Results has no result materials. Try baking materials. Can't combine meshes.");	
				return false;				
			}
		} else {
			if (textureBakeResults.resultMaterial == null){
				Debug.LogError("Material Bake Results has no result material. Try baking materials. Can't combine meshes.");	
				return false;				
			}
		}
		return true;
	}
	
	bool _validateMeshFlags(){
		if (objectsInCombinedMesh.Count > 0){
			if (_doNorm == false && doNorm == true ||
				_doTan == false && doTan == true ||
				_doCol == false && doCol == true ||
				_doUV == false && doUV == true ||
				_doUV1 == false && doUV1 == true){
				Debug.LogError("The channels have changed. There are already objects in the combined mesh that were added with a different set of channels.");
				return false;	
			}
		}
		_doNorm = doNorm;
		_doTan = doTan;
		_doCol = doCol;
		_doUV = doUV;
		_doUV1 = doUV1;
		return true;
	}
	
	bool getIsGameObjectActive(GameObject g){
#if UNITY_3_5
		return g.active;
#else
		return g.activeInHierarchy;
#endif
	}
	
	bool _addToCombined(GameObject[] goToAdd, GameObject[] goToDelete,bool disableRendererInSource){
		GameObject[] _goToAdd, _goToDelete;
		if (!_validateTextureBakeResults()) return false;
		if (!_validateMeshFlags()) return false;
		
		if (outputOption != MB2_OutputOptions.bakeMeshAssetsInPlace && renderType == MB_RenderType.skinnedMeshRenderer && (targetRenderer == null || !(__targetRenderer is SkinnedMeshRenderer))){
			Debug.LogError("Target renderer must be set and must be a SkinnedMeshRenderer");
			return false;
		}
		if (goToAdd == null) _goToAdd = empty;
		else _goToAdd = (GameObject[]) goToAdd.Clone();
		if (goToDelete == null) _goToDelete = empty;
		else _goToDelete = (GameObject[]) goToDelete.Clone();
		if (_mesh == null) DestroyMesh(); //cleanup maps and arrays
		
		Dictionary<Material,Rect> mat2rect_map = textureBakeResults.GetMat2RectMap();
		
		_initialize();
		
		int numResultMats = 1;
		if (textureBakeResults.doMultiMaterial) numResultMats = textureBakeResults.resultMaterials.Length;
		
		if (VERBOSE) Debug.Log("_addToCombined objs adding:" + _goToAdd.Length + " objs deleting:" + _goToDelete.Length + " fixOutOfBounds:" + textureBakeResults.fixOutOfBoundsUVs + " doMultiMaterial:" + textureBakeResults.doMultiMaterial + " disableRenderersInSource:" + disableRendererInSource);
		
		OrderedDictionary  sourceMats2submeshIdx_map = null;
		if (textureBakeResults.doMultiMaterial){
			//build the sourceMats to submesh index map
			sourceMats2submeshIdx_map = new OrderedDictionary ();
			for(int i = 0; i < numResultMats; i++){
				MB_MultiMaterial mm = textureBakeResults.resultMaterials[i];				
				for (int j = 0; j < mm.sourceMaterials.Count; j++){
					if (mm.sourceMaterials[j] == null){
						Debug.LogError("Found null material in source materials for combined mesh materials " + i);
						return false;
					}
					if (!sourceMats2submeshIdx_map.Contains(mm.sourceMaterials[j])){
						sourceMats2submeshIdx_map.Add(mm.sourceMaterials[j],i);
					}
				}
			}
		}
		
		if (submeshTris.Length != numResultMats){
			submeshTris = new int[numResultMats][];
			for (int i = 0; i < submeshTris.Length;i++) submeshTris[i] = new int[0];
		}
		
		//calculate num to delete
		int totalDeleteVerts = 0;
		int totalDeleteBones = 0;
		int[] totalDeleteSubmeshTris = new int[numResultMats];
		for (int i = 0; i < _goToDelete.Length; i++){
			MB_DynamicGameObject dgo;
			if(instance2combined_map.TryGetValue(_goToDelete[i],out dgo)){
				totalDeleteVerts += dgo.numVerts;
				if (renderType == MB_RenderType.skinnedMeshRenderer){
					totalDeleteBones += dgo.numBones;
				}
				for (int j = 0; j < dgo.submeshNumTris.Length; j++){
					totalDeleteSubmeshTris[j] += dgo.submeshNumTris[j];	
				}
			}else{
				Debug.LogWarning("Trying to delete an object that is not in combined mesh");	
			}
		}
		
		//now add
		List<MB_DynamicGameObject> toAddDGOs = new List<MB_DynamicGameObject>();
		int totalAddVerts = 0;
		int totalAddBones = 0;
		int[] totalAddSubmeshTris = new int[numResultMats];
		for (int i = 0; i < _goToAdd.Length; i++){
			// if not already in mesh or we are deleting and re-adding in same operation
			if(!instance2combined_map.ContainsKey(_goToAdd[i]) || Array.Find<GameObject>(_goToDelete,o => o == _goToAdd[i])){
				MB_DynamicGameObject dgo = new MB_DynamicGameObject();

				GameObject go = _goToAdd[i];
				
				Material[] sharedMaterials = MB_Utility.GetGOMaterials(go);

				if (sharedMaterials == null){
					Debug.LogError("Object " + go.name + " does not have a Renderer");
					_goToAdd[i] = null;
					return false;
				}

				Mesh m = MB_Utility.GetMesh(go);				
				if (m == null){
					Debug.LogError("Object " + go.name + " MeshFilter or SkinedMeshRenderer had no mesh");
					_goToAdd[i] = null;	
					return false;
				}
				
				Rect[] uvRects=new Rect[sharedMaterials.Length];
				for (int j = 0; j < sharedMaterials.Length; j++){
					if (!mat2rect_map.TryGetValue(sharedMaterials[j],out uvRects[j])){
						Debug.LogError("Object " + go.name + " has an unknown material " + sharedMaterials[j] + ". Try baking textures");
						_goToAdd[i] = null;			
					}
				}				
				if (_goToAdd[i] != null){
					dgo.go = _goToAdd[i];
					dgo.uvRects = uvRects;
					dgo.sharedMesh = m;
					dgo.numVerts = m.vertexCount;
					Renderer r = MB_Utility.GetRenderer(dgo.go);
					dgo.numBones = _getNumBones( r );
					
					if (lightmapIndex == -1){
						lightmapIndex = r.lightmapIndex; //initialize	
					}
					if (lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping){
						if (lightmapIndex != r.lightmapIndex){
							Debug.LogWarning("Object " + go.name + " has a different lightmap index. Lightmapping will not work.");						
						}
						if (!getIsGameObjectActive(dgo.go)){
							Debug.LogWarning("Object " + go.name + " is inactive. Can only get lightmap index of active objects.");													
						}
						if (r.lightmapIndex == -1){
							Debug.LogWarning("Object " + go.name + " does not have an index to a lightmap.");													
						}						
					}
					dgo.lightmapIndex = r.lightmapIndex;
					dgo.lightmapTilingOffset = r.lightmapScaleOffset;
					if (!_collectMaterialTriangles(m,dgo,sharedMaterials,sourceMats2submeshIdx_map)){
						return false;
					}
					dgo.submeshNumTris = new int[numResultMats];
					dgo.submeshTriIdxs = new int[numResultMats];

					if (textureBakeResults.fixOutOfBoundsUVs){
						if (!_collectOutOfBoundsUVRects2(m,dgo,sharedMaterials,sourceMats2submeshIdx_map)){
							return false;
						}
					}
					toAddDGOs.Add(dgo);
					totalAddVerts += dgo.numVerts;
					if (renderType == MB_RenderType.skinnedMeshRenderer) totalAddBones += dgo.numBones;
					
					for (int j = 0; j < dgo._submeshTris.Length; j++){
						totalAddSubmeshTris[dgo.targetSubmeshIdxs[j]] += dgo._submeshTris[j].Count;
					}			
				}
			}else{
				Debug.LogWarning("Object " + _goToAdd[i].name + " has already been added");
				_goToAdd[i] = null;
			}
		}
		
		for (int i = 0; i < _goToAdd.Length; i++){
			if (_goToAdd[i] != null && disableRendererInSource){ 
				MB_Utility.DisableRendererInSource(_goToAdd[i]);
			}
		}
		
		int newVertSize = verts.Length + totalAddVerts - totalDeleteVerts;
		int newBonesSize = bindPoses.Length + totalAddBones - totalDeleteBones;
		int[] newSubmeshTrisSize = new int[numResultMats];
		if (VERBOSE) Debug.Log("Verts adding:" +totalAddVerts + " deleting:" + totalDeleteVerts);

		if (VERBOSE) Debug.Log("Submeshes:" + newSubmeshTrisSize.Length);
		for (int i = 0; i < newSubmeshTrisSize.Length; i++){
			newSubmeshTrisSize[i] = submeshTris[i].Length + totalAddSubmeshTris[i] - totalDeleteSubmeshTris[i];	
			if (VERBOSE) Debug.Log ("    submesh :" + i + " already contains:" + submeshTris[i].Length + " trisAdded:" + totalAddSubmeshTris[i] + " trisDeleted:" + totalDeleteSubmeshTris[i]);
		}		
		
		if (newVertSize > 65534){
			Debug.LogError("Cannot add objects. Resulting mesh will have more than 64k vertices .");
			return false;				
		}		
		
		Vector3[] nnormals = null;
		Vector4[] ntangents = null;
		Vector2[] nuvs = null, nuv1s = null, nuv2s = null;
		Color[] ncolors = null;
		
		Vector3[] nverts = new Vector3[newVertSize];
		
		if (_doNorm) nnormals = new Vector3[newVertSize];
		if (_doTan) ntangents = new Vector4[newVertSize];
		if (_doUV) nuvs = new Vector2[newVertSize];
		if (_doUV1) nuv1s = new Vector2[newVertSize];		
		if (lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged ||
			lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping) nuv2s = new Vector2[newVertSize];
		if (_doCol) ncolors = new Color[newVertSize];
		
		BoneWeight[] nboneWeights = new BoneWeight[newVertSize];
		Matrix4x4[] nbindPoses = new Matrix4x4[newBonesSize];
		Transform[] nbones = new Transform[newBonesSize];
		int[][] nsubmeshTris = null;
		
		nsubmeshTris = new int[numResultMats][];
		for (int i = 0; i < nsubmeshTris.Length; i++){
			nsubmeshTris[i] = new int[newSubmeshTrisSize[i]];
		}
		
		for (int i = 0; i < _goToDelete.Length; i++){
			MB_DynamicGameObject dgo; 
			if (instance2combined_map.TryGetValue(_goToDelete[i], out dgo)){
				dgo._beingDeleted = true;
			}
		}		
		
		mbDynamicObjectsInCombinedMesh.Sort();
		
		//copy existing arrays to narrays gameobj by gameobj omitting deleted ones
		int targVidx = 0;
		int targBidx = 0;
		int[] targSubmeshTidx = new int[numResultMats];
		int triangleIdxAdjustment = 0;
		int boneIdxAdjustment = 0;
		for (int i = 0; i < mbDynamicObjectsInCombinedMesh.Count; i++){
			MB_DynamicGameObject dgo = mbDynamicObjectsInCombinedMesh[i];
			if (!dgo._beingDeleted){
				if (VERBOSE) Debug.Log("Copying obj in combined arrays idx:" + i);
				Array.Copy(verts,dgo.vertIdx,nverts,targVidx,dgo.numVerts);
				if (_doNorm) Array.Copy(normals,dgo.vertIdx,nnormals,targVidx,dgo.numVerts);
				if (_doTan) Array.Copy(tangents,dgo.vertIdx,ntangents,targVidx,dgo.numVerts);
				if (_doUV) Array.Copy(uvs,dgo.vertIdx,nuvs,targVidx,dgo.numVerts);
				if (_doUV1) Array.Copy(uv1s,dgo.vertIdx,nuv1s,targVidx,dgo.numVerts);
				if (doUV2()){
					Array.Copy(uv2s,dgo.vertIdx,nuv2s,targVidx,dgo.numVerts);
				}
				if (_doCol) Array.Copy(colors,dgo.vertIdx,ncolors,targVidx,dgo.numVerts);
				
				if (renderType == MB_RenderType.skinnedMeshRenderer){
					for (int j = dgo.vertIdx; j < dgo.vertIdx + dgo.numVerts; j++){
						boneWeights[j].boneIndex0 = boneWeights[j].boneIndex0 - boneIdxAdjustment;
						boneWeights[j].boneIndex1 = boneWeights[j].boneIndex1 - boneIdxAdjustment;
						boneWeights[j].boneIndex2 = boneWeights[j].boneIndex2 - boneIdxAdjustment;
						boneWeights[j].boneIndex3 = boneWeights[j].boneIndex3 - boneIdxAdjustment;
					}
					Array.Copy(boneWeights,dgo.vertIdx,nboneWeights,targVidx,dgo.numVerts);
				}
				
				Array.Copy(bindPoses,dgo.bonesIdx,nbindPoses,targBidx,dgo.numBones);
				Array.Copy(bones,dgo.bonesIdx,nbones,targBidx,dgo.numBones);
				//adjust triangles, then copy them over

				for (int subIdx = 0; subIdx < numResultMats; subIdx++){
					int[] sTris = submeshTris[subIdx];
					int sTriIdx = dgo.submeshTriIdxs[subIdx];
					int sNumTris = dgo.submeshNumTris[subIdx];
					if (VERBOSE) Debug.Log("    Adjusting submesh triangles submesh:"+subIdx+" startIdx:"+sTriIdx+" num:"+sNumTris);
					for (int j = sTriIdx; j < sTriIdx + sNumTris; j++){
						sTris[j] = sTris[j] - triangleIdxAdjustment;
					}								
					Array.Copy(sTris,sTriIdx,nsubmeshTris[subIdx],targSubmeshTidx[subIdx],sNumTris);
				}
				
				dgo.bonesIdx = targBidx;
				dgo.vertIdx = targVidx;

				for (int j = 0; j < targSubmeshTidx.Length; j++){
					dgo.submeshTriIdxs[j] = targSubmeshTidx[j];
					targSubmeshTidx[j] += dgo.submeshNumTris[j];
				}
				targBidx += dgo.numBones;				
				targVidx += dgo.numVerts;
			} else {
				if (VERBOSE) Debug.Log("Not copying obj: " + i);
				triangleIdxAdjustment += dgo.numVerts;
				boneIdxAdjustment += dgo.numBones;
			}
		}
		
		for (int i = mbDynamicObjectsInCombinedMesh.Count - 1; i >= 0;i--){
			if (mbDynamicObjectsInCombinedMesh[i]._beingDeleted){
				instance2combined_map.Remove(mbDynamicObjectsInCombinedMesh[i].go);
				objectsInCombinedMesh.RemoveAt(i);
				mbDynamicObjectsInCombinedMesh.RemoveAt(i);	
			}
		}

		verts = nverts;
		if (_doNorm) normals = nnormals;
		if (_doTan) tangents = ntangents;
		if (_doUV) uvs = nuvs;
		if (_doUV1) uv1s = nuv1s;
		if (doUV2()) uv2s = nuv2s;
		if (_doCol) colors = ncolors;
		if (renderType == MB_RenderType.skinnedMeshRenderer) boneWeights = nboneWeights;
		bindPoses = nbindPoses;
		bones = nbones;
		submeshTris = nsubmeshTris;
		
		//add new
		for (int i = 0; i < toAddDGOs.Count; i++){
			MB_DynamicGameObject dgo = toAddDGOs[i];
			GameObject go = dgo.go;
			int vertsIdx = targVidx;
			int bonesIdx = targBidx;
					
			Mesh mesh = dgo.sharedMesh;
			Matrix4x4 l2wMat = go.transform.localToWorldMatrix;
			Quaternion l2wQ = go.transform.rotation;
			nverts = mesh.vertices;
			Vector3[] nnorms = null; 
			Vector4[] ntangs = null;
			if (_doNorm) nnorms = _getMeshNormals(mesh);
			if (_doTan) ntangs = _getMeshTangents(mesh);
			if (renderType != MB_RenderType.skinnedMeshRenderer){ //for skinned meshes leave in bind pose
				for (int j = 0; j < nverts.Length; j++){
					nverts[j] = l2wMat.MultiplyPoint(nverts[j]);
					if (_doNorm) nnorms[j] = l2wQ * nnorms[j];
					if (_doTan){
						float w = ntangs[j].w; //need to preserve the w value
						ntangs[j] = l2wQ * ntangs[j];
						ntangs[j].w = w;
					}
				}
			}
			if (_doNorm) nnorms.CopyTo(normals,vertsIdx);
			if (_doTan) ntangs.CopyTo(tangents,vertsIdx);							
			nverts.CopyTo(verts,vertsIdx);
			
			int numTriSets = mesh.subMeshCount;
			if (dgo.uvRects.Length < numTriSets){
				Debug.LogWarning("Mesh " + dgo.go.name + " has more submeshes than materials");
				numTriSets = dgo.uvRects.Length;
			} else if (dgo.uvRects.Length > numTriSets){
				Debug.LogWarning("Mesh " + dgo.go.name + " has fewer submeshes than materials");
			}
			
			if (_doUV){
				nuvs = _getMeshUVs(mesh);
				int[] done = new int[nuvs.Length]; //use this to track uvs that have already been adjusted don't adjust twice
				for (int l = 0; l < done.Length; l++) done[l] = -1;
	 					
				bool triangleArraysOverlap = false;
				Rect obUVRect = new Rect();
				for (int k = 0; k < dgo._submeshTris.Length; k++){
					List<int> subTris = dgo._submeshTris[k];
					Rect uvRect = dgo.uvRects[k];
					if (textureBakeResults.fixOutOfBoundsUVs) obUVRect = dgo.obUVRects[k];
					for (int l = 0; l < subTris.Count; l++){
						int vidx = subTris[l];
						if (done[vidx] == -1){
							done[vidx] = k; //prevents a uv from being adjusted twice
							if (textureBakeResults.fixOutOfBoundsUVs){
								nuvs[vidx].x = nuvs[vidx].x / obUVRect.width - obUVRect.x/obUVRect.width;
								nuvs[vidx].y = nuvs[vidx].y / obUVRect.height - obUVRect.y/obUVRect.height;
							}
							nuvs[vidx].x = uvRect.x + nuvs[vidx].x*uvRect.width;
							nuvs[vidx].y = uvRect.y + nuvs[vidx].y*uvRect.height;
						}
						if (done[vidx] != k){
							triangleArraysOverlap = true;	
						}
					}
				}
				if (triangleArraysOverlap){
					Debug.LogWarning(dgo.go.name + "has submeshes which share verticies. Adjusted uvs may not map correctly in combined atlas.");	
				}
				nuvs.CopyTo(uvs,vertsIdx);
			}
			
			if (doUV2()){
				nuv2s = _getMeshUV2s(mesh);			
				if (lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping){ //has a lightmap
					Vector2 uvscale2;
					Vector4 lightmapTilingOffset = dgo.lightmapTilingOffset;
					Vector2 uvscale = new Vector2( lightmapTilingOffset.x, lightmapTilingOffset.y );	
					Vector2 uvoffset = new Vector2( lightmapTilingOffset.z, lightmapTilingOffset.w );
					for ( int j = 0; j < nuv2s.Length; j++ ) {
						uvscale2.x = uvscale.x * nuv2s[j].x;
						uvscale2.y = uvscale.y * nuv2s[j].y;
						nuv2s[j] = uvoffset + uvscale2;
					}
				}
				nuv2s.CopyTo(uv2s,vertsIdx);
			}
			
			if (_doUV1){
				nuv1s = _getMeshUV1s(mesh);
				nuv1s.CopyTo(uv1s,vertsIdx);
			}			
			
			if (_doCol){
				ncolors = _getMeshColors(mesh);
				ncolors.CopyTo(colors,vertsIdx);
			}
			
			if (renderType == MB_RenderType.skinnedMeshRenderer){
				Renderer r = MB_Utility.GetRenderer(dgo.go);
				nbones = _getBones(r);
				nbones.CopyTo(bones,bonesIdx);
				
				nbindPoses = _getBindPoses(r);
				nbindPoses.CopyTo(bindPoses,bonesIdx);
				
				nboneWeights = _getBoneWeights(r,dgo.numVerts);
				for (int j = 0; j < nboneWeights.Length; j++){
					nboneWeights[j].boneIndex0 = nboneWeights[j].boneIndex0 + bonesIdx;	
					nboneWeights[j].boneIndex1 = nboneWeights[j].boneIndex1 + bonesIdx;	
					nboneWeights[j].boneIndex2 = nboneWeights[j].boneIndex2 + bonesIdx;	
					nboneWeights[j].boneIndex3 = nboneWeights[j].boneIndex3 + bonesIdx;	
				}
				nboneWeights.CopyTo(boneWeights,vertsIdx);				
			}						
			
			for (int combinedMeshIdx = 0; combinedMeshIdx < targSubmeshTidx.Length; combinedMeshIdx++){
				dgo.submeshTriIdxs[combinedMeshIdx] = targSubmeshTidx[combinedMeshIdx];
			}
			for (int j = 0; j < dgo._submeshTris.Length; j++){
				List<int> sts = dgo._submeshTris[j];
				for (int k = 0; k < sts.Count; k++){
					sts[k] = sts[k] + vertsIdx;
				}
				int combinedMeshIdx = dgo.targetSubmeshIdxs[j];
				sts.CopyTo(submeshTris[combinedMeshIdx], targSubmeshTidx[combinedMeshIdx]);
				dgo.submeshNumTris[combinedMeshIdx] += sts.Count;
				targSubmeshTidx[combinedMeshIdx] += sts.Count;
			}
						
			dgo.vertIdx = targVidx;
			dgo.bonesIdx = targBidx;
			
			instance2combined_map.Add(go,dgo);
			objectsInCombinedMesh.Add(go);
			mbDynamicObjectsInCombinedMesh.Add(dgo);

			targVidx += nverts.Length;
			targBidx += nbindPoses.Length;
			for (int j = 0; j < dgo._submeshTris.Length; j++) dgo._submeshTris[j].Clear();
			dgo._submeshTris = null;
			if (VERBOSE) Debug.Log("Added to combined:" + dgo.go.name + " verts:" + nverts.Length);
		}	
		return true;
	}
	
	Color[] _getMeshColors(Mesh m){
		Color[] cs = m.colors;
		if (cs.Length == 0){
			if (VERBOSE) Debug.Log("Mesh " + m + " has no colors. Generating");
			if (_doCol) Debug.LogWarning("Mesh " + m + " didn't have colors. Generating an array of white colors");
			cs = new Color[m.vertexCount];
			for (int i = 0; i < cs.Length; i++){cs[i] = Color.white;}
		}
		return cs;
	}

	Vector3[] _getMeshNormals(Mesh m){
		Vector3[] ns = m.normals;
		if (ns.Length == 0){
			if (VERBOSE) Debug.Log("Mesh " + m + " has no normals. Generating");
			Debug.LogWarning("Mesh " + m + " didn't have normals. Generating normals.");
			Mesh tempMesh = (Mesh) GameObject.Instantiate(m);
			tempMesh.RecalculateNormals();
			ns = tempMesh.normals;
			MB_Utility.Destroy(tempMesh);				
		}
		return ns;		
	}	

	Vector4[] _getMeshTangents(Mesh m){
		Vector4[] ts = m.tangents;
		if (ts.Length == 0){
			if (VERBOSE) Debug.Log("Mesh " + m + " has no tangents. Generating");
			Debug.LogWarning("Mesh " + m + " didn't have tangents. Generating tangents.");			
			Vector3[] verts = m.vertices;
			Vector2[] uvs = _getMeshUVs(m);
			Vector3[] norms = _getMeshNormals(m);
			ts = new Vector4[m.vertexCount];
			for (int i = 0; i < m.subMeshCount; i++){
				int[] tris = m.GetTriangles(i);
				_generateTangents(tris,verts,uvs,norms,ts);
			}
		}
		return ts;
	}	
	
	Vector2 _HALF_UV = new Vector2(.5f, .5f);
	Vector2[] _getMeshUVs(Mesh m){
		Vector2[] uv = m.uv;
		if (uv.Length == 0){
			//todo if editor use the unwrapping class to generate UVs
			if (VERBOSE) Debug.Log("Mesh " + m + " has no uvs. Generating");
			Debug.LogWarning("Mesh " + m + " didn't have uvs. Generating uvs.");
			uv = new Vector2[m.vertexCount];
			for (int i = 0; i < uv.Length; i++){uv[i] = _HALF_UV;}			
		}		
		return uv;		
	}

	Vector2[] _getMeshUV1s(Mesh m){
		Vector2[] uv = m.uv2;
		if (uv.Length == 0){
			if (VERBOSE) Debug.Log("Mesh " + m + " has no uv1s. Generating");
			Debug.LogWarning("Mesh " + m + " didn't have uv1s. Generating uv1s.");			
			uv = new Vector2[m.vertexCount];
			for (int i = 0; i < uv.Length; i++){uv[i] = _HALF_UV;}
		}		
		return uv;		
	}

	Vector2[] _getMeshUV2s(Mesh m){
		Vector2[] uv = m.uv2;
		if (uv.Length == 0){
			if (VERBOSE) Debug.Log("Mesh " + m + " has no uv2s. Generating");
			Debug.LogWarning("Mesh " + m + " didn't have uv2s. Lightmapping option was set to " + lightmapOption + " Generating uv2s.");			
			uv = new Vector2[m.vertexCount];
			for (int i = 0; i < uv.Length; i++){uv[i] = _HALF_UV;}
		}		
		return uv;		
	}	
	
	public void UpdateSkinnedMeshApproximateBounds(){
		Vector3 max, min;
		if (outputOption == MB2_OutputOptions.bakeMeshAssetsInPlace){
			Debug.LogWarning("Can't UpdateSkinnedMeshApproximateBounds when output type is bakeMeshAssetsInPlace");
			return;
		}
		if (bones.Length == 0){
			Debug.LogWarning("No bones in SkinnedMeshRenderer. Could not UpdateSkinnedMeshApproximateBounds.");
			return;
		}
		if (targetRenderer == null){
			Debug.LogWarning("Target Renderer is not set. No point in calling UpdateSkinnedMeshApproximateBounds.");
			return;			
		}
		if (!__targetRenderer.GetType().Equals( typeof(SkinnedMeshRenderer) )){
			Debug.LogWarning("Target Renderer is not a SkinnedMeshRenderer. No point in calling UpdateSkinnedMeshApproximateBounds.");
			return;			
		}
		
		SkinnedMeshRenderer smr = (SkinnedMeshRenderer) targetRenderer;	

		max = bones[0].position;
		min = bones[0].position;
		for (int i = 1; i < bones.Length; i++){
			Vector3 v = bones[i].position;
			if (v.x < min.x) min.x = v.x;
			if (v.y < min.y) min.y = v.y;
			if (v.z < min.z) min.z = v.z;
			if (v.x > max.x) max.x = v.x;
			if (v.y > max.y) max.y = v.y;
			if (v.z > max.z) max.z = v.z;			
		}
		Vector3 center = (max + min)/2f;
		Vector3 size = max - min;
		Matrix4x4 w2l = smr.worldToLocalMatrix;
		Bounds b = new Bounds(w2l * center, w2l * size);		
		smr.localBounds = b;
	}
	
	int _getNumBones(Renderer r){
		if (renderType == MB_RenderType.skinnedMeshRenderer){
			if (r is SkinnedMeshRenderer){
				return ((SkinnedMeshRenderer)r).bones.Length;	
			} else if (r is MeshRenderer){
				return 1;
			} else {
				Debug.LogError("Could not _getNumBones. Object does not have a renderer");
				return 0;
			}
		} else {
			return 0;	
		}
	}
	
	Transform[] _getBones(Renderer r){
		if (r is SkinnedMeshRenderer){
			return ((SkinnedMeshRenderer)r).bones;	
		} else if (r is MeshRenderer){
			Transform[] bone = new Transform[1];
			bone[0] = r.transform;
			return bone;
		} else {
			Debug.LogError("Could not getBones. Object does not have a renderer");
			return null;
		}
	}
	
	Matrix4x4[] _getBindPoses(Renderer r){
		if (r is SkinnedMeshRenderer){
			return ((SkinnedMeshRenderer)r).sharedMesh.bindposes;	
		} else if (r is MeshRenderer){
			Matrix4x4 bindPose = Matrix4x4.identity;
			Matrix4x4[] poses = new Matrix4x4[1];
			poses[0] = bindPose;
			return poses;
		} else {
			Debug.LogError("Could not _getBindPoses. Object does not have a renderer");
			return null;
		}		
	}
	
	BoneWeight[] _getBoneWeights(Renderer r,int numVerts){
		if (r is SkinnedMeshRenderer){
			return ((SkinnedMeshRenderer)r).sharedMesh.boneWeights;	
		} else if (r is MeshRenderer){
			BoneWeight bw = new BoneWeight();
			bw.boneIndex0 = bw.boneIndex1 = bw.boneIndex2 = bw.boneIndex3 = 0;
			bw.weight0 = 1f;
			bw.weight1 = bw.weight2 = bw.weight3 = 0f;
			BoneWeight[] bws = new BoneWeight[numVerts];
			for (int i = 0; i < bws.Length; i++) bws[i] = bw;
			return bws;
		} else {
			Debug.LogError("Could not _getBoneWeights. Object does not have a renderer");
			return null;
		}			
	}
	
	public void Apply(){
		bool doBones = false;
		if (renderType == MB_RenderType.skinnedMeshRenderer) doBones = true;
		Apply(true,true,_doNorm,_doTan,_doUV,_doCol,_doUV1,doUV2(),doBones);
	}
	
	[Obsolete("ApplyAll is deprecated, please use Apply instead.")]
	public void ApplyAll(){
		Apply(true,true,_doNorm,_doTan,_doUV,_doCol,_doUV1,doUV2(),true);
	}
	
	public void Apply(bool triangles,
					  bool vertices,
					  bool normals,
					  bool tangents,
					  bool uvs,
					  bool colors,
					  bool uv1,
					  bool uv2,
					  bool bones=false){
		if (_mesh != null){
			if (_mesh.vertexCount != verts.Length){
#if UNITY_3_5
				_mesh.Clear();
#else
				_mesh.Clear(false); //clear all the data and start with a blank mesh
#endif
			} else if (triangles){ 
				_mesh.Clear(); //only clear the triangles	
			}
			if (vertices)  _mesh.vertices = verts;
			if (triangles){
				_mesh.triangles = submeshTris[0];
				if (textureBakeResults.doMultiMaterial){
					_mesh.subMeshCount = submeshTris.Length;
					for (int i = 0; i < submeshTris.Length; i++){
						_mesh.SetTriangles(submeshTris[i],i);
					}
				}
			}
			if (normals){
				if (_doNorm) { _mesh.normals = this.normals; }
				else { Debug.LogError("normal flag was set in Apply but MeshBaker didn't generate normals"); }
			}
			
			if (tangents){
				if (_doTan) {_mesh.tangents = this.tangents; }
				else { Debug.LogError("tangent flag was set in Apply but MeshBaker didn't generate tangents"); }
			}
			if (uvs){
				if (_doUV) {_mesh.uv = this.uvs; }
				else { Debug.LogError("uv flag was set in Apply but MeshBaker didn't generate uvs"); }
			}
			if (colors){
				if (_doCol) {_mesh.colors = this.colors; }
				else { Debug.LogError("color flag was set in Apply but MeshBaker didn't generate colors"); }
			}
			if (uv1){
				if (_doUV1) { _mesh.uv2 = this.uv1s; }
				else { Debug.LogError("uv1 flag was set in Apply but MeshBaker didn't generate uv1s"); }	
			}
			if (uv2){
				if (doUV2()){_mesh.uv2 = this.uv2s; }
				else { Debug.LogError("uv2 flag was set in Apply but lightmapping option was set to " + lightmapOption); }					
			}
			
			bool do_generate_new_UV2_layout = false;
#if UNITY_EDITOR
			if (renderType != MB_RenderType.skinnedMeshRenderer && lightmapOption == MB2_LightmapOptions.generate_new_UV2_layout){
				Unwrapping.GenerateSecondaryUVSet(_mesh);
				do_generate_new_UV2_layout = true;
			} else if (renderType == MB_RenderType.skinnedMeshRenderer && lightmapOption == MB2_LightmapOptions.generate_new_UV2_layout) {
				Debug.LogWarning("UV2 cannot be generated for SkinnedMeshRenderer objects.");
			}
#endif		
			if (renderType != MB_RenderType.skinnedMeshRenderer && lightmapOption == MB2_LightmapOptions.generate_new_UV2_layout && do_generate_new_UV2_layout == false){
				Debug.LogError("Failed to generate new UV2 layout. Only works in editor.");
			}

			if (bones){
				_mesh.bindposes = this.bindPoses;
				_mesh.boneWeights = this.boneWeights;
			}
			if (triangles || vertices) _mesh.RecalculateBounds();			
		} else {
			Debug.LogError("Need to add objects to this meshbaker before calling Apply or ApplyAll");	
		}
	}
	
	public void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true){		
		_updateGameObjects(gos, recalcBounds);
	}
	
	void _updateGameObjects(GameObject[] gos, bool recalcBounds){
		_initialize();		
		for (int i = 0; i < gos.Length; i++){
			_updateGameObject(gos[i],false);
		}
		if (recalcBounds)
			_mesh.RecalculateBounds();
	}
	
	void _updateGameObject(GameObject go, bool recalcBounds){
			MB_DynamicGameObject dgo;
			if (!instance2combined_map.TryGetValue(go,out dgo)){
				Debug.LogError("Object " + go.name + " has not been added");
				return;
			}
			Mesh mesh = dgo.sharedMesh;
			if (dgo.numVerts != mesh.vertexCount){
				Debug.LogError("Object " + go.name + " source mesh has been modified since being added");
				return;			
			}
			Matrix4x4 l2wMat = go.transform.localToWorldMatrix;
			Quaternion l2wQ = go.transform.rotation;

			Vector3[] nverts = mesh.vertices;
			Vector3[] nnorms = mesh.normals;
			Vector4[] ntangs = mesh.tangents;
			for (int j = 0; j < nverts.Length; j++){
				int midx = dgo.vertIdx + j;
				verts[midx] = l2wMat.MultiplyPoint3x4(nverts[j]);
				if (_doNorm) normals[midx] = l2wQ * nnorms[j];
				if (_doTan){
					float w = ntangs[j].w;
					tangents[midx] = l2wQ * ntangs[j];
					tangents[midx].w = w;
				}
			}
	}

	public Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs){
		return AddDeleteGameObjects(gos,deleteGOs,true,textureBakeResults.fixOutOfBoundsUVs);
	}
	
	public Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource){		
		return AddDeleteGameObjects(gos,deleteGOs,disableRendererInSource,textureBakeResults.fixOutOfBoundsUVs);
	}
	
	public Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource, bool fixOutOfBoundUVs){
		//check for duplicates
		if (gos != null){
			for (int i = 0; i < gos.Length; i++){
				if (gos[i] == null){
					Debug.LogError("The " + i + "th object on the list of objects to combine is 'None'. Use Command-Delete on Mac OS X; Delete or Shift-Delete on Windows to remove this one element.");
					return null;					
				}
				for (int j = i + 1; j < gos.Length; j++){
					if (gos[i] == gos[j]){
						Debug.LogError("GameObject " + gos[i] + "appears twice in list of game objects to add");
						return null;
					}
				}
			}
		}
		if (deleteGOs != null){
			for (int i = 0; i < deleteGOs.Length; i++){
//				if (deleteGOs[i] == null){
//					Debug.LogError("The " + i + "th object on the list of objects to delete is 'None'.");
//					return null;					
//				}				
				for (int j = i + 1; j < deleteGOs.Length; j++){
					if (deleteGOs[i] == deleteGOs[j] && deleteGOs[i] != null){
						Debug.LogError("GameObject " + deleteGOs[i] + "appears twice in list of game objects to delete");
						return null;
					}
				}
			}
		}
		if (!_addToCombined(gos,deleteGOs,disableRendererInSource)){
			Debug.LogError("Failed to add/delete objects to combined mesh");
			return null;
		}
		if (renderType == MB_RenderType.skinnedMeshRenderer && targetRenderer != null && outputOption != MB2_OutputOptions.bakeMeshAssetsInPlace){
			SkinnedMeshRenderer smr = (SkinnedMeshRenderer) targetRenderer;
			smr.bones = bones;
			UpdateSkinnedMeshApproximateBounds();
		}		
		return _mesh;
	}
	
	public bool CombinedMeshContains(GameObject go){
		return objectsInCombinedMesh.Contains(go);
	}
	
	void _clearArrays(){
		verts = new Vector3[0];
		normals = new Vector3[0];
		tangents = new Vector4[0];
		uvs = new Vector2[0];
		uv1s = new Vector2[0];
		uv2s = new Vector2[0];
		colors = new Color[0];
		bones = new Transform[0];
		bindPoses = new Matrix4x4[0];
		boneWeights = new BoneWeight[0];
		submeshTris = new int[0][];
		
		mbDynamicObjectsInCombinedMesh.Clear();
		objectsInCombinedMesh.Clear();
		instance2combined_map.Clear();		
	}
	
	/*
	 * Empties all channels and clears the mesh
	 */
	public void ClearMesh(){
		if (_mesh != null){
#if UNITY_3_5
				_mesh.Clear();
#else
				_mesh.Clear(false); //clear all the data and start with a blank mesh
#endif
		} else {
			_mesh = new Mesh();	
		}	
		_clearArrays();
	}

	/*
	 * Empties all channels, destroys the mesh and replaces it with a new mesh
	 */	
	public void DestroyMesh(){
		if (_mesh != null){
			if (VERBOSE) Debug.Log("Destroying Mesh");
			MB_Utility.Destroy(_mesh);
		}
		_mesh = new Mesh();
		_clearArrays();	
	}
	
    void _generateTangents(int[] triangles, Vector3[] verts, Vector2[] uvs, Vector3[] normals, Vector4[] outTangents){
        int triangleCount = triangles.Length;
        int vertexCount = verts.Length;

        Vector3[] tan1 = new Vector3[vertexCount];
        Vector3[] tan2 = new Vector3[vertexCount];

        for(int a = 0; a < triangleCount; a+=3)
        {
            int i1 = triangles[a+0];
            int i2 = triangles[a+1];
            int i3 = triangles[a+2];

            Vector3 v1 = verts[i1];
            Vector3 v2 = verts[i2];
            Vector3 v3 = verts[i3];

            Vector2 w1 = uvs[i1];
            Vector2 w2 = uvs[i2];
            Vector2 w3 = uvs[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;
			
			float rBot = (s1 * t2 - s2 * t1);
			if (rBot == 0f){
				Debug.LogError("Mesh contains degenerate UVs. Could not compute tangents.");
				return;
			}
            float r = 1.0f / rBot;

            Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }


        for (int a = 0; a < vertexCount; ++a)
        {
            Vector3 n = normals[a];
            Vector3 t = tan1[a];

            Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
            outTangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
            outTangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
        }
    }	

	public void SaveMeshsToAssetDatabase(string folderPath,string newFileNameBase){
#if UNITY_EDITOR
		string newFilename = newFileNameBase + ".asset";
		string ap = AssetDatabase.GetAssetPath(_mesh);
		if (ap == null || ap.Equals("")){
			Debug.Log("Saving mesh asset to " + newFilename);
			AssetDatabase.CreateAsset(_mesh, newFilename);
		} else {
			Debug.Log("Mesh is an asset at " + ap);	
		}
#else
		Debug.LogError("Can only save meshes in the editor");
#endif
	}
	
	public void RebuildPrefab(GameObject prefabRoot){
#if UNITY_EDITOR
		GameObject rootGO = (GameObject) PrefabUtility.InstantiatePrefab(prefabRoot);
		MB_Utility.buildSceneMeshObject(rootGO,this,_mesh,true);		
		string prefabPth = AssetDatabase.GetAssetPath(prefabRoot);
		PrefabUtility.ReplacePrefab(rootGO,AssetDatabase.LoadAssetAtPath(prefabPth,typeof(GameObject)),ReplacePrefabOptions.ConnectToPrefab);
		Editor.DestroyImmediate(rootGO);
#else
		Debug.LogError("Can only rebuild prefabs in the editor");		
#endif
	}	
}
