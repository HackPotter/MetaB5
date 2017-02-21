
using UnityEngine;
using System;
using System.Reflection;

#if !UNITY_FLASH
public class MegaCopyObject
{
#if false
	static public void CopyFromTo(GameObject obj, GameObject to)
	{
		Component[] components = obj.GetComponents<Component>();

		for ( int i = 0; i < components.Length; i++ )
		{
			bool en = false;
			Type tp = components[i].GetType();

			if ( tp.IsSubclassOf(typeof(Behaviour)) )
			{
				en = (components[i] as Behaviour).enabled;
			}
			else
			{
				if ( tp.IsSubclassOf(typeof(Component)) && tp.GetProperty("enabled") != null )
					en = (bool)tp.GetProperty("enabled").GetValue(components[i], null);
				else
					en = true;
			}

			FieldInfo[] fields = tp.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);	//claredOnly);
			PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);	//claredOnly);

			Component c = to.GetComponent(tp);

			if ( c == null )
				c = to.AddComponent(tp);

			if ( tp.IsSubclassOf(typeof(Behaviour)) )
			{
				(c as Behaviour).enabled = en;
			}
			else
			{
				if ( tp.IsSubclassOf(typeof(Component)) && tp.GetProperty("enabled") != null )
					tp.GetProperty("enabled").SetValue(c, en, null);
			}

			for ( int j = 0; j < fields.Length; j++ )
			{
				fields[j].SetValue(c, fields[j]);
			}

			for ( int j = 0; j < properties.Length; j++ )
			{
				Debug.Log("prop " + properties[j].Name);

				//if ( properties[j].CanWrite )
				//	properties[j].SetValue(c, properties[j], null);
			}
		}
	}
#endif

	static GameObject CopyMesh(GameObject subject)
	{
		GameObject clone = (GameObject)GameObject.Instantiate(subject);

		MeshFilter[] mfs = subject.GetComponentsInChildren<MeshFilter>();
		MeshFilter[] clonemfs = clone.GetComponentsInChildren<MeshFilter>();

		MeshCollider[] mcs = clone.GetComponentsInChildren<MeshCollider>();
		MeshCollider[] clonemcs = clone.GetComponentsInChildren<MeshCollider>();

		int l = mfs.Length;

		for ( int i = 0; i < l; i++ )
		{
			MeshFilter mf = mfs[i];
			MeshFilter clonemf = clonemfs[i];
			Mesh mesh = mf.sharedMesh;
			Mesh clonemesh = new Mesh();
			clonemesh.vertices = mesh.vertices;
			clonemesh.uv2 = mesh.uv2;
			clonemesh.uv2 = mesh.uv2;
			clonemesh.uv = mesh.uv;
			clonemesh.normals = mesh.normals;
			clonemesh.tangents = mesh.tangents;
			clonemesh.colors = mesh.colors;

			clonemesh.subMeshCount = mesh.subMeshCount;

			for ( int s = 0; s < mesh.subMeshCount; s++ )
			{
				clonemesh.SetTriangles(mesh.GetTriangles(s), s);
			}

			//clonemesh.triangles = mesh.triangles;

			clonemesh.boneWeights = mesh.boneWeights;
			clonemesh.bindposes = mesh.bindposes;
			clonemesh.name = mesh.name + "_copy";
			clonemesh.RecalculateBounds();
			clonemf.sharedMesh = clonemesh;

			for ( int j = 0; j < mcs.Length; j++ )
			{
				MeshCollider mc = mcs[j];
				if ( mc.sharedMesh = mesh )
					clonemcs[j].sharedMesh = clonemesh;
			}
		}

		return clone;
	}

	static GameObject CopyMesh(GameObject subject, MegaModifyObject mod)
	{
		GameObject clone = (GameObject)GameObject.Instantiate(subject);

		MeshFilter[] mfs = subject.GetComponentsInChildren<MeshFilter>();
		MeshFilter[] clonemfs = clone.GetComponentsInChildren<MeshFilter>();

		MeshCollider[] mcs = clone.GetComponentsInChildren<MeshCollider>();
		MeshCollider[] clonemcs = clone.GetComponentsInChildren<MeshCollider>();

		int l = mfs.Length;

		for ( int i = 0; i < l; i++ )
		{
			MeshFilter mf = mfs[i];
			MeshFilter clonemf = clonemfs[i];
			Mesh mesh = mf.sharedMesh;
			Mesh clonemesh = new Mesh();
			clonemesh.vertices = mod.verts;	//mesh.vertices;
			clonemesh.uv2 = mesh.uv2;
			clonemesh.uv2 = mesh.uv2;
			clonemesh.uv = mod.uvs;	//mesh.uv;
			clonemesh.normals = mod.norms;	//mesh.normals;
			clonemesh.tangents = mesh.tangents;
			clonemesh.colors = mesh.colors;

			clonemesh.subMeshCount = mesh.subMeshCount;

			for ( int s = 0; s < mesh.subMeshCount; s++ )
			{
				clonemesh.SetTriangles(mesh.GetTriangles(s), s);
			}

			//clonemesh.triangles = mesh.triangles;

			clonemesh.boneWeights = mesh.boneWeights;
			clonemesh.bindposes = mesh.bindposes;
			clonemesh.name = mesh.name + "_copy";
			clonemesh.RecalculateBounds();
			clonemf.sharedMesh = clonemesh;

			for ( int j = 0; j < mcs.Length; j++ )
			{
				MeshCollider mc = mcs[j];
				if ( mc.sharedMesh = mesh )
					clonemcs[j].sharedMesh = clonemesh;
			}
		}

		return clone;
	}

	static void CopyModObj(MegaModifyObject from, MegaModifyObject to)
	{
		if ( from && to )
		{
			to.Enabled = from.Enabled;
			to.recalcbounds = from.recalcbounds;
			to.recalcCollider = from.recalcCollider;
			to.recalcnorms = from.recalcnorms;
			to.DoLateUpdate = from.DoLateUpdate;
			to.GrabVerts = from.GrabVerts;
			to.DrawGizmos = from.DrawGizmos;
			to.NormalMethod = from.NormalMethod;
		}
	}

	static public GameObject DoCopyObjects(GameObject from)
	{
		MegaModifyObject fromMod = from.GetComponent<MegaModifyObject>();

		GameObject to;

		if ( fromMod )
		{
			to = CopyMesh(from, fromMod);
		}
		else
			to = CopyMesh(from);
		//CopyObject.CopyFromTo(from, to);

		MegaModifier[] desmods = to.GetComponents<MegaModifier>();
		for ( int i = 0; i < desmods.Length; i++ )
		{
			GameObject.DestroyImmediate(desmods[i]);
		}

		MegaModifyObject mo = to.GetComponent<MegaModifyObject>();
		if ( mo )
		{
			GameObject.DestroyImmediate(mo);
			mo = to.AddComponent<MegaModifyObject>();

			CopyModObj(fromMod, mo);
		}

		MegaModifier[] mods = from.GetComponents<MegaModifier>();

		for ( int i = 0; i < mods.Length; i++ )
		{
			CopyComponent(mods[i], to);
		}

		if ( mo )
		{
			//mod.ReStart1(false);

			//for ( int i = 0; i < mods.Length; i++ )
			//	mods[i].SetModMesh(mod.cachedMesh);
			mo.MeshUpdated();
		}
		to.name = from.name + " - Copy";
		return to;
	}


	static public GameObject DoCopyObjectsChildren(GameObject from)
	{
		GameObject parent = DoCopyObjects(from);

		for ( int i = 0; i < from.transform.childCount; i++ )
		{
			GameObject cobj = from.transform.GetChild(i).gameObject;

			GameObject newchild = DoCopyObjectsChildren(cobj);
			newchild.transform.parent = parent.transform;
		}

		return parent;
	}

	static void CopyComponent(Component from, GameObject to)
	{
		bool en = false;
		Type tp = from.GetType();

		if ( tp.IsSubclassOf(typeof(Behaviour)) )
		{
			en = (from as Behaviour).enabled;
		}
		else
		{
			if ( tp.IsSubclassOf(typeof(Component)) && tp.GetProperty("enabled") != null )
				en = (bool)tp.GetProperty("enabled").GetValue(from, null);
			else
				en = true;
		}

		FieldInfo[] fields = tp.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);	//claredOnly);
		PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);	//claredOnly);

		Component c = to.GetComponent(tp);

		if ( c == null )
			c = to.AddComponent(tp);
