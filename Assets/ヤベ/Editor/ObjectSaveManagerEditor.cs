using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ObjectSaveManager))]

public class ObjectSaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("è“®•Û‘¶‚ğÀs"))
        {
            ObjectSaveManager.RefreshSaveFromEditor();
        }
    }


}
#endif