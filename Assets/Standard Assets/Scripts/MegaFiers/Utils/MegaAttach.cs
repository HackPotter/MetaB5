
using UnityEngine;

[ExecuteInEditMode]
public class MegaAttach : MonoBehaviour
{
	public MegaModifiers	target;

	[HideInInspector]
	public Vector3	BaryCoord = Vector3.zero;
	[HideInInspector]
	public int[] BaryVerts = new int[3];
	[HideInInspector]
	public bool attached = false;

	[HideInInspector]
	public Vector3 BaryCoord1 = Vector3.zero;
	[HideInInspector]
	public int[] BaryVerts1 = new int[3];

	public Vector3 attachforward = Vector3.forward;

	public Vector3 AxisRot = Vector3.zero;
	public float radius = 0.1f;
	Vector3 pt = Vector3.zero;

	public void DetachIt()
	{
		attached = false;
	}

	public void AttachIt()
	{
		if ( target )
		{
			attached = true;

			Mesh mesh = target.mesh;
			Vector3 objSpacePt = target.transform.InverseTransformPoint(pt);
			Vector3[] verts = target.sverts;	//mesh.vertices;
			int[] tris = mesh.triangles;
			int index = -1;
			MegaNearestPointTest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref BaryCoord);

			if ( index >= 0 )
			{
				BaryVerts[0] = tris[index];
				BaryVerts[1] = tris[index + 1];
				BaryVerts[2] = tris[index + 2];
			}

			MegaNearestPointTest.NearestPointOnMesh1(objSpacePt + attachforward, verts, tris, ref index, ref BaryCoord1);

			if ( index >= 0 )
			{
				BaryVerts1[0] = tris[index];
				BaryVerts1[1] = tris[index + 1];
				BaryVerts1[2] = tris[index + 2];
			}
		}
	}

	void OnDrawGizmos()
	{
		pt = transform.position;

		Gizmos.color = Color.white;
		Gizmos.DrawSphere(pt, radius);

		if ( target )
		{
			if ( attached )
			{
				Vector3 pos = GetCoordMine(target.sverts[BaryVerts[0]], target.sverts[BaryVerts[1]], target.sverts[BaryVerts[2]], BaryCoord);

				Vector3 worldPt = target.transform.TransformPoint(pos);
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(worldPt, radius);
				Vector3 nw = target.transform.TransformDirection(norm * 40.0f);
				Gizmos.DrawLine(worldPt, worldPt + nw);
			}
			else
			{
				Mesh mesh = target.mesh;
				Vector3 objSpacePt = target.transform.InverseTransformPoint(pt);
				Vector3[] verts = target.sverts;	//mesh.vertices;
				int[] tris = mesh.triangles;
				int index = -1;
				Vector3 tribary = Vector3.zero;
				Vector3 meshPt = MegaNearestPointTest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref tribary);
				Vector3 worldPt = target.transform.TransformPoint(meshPt);

				if ( index >= 0 )
				{
					Vector3 cp2 = GetCoordMine(verts[tris[index]], verts[tris[index + 1]], verts[tris[index + 2]], tribary);
					worldPt = target.transform.TransformPoint(cp2);	//meshPt);
					// Calc the normal
				}

				Gizmos.color = Color.red;
				Gizmos.DrawSphere(worldPt, radius);	//.01f);

				//transform.position = worldPt;
				Gizmos.color = Color.blue;
				meshPt = MegaNearestPointTest.NearestPointOnMesh1(objSpacePt + attachforward, verts, tris, ref index, ref tribary);
				Vector3 worldPt1 = target.transform.TransformPoint(meshPt);

				Gizmos.DrawSphere(worldPt1, radius);	//.01f);

				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(worldPt, worldPt1);
			}
		}
	}

	Vector3 norm = Vector3.zero;
	public Vector3 up = Vector3.up;

	void LateUpdate()
	{
		if ( attached )
		{
			Vector3 v0 = target.sverts[BaryVerts[0]];
			Vector3 v1 = target.sverts[BaryVerts[1]];
			Vector3 v2 = target.sverts[BaryVerts[2]];

			Vector3 pos = GetCoordMine(v0, v1, v2, BaryCoord);

			transform.localPosition = pos;

			// Rotation
			Vector3 va = v1 - v0;
			Vector3 vb = v2 - v1;

			norm = Vector3.Cross(va, vb);

			v0 = target.sverts[BaryVerts1[0]];
			v1 = target.sverts[BaryVerts1[1]];
			v2 = target.sverts[BaryVerts1[2]];

			Vector3 fwd = GetCoordMine(v0, v1, v2, BaryCoord1) - pos;

			Quaternion erot = Quaternion.Euler(AxisRot);
			Quaternion rot = Quaternion.LookRotation(fwd, norm) * erot;	//, up);	//Vector3.forward);
			transform.localRotation = rot;
		}
	}

