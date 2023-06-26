using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ExtensionMethods
    {
        public static void GetCreateParent(this Transform trans,string parentPath)
        {
            var paths = parentPath.Split('/');
            Transform lastTransform = null;
            foreach(var path in paths)
            {
                Debug.Log(path);
                var lastGameObject = MonoBehaviour.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
                lastGameObject.name = path;
                lastGameObject.transform.SetParent(lastTransform);
                lastTransform = lastGameObject.transform;
            }
            trans.SetParent(lastTransform);
        }
    }
}

