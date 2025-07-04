using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using static ObjectsData;
using static ObjectsSaveData;
using System;
using System.Linq;

public static class ObjectSaveManagerBridge
{
    public static ObjectSaveManager instance;
}

[ExecuteInEditMode]  //エディターでも動作させる
public class ObjectSaveManager : MonoBehaviour
{
    public GameObject prefab;
    private string path;
    private Vector3 lastPosition;
    private Vector3 lastRotation;

    private Dictionary<GameObject, Vector3> lastPositions = new();
    private Dictionary<GameObject, Vector3> lastRotations = new();



    void OnEnable()
    {
        if (!Application.isPlaying)
        {
            ObjectSaveManagerBridge.instance = this;
        }
    }

    public static void RefreshSaveFromEditor()
    {
        if (ObjectSaveManagerBridge.instance != null)
        {
            ObjectSaveManagerBridge.instance.SaveData();
        }
    }



    private void Awake()  //Awake()の方がシーンロード時に確実に動作する
    {
        path = Application.dataPath + "/SavedData/objectSaveData.json";
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Savableオブジェクトの数: " + GameObject.FindGameObjectsWithTag("Savable").Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            GameObject[] savables = GameObject.FindGameObjectsWithTag("Savable");

            foreach (GameObject obj in savables)
            {
                if(!lastPositions.ContainsKey(obj) || Vector3.Distance(obj.transform.position, lastPositions[obj]) > 0.01f ||
                  !lastRotations.ContainsKey(obj) || Vector3.Distance(obj.transform.rotation.eulerAngles, lastRotations[obj]) > 0.01f)
{
                    SaveData();
                    lastPositions[obj] = obj.transform.position;
                    lastRotations[obj] = obj.transform.rotation.eulerAngles;
                }


            }
        }

        if (Application.isPlaying && Input.GetKeyDown(KeyCode.P))
        {
            SaveData(); // プレイ中に手動セーブ
            Debug.Log("手動セーブしました！");
        }



    }

    void SaveData()
    {
        ObjectSaveData saveData = new ObjectSaveData();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Savable"))
        {
            if (Application.isPlaying)
            {
                // プレイ中：ロードされたものを除外
                if (obj.GetComponent<AlreadyLoadedFlag>() != null)
                    continue;
            }
            else
            {
                // エディタ時：フラグが無い＝新規配置ではないので除外
                if (obj.GetComponent<AlreadyLoadedFlag>() == null)
                    continue;
            }



            // ObjectData 情報構築（名前やID付き）
            ObjectData data = new ObjectData
            {
                id = Guid.NewGuid().ToString(),
                prefabName = obj.name.Replace("(Clone)", "").Trim(), // プレハブ名を推定（任意）
                position = obj.transform.position,
                rotation = obj.transform.rotation
            };

            // 既に同一の位置＆回転のデータがあるかチェック（重複保存防止）
            if (!saveData.objects.Any(o =>
                Vector3.Distance(o.position, data.position) < 0.01f &&
                Quaternion.Angle(o.rotation, data.rotation) < 1f))
            {
                saveData.objects.Add(data);
            }
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
        Debug.Log("保存されました（ID・プレハブ名つき）: " + json);
    }





}