#if false
	float Determinant(Matrix4x4 m)
	{
		Vector3 r0 = m.GetRow(0);
		Vector3 r1 = m.GetRow(1);
		Vector3 r2 = m.GetRow(2);

		return -(r0.z * r1.y * r2.x) + (r0.y * r1.z * r2.x) + (r0.z * r1.x * r2.y) - (r0.x * r1.z * r2.y) - (r0.y * r1.x * r2.z) + (r0.x * r1.y * r2.z);
	}

	Vector3 barycentricCoords(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p)
	{
		Vector3 bc = Vector3.zero;
		Vector3 q  = p - p3;
		Vector4 q0 = p0 - p3;
		Vector4 q1 = p1 - p3;
		Vector4 q2 = p2 - p3;

		Matrix4x4 m = Matrix4x4.identity;
		m.SetColumn(0, q0);
		m.SetColumn(1, q1);
		m.SetColumn(2, q2);

		float det = Determinant(m);

		m.SetColumn(0, q);
		bc.x = Determinant(m);

		m.SetColumn(0, q0);
		m.SetColumn(1, q);
		bc.y = Determinant(m);

		m.SetColumn(1, q1);
		m.SetColumn(2, q);
		bc.z = Determinant(m);

		if ( det != 0.0f )
			bc /= det;

		return bc;
	}

	Vector2 baryCoord(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
	{
		// Compute vectors        
		Vector3 v0 = C - A;
		Vector3 v1 = B - A;
		Vector3 v2 = P - A;

		// Compute dot products
		float dot00 = Vector3.Dot(v0, v0);
		float dot01 = Vector3.Dot(v0, v1);
		float dot02 = Vector3.Dot(v0, v2);
		float dot11 = Vector3.Dot(v1, v1);
		float dot12 = Vector3.Dot(v1, v2);

		// Compute barycentric coordinates
		float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
		Vector2 coord = Vector2.zero;
		coord.x = (dot11 * dot02 - dot01 * dot12) * invDenom;
		coord.y = (dot00 * dot12 - dot01 * dot02) * invDenom;

		// Check if point is in triangle
		return coord;	//(u >= 0) && (v >= 0) && (u + v < 1)
	}

	Vector3 GetCoordA(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
	{
		Vector3 vector;
		vector.x = (value1.x + (amount1 * (value2.x - value1.x))) + (amount2 * (value3.x - value1.x));
		vector.y = (value1.y + (amount1 * (value2.y - value1.y))) + (amount2 * (value3.y - value1.y));
		vector.z = (value1.z + (amount1 * (value2.z - value1.z))) + (amount2 * (value3.z - value1.z));
		return vector;
	}

	Vector3 GetCoord(Vector3 A, Vector3 B, Vector3 C, Vector2 bary)
	{
		Vector3 p = Vector3.zero;
		p.x = ((1.0f - bary.x - bary.y) * A.x) + (bary.x * B.x) + (bary.y * C.x);
		p.y = ((1.0f - bary.x - bary.y) * A.y) + (bary.x * B.y) + (bary.y * C.y);
		p.z = ((1.0f - bary.x - bary.y) * A.z) + (bary.x * B.z) + (bary.y * C.z);

		return p;
	}
#endif

	Vector3 GetCoordMine(Vector3 A, Vector3 B, Vector3 C, Vector3 bary)
	{
		Vector3 p = Vector3.zero;
		p.x = (bary.x * A.x) + (bary.y * B.x) + (bary.z * C.x);
		p.y = (bary.x * A.y) + (bary.y * B.y) + (bary.z * C.y);
		p.z = (bary.x * A.z) + (bary.y * B.z) + (bary.z * C.z);

		return p;
	}

#if false
	Vector3 GetCoord1(Vector3 A, Vector3 B, Vector3 C, Vector2 bary)
	{
		Vector3 p = Vector3.zero;
		p.x = ((1.0f - bary.x - bary.y) * B.x) + (bary.x * C.x) + (bary.y * A.x);
		p.y = ((1.0f - bary.x - bary.y) * B.y) + (bary.x * C.y) + (bary.y * A.y);
		p.z = ((1.0f - bary.x - bary.y) * B.z) + (bary.x * C.z) + (bary.y * A.z);

		return p;
	}

	Vector3 GetCoord2(Vector3 A, Vector3 B, Vector3 C, Vector2 bary)
	{
		Vector3 p = Vector3.zero;
		p.x = ((1.0f - bary.x - bary.y) * C.x) + (bary.x * B.x) + (bary.y * A.x);
		p.y = ((1.0f - bary.x - bary.y) * C.y) + (bary.x * B.y) + (bary.y * A.y);
		p.z = ((1.0f - bary.x - bary.y) * C.z) + (bary.x * B.z) + (bary.y * A.z);

		return p;
	}

	Vector3 GetCoord3(Vector3 A, Vector3 B, Vector3 C, Vector2 bary)
	{
		Vector3 p = Vector3.zero;
		p.x = ((1.0f - bary.x - bary.y) * A.x) + (bary.x * C.x) + (bary.y * B.x);
		p.y = ((1.0f - bary.x - bary.y) * A.y) + (bary.x * C.y) + (bary.y * B.y);
		p.z = ((1.0f - bary.x - bary.y) * A.z) + (bary.x * C.z) + (bary.y * B.z);

		return p;
	}
#endif
}