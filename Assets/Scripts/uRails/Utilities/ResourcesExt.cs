using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ResourcesExt
{
    // TODO re-enable asserter.
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        UnityEngine.Object asset = Resources.Load(path, typeof(T));
        Asserter.NotNull(asset, "ResourcesExt.Load<T>:Asset at path \"" + path + "\" could not be found, or is not of specified type \"" + typeof(T).Name + "\"");
        return (T)asset;
    }
}
