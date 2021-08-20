using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class MouseFix
{

    static MouseFix()
    {
        EditorApplication.playModeStateChanged += (mode) =>
        {
            if (mode == PlayModeStateChange.EnteredEditMode)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        };
    }

}