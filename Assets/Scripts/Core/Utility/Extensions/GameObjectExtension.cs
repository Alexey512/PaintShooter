using System.Collections.Generic;
using UnityEngine;

namespace Utility.Extensions
{
    public static class GameObjectExtension
    {
        public static Bounds GetBounds(this GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
            foreach (Renderer renderer in renderers)
                bounds.Encapsulate(renderer.bounds);

            return bounds;
        }

        public static List<T> GetAllComponentsInChildren<T>(this Component owner) where T: Component
        {
            List<T> result = new List<T>(owner.GetComponentsInChildren<T>());
            foreach (Transform child in owner.transform)
            {
                result.AddRange(GetAllComponentsInChildren<T>(child));
            }
            return result;
        }
    }
}