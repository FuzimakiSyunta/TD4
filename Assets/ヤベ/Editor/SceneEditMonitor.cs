using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public class SceneEditMonitor
{
    static Vector3 lastPos = Vector3.zero;
    static Vector3 lastRot = Vector3.zero;

    static Dictionary<GameObject, Vector3> lastPositions = new();
    static Dictionary<GameObject, Vector3> lastRotations = new();


    static SceneEditMonitor()
    {
        EditorApplication.update += CheckForChanges;
    }

    static void CheckForChanges()
    {
        if (Application.isPlaying) return;

        GameObject[] savables = GameObject.FindGameObjectsWithTag("Savable");
        bool changeDetected = false;

        foreach (var obj in savables)
        {
            if (obj == null) continue;

            Vector3 currentPos = obj.transform.position;
            Vector3 currentRot = obj.transform.rotation.eulerAngles;

            bool posChanged = !lastPositions.ContainsKey(obj) || Vector3.Distance(currentPos, lastPositions[obj]) > 0.01f;
            bool rotChanged = !lastRotations.ContainsKey(obj) || Vector3.Distance(currentRot, lastRotations[obj]) > 0.01f;

            if (posChanged || rotChanged)
            {
                lastPositions[obj] = currentPos;
                lastRotations[obj] = currentRot;
                changeDetected = true;
            }
        }

        if (changeDetected)
        {
            Debug.Log("[SceneEditMonitor] ïœçXÇ†ÇË Å® ï€ë∂");
            ObjectSaveManager.RefreshSaveFromEditor();
        }




    }
}
#endif



//public class SceneEditMonitor : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
