using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreateContainerEditor 
{

    [MenuItem("Assets/Create/Settings/Settings Manager", validate = false)]
    public static void CreateSettingsContainer()
    {
		var currentPath = GetSelectedPathOrFallback();

		var manager = new GameObject(SettingsManager.ManagerName).AddComponent<SettingsManager>().gameObject;

		PrefabUtility.SaveAsPrefabAssetAndConnect(manager, $"{currentPath}/{SettingsManager.ManagerName}.prefab", InteractionMode.UserAction);

        Object.DestroyImmediate(manager);
    }

	// gist.github.com/allanolivei/9260107
	public static string GetSelectedPathOrFallback()
	{
		string path = "Assets";

		foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
		{
			path = AssetDatabase.GetAssetPath(obj);
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
				break;
			}
		}
		return path;
	}

}