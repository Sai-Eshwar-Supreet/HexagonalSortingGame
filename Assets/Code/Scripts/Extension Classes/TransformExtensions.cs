using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Clear the child gameobjects of the transform
    /// </summary>
    /// <param name="transform">The context transform</param>
    public static void Clear(this Transform transform)
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
}
