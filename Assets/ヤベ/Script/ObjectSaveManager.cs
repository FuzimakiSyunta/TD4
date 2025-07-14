#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static ObjectsData;
using static ObjectsSaveData;
using UnityEditor.Build.Content;

public static class ObjectSaveManagerBridge
{
    public static ObjectSaveManager instance;
}

[ExecuteInEditMode]
public class ObjectSaveManager : MonoBehaviour
{
    public GameObject prefab;
    private string path;
    private Dictionary<GameObject, Vector3> lastPositions = new();
    private Dictionary<GameObject, Vector3> lastRotations = new();

    private bool hasPendingChanges = false;
    private double changeTimer = 0f;
    private const double changeDelay = 0.5;
    private double lastTime = 0;

    private List<string> previousIds = new();





    void OnEnable()
    {
        path = Application.dataPath + "/SavedData/objectSaveData.json";
        ObjectSaveManagerBridge.instance = this;

#if UNITY_EDITOR
        EditorApplication.update += OnEditorUpdate;
        SceneView.duringSceneGui += OnSceneGUI;
        lastTime = EditorApplication.timeSinceStartup;

        //  初期IDリストを記録（これが重要）
        previousIds = GameObject.FindGameObjectsWithTag("Savable")
            .Select(o => o.GetComponent<ObjectIdentifier>())
            .Where(id => id != null)
            .Select(id => id.id)
            .ToList();
#endif


    }

    void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= OnEditorUpdate;
        SceneView.duringSceneGui -= OnSceneGUI;
#endif
    }

    public static void RefreshSaveFromEditor()
    {
        ObjectSaveManagerBridge.instance?.SaveData();
    }

    void Update()
    {
        if (Application.isPlaying && Input.GetKeyDown(KeyCode.P))
        {
            SaveData();
            Debug.Log("手動セーブしました！");
        }
    }

#if UNITY_EDITOR
    void OnSceneGUI(SceneView view)
    {
        OnEditorUpdate();
    }

    //void OnEditorUpdate()
    //{
    //    if (Application.isPlaying) return;

    //    GameObject[] savables = GameObject.FindGameObjectsWithTag("Savable");
    //    bool anyChangeDetected = false;

    //    foreach (GameObject obj in savables)
    //    {
    //        if (!lastPositions.ContainsKey(obj) || Vector3.Distance(obj.transform.position, lastPositions[obj]) > 0.01f ||
    //            !lastRotations.ContainsKey(obj) || Vector3.Distance(obj.transform.rotation.eulerAngles, lastRotations[obj]) > 0.01f)
    //        {
    //            anyChangeDetected = true;
    //            lastPositions[obj] = obj.transform.position;
    //            lastRotations[obj] = obj.transform.rotation.eulerAngles;
    //        }
    //    }

    //    if (anyChangeDetected && !hasPendingChanges)
    //    {
    //        hasPendingChanges = true;
    //        changeTimer = 0f;
    //        Debug.Log("Sceneビューでの変更を検知しました");
    //    }

    //    double now = EditorApplication.timeSinceStartup;
    //    double delta = now - lastTime;
    //    lastTime = now;

    //    if (hasPendingChanges)
    //    {
    //        changeTimer += delta;
    //        if (changeTimer >= changeDelay)
    //        {
    //            SaveData();
    //            Debug.Log("Sceneビューの変更を検知し、遅延保存しました！");
    //            hasPendingChanges = false;
    //            changeTimer = 0f;
    //        }
    //    }
    //}
    void OnEditorUpdate()
    {
        if (Application.isPlaying) return;

        GameObject[] current = GameObject.FindGameObjectsWithTag("Savable");
        List<string> currentIds = current
            .Select(o => o.GetComponent<ObjectIdentifier>())
            .Where(id => id != null)
            .Select(id => id.id)
            .ToList();

        var deletedIds = previousIds.Where(prevId => !currentIds.Contains(prevId)).ToList();
        if (deletedIds.Count > 0)
        {
            ObjectSaveData saveData = File.Exists(path)
                ? JsonUtility.FromJson<ObjectSaveData>(File.ReadAllText(path))
                : new ObjectSaveData();

            foreach (var removedId in deletedIds)
            {
                saveData.objects.RemoveAll(o => o.id == removedId);
                Debug.Log($"Sceneビューで削除されたオブジェクト: {removedId}");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
            Debug.Log("保存データから削除反映しました");
        }

        previousIds = currentIds;



        //  位置・回転変更の検知
        bool anyChangeDetected = false;
        foreach (GameObject obj in current)
        {
            if (!lastPositions.ContainsKey(obj) || Vector3.Distance(obj.transform.position, lastPositions[obj]) > 0.01f ||
                !lastRotations.ContainsKey(obj) || Vector3.Distance(obj.transform.rotation.eulerAngles, lastRotations[obj]) > 0.01f)
            {
                anyChangeDetected = true;
                lastPositions[obj] = obj.transform.position;
                lastRotations[obj] = obj.transform.rotation.eulerAngles;
            }
        }

        if (anyChangeDetected && !hasPendingChanges)
        {
            hasPendingChanges = true;
            changeTimer = 0f;
            Debug.Log("Sceneビューでの変更を検知しました");
        }

        double now = EditorApplication.timeSinceStartup;
        double delta = now - lastTime;
        lastTime = now;

        if (hasPendingChanges)
        {
            changeTimer += delta;
            if (changeTimer >= changeDelay)
            {
                SaveData(); // 位置・回転の更新保存
                Debug.Log("Sceneビューの変更を検知し、遅延保存しました！");
                hasPendingChanges = false;
                changeTimer = 0f;
            }
        }


    }


#endif

    void SaveData()
    {
        // 既存の保存データをロード（なければ新規）
        ObjectSaveData saveData;
        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<ObjectSaveData>(existingJson);
        }
        else
        {
            saveData = new ObjectSaveData();
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Savable"))
        {
            if (Application.isPlaying && obj.GetComponent<AlreadyLoadedFlag>() != null)
                continue;

            // プレハブ名の取得とマッピング
            string prefabName = obj.name.Replace("(Clone)", "").Trim();

            if (prefabName.Contains("SmallJumpingPlatform"))
                prefabName = "小ジャンプ台";
            else if (prefabName.Contains("BigJumpingPlatform"))
                prefabName = "大ジャンプ台";

            // ObjectIdentifier を確認
            var identifier = obj.GetComponent<ObjectIdentifier>();
            if (identifier == null)
            {
                Debug.LogWarning($"IDが見つかりません（保存対象外）: {obj.name}");
                continue;
            }

            // 上書き or 新規追加
            var existing = saveData.objects.FirstOrDefault(o => o.id == identifier.id);
            if (existing != null)
            {
                existing.position = obj.transform.position;
                existing.rotation = obj.transform.rotation;
                existing.prefabName = prefabName;
                Debug.Log($"上書き保存: {prefabName} @ {existing.position}");
            }
            else
            {
                ObjectData data = new ObjectData
                {
                    id = identifier.id,
                    prefabName = prefabName,
                    position = obj.transform.position,
                    rotation = obj.transform.rotation
                };
                saveData.objects.Add(data);
                Debug.Log($"新規保存: {prefabName} @ {data.position}");
            }
        }

        Debug.Log($"保存対象数: {saveData.objects.Count}");
        foreach (var obj in saveData.objects)
        {
            Debug.Log($"保存: {obj.prefabName}, Pos: {obj.position}, Rot: {obj.rotation.eulerAngles}");
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
        Debug.Log("保存されました（上書き対応済）: " + json);


    }
}



