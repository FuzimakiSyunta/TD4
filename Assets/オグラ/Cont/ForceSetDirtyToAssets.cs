#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

/// <summary>
/// 選択中のアセットに対してDirtyフラグを強制的に設定するスクリプト
/// </summary>
public class ForceSetDirtyToAssets
{
    [MenuItem("Assets/Set Assets as Dirty", false)]
    public static void SetSelectedAssetsDirty()
    {
        var selectedAssetGUIDs = Selection.assetGUIDs;
        if (selectedAssetGUIDs.Length == 0)
        {
            return;
        }

        var folderPaths = new List<string>();
        foreach (var guid in selectedAssetGUIDs)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                folderPaths.Add(assetPath);
            }
            else
            {
                SetAssetDirtyByPath(assetPath);
            }
        }

        SetAssetsInFoldersDirty(folderPaths);
        AssetDatabase.SaveAssets();
    }

    private static void SetAssetDirtyByPath(string assetPath)
    {
        var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
        if (asset != null)
        {
            EditorUtility.SetDirty(asset);
        }
    }

    private static void SetAssetsInFoldersDirty(IEnumerable<string> folderPaths)
    {
        var allAssetGUIDsInFolders = AssetDatabase.FindAssets("", folderPaths.ToArray());
        foreach (var guid in allAssetGUIDsInFolders)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            SetAssetDirtyByPath(assetPath);
        }
    }
}
#endif