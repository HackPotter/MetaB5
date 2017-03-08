using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Waypoint
{
    [Infobox("A transform at which the way point will be placed.")]
    public Transform WaypointPosition;
    [Infobox("The icon of the waypoint to be displayed in the world.")]
    public Texture2D OptionalWaypointIcon;
    [Infobox("The size of the icon.")]
    public Vector2 IconSize;
    [Infobox("If true, the waypoint icon will be mirrored vertically.")]
    public bool MirrorIconVertically;
}

public class WaypointView : MonoBehaviour
{
    private Dictionary<string, Waypoint> _activeWaypoints = new Dictionary<string, Waypoint>();

    public void AddWaypoint(string identifier, Waypoint waypoint)
    {
        if (_activeWaypoints.ContainsKey(identifier))
        {
            DebugFormatter.LogError(this, "Attempting to add waypoint with identifier {0}, but a waypoint with that identifier already exists.", identifier);
            return;
        }
        _activeWaypoints.Add(identifier, waypoint);
        if (_activeWaypoints.Count != 0)
        {
            this.enabled = true;
        }
    }

    public void RemoveWaypoint(string identifier)
    {
        if (_activeWaypoints.ContainsKey(identifier))
        {
            _activeWaypoints.Remove(identifier);
        }
        if (_activeWaypoints.Count == 0)
        {
            this.enabled = false;
        }
    }

    public void ClearWaypoints()
    {
        _activeWaypoints.Clear();
    }

    public Texture2D DefaultWaypointIcon
    {
        get;
        private set;
    }

    public Vector2 DefaultIconSize
    {
        get;
        private set;
    }
    
    void OnGUI()
    {
        if (_activeWaypoints.Count == 0)
        {
            this.enabled = false;
            return;
        }
		
		if (Camera.main == null)
		{
			return;
		}

        List<string> toRemove = new List<string>();
        foreach (KeyValuePair<string, Waypoint> waypoint in _activeWaypoints)
        {
            if (!waypoint.Value.WaypointPosition)
            {
                toRemove.Add(waypoint.Key);
            }
        }

        foreach (string s in toRemove)
        {
            _activeWaypoints.Remove(s);
        }

        foreach (Waypoint wp in _activeWaypoints.Values)
        {
            Vector3 markerPoint = wp.WaypointPosition.position;
            Texture2D waypointIcon = wp.OptionalWaypointIcon == null ? DefaultWaypointIcon : wp.OptionalWaypointIcon;
            Vector2 iconSize = wp.OptionalWaypointIcon != null ? wp.IconSize : DefaultIconSize;

            float waypointHeight = iconSize.y;
            float waypointWidth = iconSize.x;
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(markerPoint);

            if (viewportPoint.z < 0)
            {
                continue;
            }

            float waypointX = Screen.width * viewportPoint.x - (waypointWidth / 2);
            float waypointY = Screen.height - (Screen.height * viewportPoint.y) - waypointHeight / 2;

            Rect waypointRect;
            if (wp.MirrorIconVertically)
            {
                waypointRect = new Rect(waypointX, waypointY + waypointHeight, waypointWidth, -waypointHeight);
            }
            else
            {
                waypointRect = new Rect(waypointX, waypointY, waypointWidth, waypointHeight);
            }

            //string distance = Vector3.Distance(markerGO.transform.position, Ship.transform.position).ToString("n2") + " microns";
            //Vector2 nameSize = waypointStyle.CalcSize(new GUIContent(marker.GetName()));
            //Vector2 distSize = waypointStyle.CalcSize(new GUIContent(distance));
            //Rect nameRect = new Rect(waypointX - nameSize.y / 4.0f, waypointY - nameSize.x, nameSize.y, nameSize.x);
            //Rect distRect = new Rect(waypointX - distSize.y / 4.0f, waypointY + waypointHeight, distSize.y, distSize.x);

            GUI.DrawTexture(waypointRect, waypointIcon);
        }
    }
	
}

