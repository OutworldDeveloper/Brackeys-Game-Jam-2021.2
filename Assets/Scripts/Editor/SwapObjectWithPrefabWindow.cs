using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SwapObjectWithPrefabWindow : EditorWindow
{

    [MenuItem("Tools/Swap Objects With Prefab")]
    public static void OpenWindow()
    {
        GetWindow<SwapObjectWithPrefabWindow>();
    }

    private GameObject _prefabToSwapWith;

    private void OnGUI()
    {
        if (Selection.gameObjects.Length < 1)
        {
            EditorGUILayout.HelpBox("Select objects that need to be swapped", MessageType.Warning);
        }
        else
        {
            _prefabToSwapWith = (GameObject)EditorGUILayout.ObjectField("Prefab to swap with", _prefabToSwapWith, typeof(GameObject), false);

            if (_prefabToSwapWith == null)
            {
                EditorGUILayout.HelpBox("Select a prefab to swap with", MessageType.Warning);
            }
            else 
            {
                if (GUILayout.Button("Swap"))
                {
                    Swap();
                }
                EditorGUILayout.HelpBox($"Pressing this will swap {Selection.transforms.Length} objects with the prefab", MessageType.Info);
                EditorGUILayout.HelpBox("Changes will be recorded, so you will be able to undo them", MessageType.Info);
            }
        }
    }

    private void Swap()
    {
        foreach (var transform in Selection.transforms)
        {
            var prefab = PrefabUtility.InstantiatePrefab(_prefabToSwapWith) as GameObject;
            Undo.RegisterCreatedObjectUndo(prefab, $"{nameof(SwapObjectWithPrefabWindow)}-instantiate-prefab");
            prefab.transform.position = transform.position;
            prefab.transform.rotation = transform.rotation;
            prefab.transform.localScale = transform.localScale;
            prefab.transform.parent = transform.parent;
            Undo.DestroyObjectImmediate(transform.gameObject);
        }
    }

}