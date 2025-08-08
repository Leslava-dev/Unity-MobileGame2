using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class SetConvexInLevelSegments : MonoBehaviour
{
    [MenuItem("Tools/Fix MeshColliders in LevelSegments")]
    static void FixMeshColliders()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });

        int totalFixed = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null || !prefab.name.ToLower().Contains("lvl")) continue;

            GameObject instance = PrefabUtility.LoadPrefabContents(path);
            bool changed = false;

            MeshCollider[] colliders = instance.GetComponentsInChildren<MeshCollider>(true);
            foreach (MeshCollider col in colliders)
            {
                if (!col.convex)
                {
                    col.convex = true;
                    changed = true;
                    Debug.Log($"✅ Convex enabled: {col.name} у {prefab.name}", col.gameObject);
                }
            }

            if (changed)
            {
                PrefabUtility.SaveAsPrefabAsset(instance, path);
                totalFixed++;
            }

            PrefabUtility.UnloadPrefabContents(instance);
        }

        Debug.Log($"🔧 Done: {totalFixed} prefabs renew.");
    }
}