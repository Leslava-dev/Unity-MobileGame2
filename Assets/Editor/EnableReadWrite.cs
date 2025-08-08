using UnityEditor;
using UnityEngine;

public class EnableReadWrite
{
    [MenuItem("Tools/Enable Read/Write on all FBX")]
    public static void EnableReadWriteOnAllFBX()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                Debug.Log("Enabled Read/Write for: " + path);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log("---> Read/Write Enabled for all models. <---");
    }
}
