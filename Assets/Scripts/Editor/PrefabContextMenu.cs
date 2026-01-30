using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabContextMenu
{

    [MenuItem("GameObject/Prefab/Create Poolable data object")]
    static void CreatePoolableFromPrefab()
    {
        if (Selection.activeGameObject == null || PrefabUtility.GetPrefabInstanceStatus(Selection.activeGameObject) != PrefabInstanceStatus.Connected)
        {
            Debug.LogError("Selected object is not valid for creating poolable?");
            return;
        }

        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(Selection.activeGameObject);

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("No path for prefab?");
            return;
        }

        Object prefabObj = AssetDatabase.LoadAssetAtPath<Object>(path);

        PooledPrefabData asset = ScriptableObject.CreateInstance<PooledPrefabData>();

        asset.prefab = prefabObj as GameObject;

        AssetDatabase.CreateAsset(asset, "Assets/Data/Resources/PooledPrefabs/" + Selection.activeGameObject.name + "_Poolable.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Created poolable asset!");
    }

    [MenuItem("Prefab/Create Poolable data object", true)]
    static bool CheckThatTargetIsPrefab()
    {
        return Selection.activeGameObject != null && PrefabUtility.GetPrefabInstanceStatus(Selection.activeGameObject) == PrefabInstanceStatus.Connected;
    }

}