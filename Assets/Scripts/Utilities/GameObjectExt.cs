using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectExt
{
    public static Transform FindChildInHierarchy(this Transform component, string GameObjectName)
    {
        Transform foundChild = component.FindChild(GameObjectName);
        if (foundChild)
        {
            return foundChild;
        }
        else
        {
            foreach (Transform child in component)
            {
                foundChild = child.FindChildInHierarchy(GameObjectName);
                if (foundChild)
                {
                    return foundChild;
                }
            }
        }

        return null;
    }

    public static T FindGameObjectWithComponent<T>(string GameObjectName) where T : Component
    {
        GameObject gameObject = GameObject.Find(GameObjectName);
        Asserter.NotNull(gameObject, "GameObjectExt.FindGameObjectWithComponent:Could not find gameObject by name \"" + GameObjectName + "\"");
        T component = gameObject.GetComponent<T>();
        Asserter.NotNull(component, "GameObjectExt.FindGameObjectWithComponent:Could not find component of type \"" + typeof(T).Name + "\" on GameObject \"" + GameObjectName + "\"");

        return component;
    }
}