#if true
		if ( tp.IsSubclassOf(typeof(Behaviour)) )
		{
			(c as Behaviour).enabled = en;
		}
		else
		{
			if ( tp.IsSubclassOf(typeof(Component)) && tp.GetProperty("enabled") != null )
				tp.GetProperty("enabled").SetValue(c, en, null);
		}

		for ( int j = 0; j < fields.Length; j++ )
		{
			fields[j].SetValue(c, fields[j].GetValue(from));
		}

		for ( int j = 0; j < properties.Length; j++ )
		{
			if ( properties[j].CanWrite )
				properties[j].SetValue(c, properties[j].GetValue(from, null), null);
		}
#endif
	}

	static public void CopyFromTo1(GameObject obj, GameObject to)
	{
		Component[] components = obj.GetComponents<Component>();

		for ( int i = 0; i < components.Length; i++ )
		{
			bool en = false;
			Type tp = components[i].GetType();

			if ( tp.IsSubclassOf(typeof(Behaviour)) )
			{
				en = (components[i] as Behaviour).enabled;
			}
			else
			{
				if ( tp.IsSubclassOf(typeof(Component)) && tp.GetProperty("enabled") != null )
					en = (bool)tp.GetProperty("enabled").GetValue(components[i], null);
				else
					en = true;
			}

			FieldInfo[] fields = tp.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);	//claredOnly);
			PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);	//claredOnly);

			Component c = to.GetComponent(tp);

			if ( c == null )
				c = to.AddComponent(tp);

			if ( tp.IsSubclassOf(typeof(Behaviour)) )
			{
				(c as Behaviour).enabled = en;
			}
			else
			{
				if ( tp.IsSubclassOf(typeof(Component)) && tp.GetProperty("enabled") != null )
					tp.GetProperty("enabled").SetValue(c, en, null);
			}

			for ( int j = 0; j < fields.Length; j++ )
			{
				fields[j].SetValue(c, fields[j].GetValue(tp));
			}

			for ( int j = 0; j < properties.Length; j++ )
			{
				//Debug.Log("prop " + properties[j].Name);

				if ( properties[j].CanWrite )
					properties[j].SetValue(c, properties[j].GetValue(tp, null), null);
			}
		}
	}
}
#endif