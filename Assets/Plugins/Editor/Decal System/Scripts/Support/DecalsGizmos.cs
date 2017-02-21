//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://www.edelweissinteractive.com)
//

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Edelweiss.DecalSystem {
	public class DecalsGizmos {
		
		[DrawGizmo (GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
		static void RenderProjectorGizmo (DS_DecalProjector a_Projector, GizmoType a_GizmoType) {
			Gizmos.DrawIcon (a_Projector.transform.position, "DecalProjectorGizmo.png", true);
		}
		
		[DrawGizmo (GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
		static void RenderProjectorGizmo (DS_SkinnedDecalProjector a_Projector, GizmoType a_GizmoType) {
			Gizmos.DrawIcon (a_Projector.transform.position, "DecalProjectorGizmo.png", true);
		}
	}
}
