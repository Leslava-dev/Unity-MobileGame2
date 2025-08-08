namespace Dreamteck
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class SceneUtility
    {
        public static List<Transform> childrenList = new List<Transform>();

        public static void GetChildrenRecursively(Transform current)
        {
            childrenList.Clear();
            GetChildrenRecursivelyInternal(current);
        }

        private static void GetChildrenRecursivelyInternal(Transform current)
        {
            childrenList.Add(current);
            int childCount = current.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GetChildrenRecursivelyInternal(current.GetChild(i));
            }
        }

        public static T[] GetComponentsInChildrenRecusrively<T>(this GameObject gameObject) where T : Component
        {
            GetChildrenRecursively(gameObject.transform);
            List<T> components = new List<T>();
            for (int i = 0; i < childrenList.Count; i++)
            {
                T component = childrenList[i].GetComponent<T>();
                if(component != null)
                {
                    components.Add(component);
                }
            }
            return components.ToArray();
        }

        public static void GetChildrenRecursively(Transform current, ref List<Transform> transformList)
        {
            transformList.Add(current);
            foreach (Transform child in current) GetChildrenRecursively(child, ref transformList);
        }

        public static T GetComponentInScene<T>(this Scene scene, string objectName = null) where T : Component
        {
            var component = default(T);
            var rootObjects = scene.GetRootGameObjects();

            foreach (var obj in rootObjects)
            {
                if (objectName != null && obj.name != objectName) continue;
                component = obj.GetComponentInChildren<T>();
                if(component != null)
                {
                    break;
                }
            }

            return component;
        }

        public static T[] GetComponentsInScene<T>(this Scene scene, string objectName = null, bool includeInactive = false)
        {
            var rootObjects = scene.GetRootGameObjects();
            var components = new List<T>();

            foreach (var obj in rootObjects)
            {
                var rootComponent = obj.GetComponent<T>();

                if (rootComponent != null && (objectName == null || (objectName != null && obj.gameObject.name == obj.name)))
                {
                    components.Add(rootComponent);
                }

                var foundComponents = obj.GetComponentsInChildren<T>(includeInactive);

                for (int i = 0; i < foundComponents.Length; i++)
                {
                    var component = foundComponents[i] as Component;

                    if (objectName != null && component != null && component.gameObject.name != objectName) continue;

                    components.Add(foundComponents[i]);
                }
            }

            return components.ToArray();
        }
    }
}
