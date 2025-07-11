using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using static ObjectsData;
using static ObjectsSaveData;
public class JumpingSpawner : MonoBehaviour
{
    [Header("ジャンプ台プレハブの登録")] public GameObject smallJumpPadPrefab; public GameObject bigJumpPadPrefab;


    [SerializeField]
    private Camera mainCamera;
    private string path;
    private string currentPrefabName = "小ジャンプ台";

    private Dictionary<string, GameObject> prefabDict;

    void Start()
    {
        path = Application.dataPath + "/SavedData/objectSaveData.json";

        //プレハブ辞書に登録
        prefabDict = new Dictionary<string, GameObject>
    {
        { "小ジャンプ台", smallJumpPadPrefab },
        { "大ジャンプ台", bigJumpPadPrefab }
    };

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ObjectSaveData saveData = JsonUtility.FromJson<ObjectSaveData>(json);

            foreach (ObjectData data in saveData.objects)
            {
                if (prefabDict.TryGetValue(data.prefabName, out GameObject prefab))
                {
                    GameObject obj = Instantiate(prefab, data.position, data.rotation);
                    obj.tag = "Savable";
                    obj.AddComponent<AlreadyLoadedFlag>();

                    var identifier = obj.AddComponent<ObjectIdentifier>();
                    identifier.id = data.id;


                }
                else
                {
                    Debug.LogWarning($"未登録のプレハブ名です: {data.prefabName}");
                }
            }
        }
    }

    void Update()
    {
        //キーでジャンプ台の種類を切り替え
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentPrefabName = "小ジャンプ台";
        if (Input.GetKeyDown(KeyCode.Alpha6)) currentPrefabName = "大ジャンプ台";

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CreateObject(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clicked = hit.collider.gameObject;
                if (clicked.CompareTag("Savable"))
                {
                    DeleteObject(clicked.transform.root.gameObject);
                }
            }
        }
    }

    void CreateObject(Vector3 position)
    {
        if (!prefabDict.ContainsKey(currentPrefabName)) return;
        GameObject obj = Instantiate(prefabDict[currentPrefabName], position, Quaternion.identity);
        obj.tag = "Savable";

        // ObjectIdentifier を追加して一意なIDを割り当てる
        var identifier = obj.AddComponent<ObjectIdentifier>();
        identifier.id = Guid.NewGuid().ToString();

        ObjectSaveData saveData = LoadData();
        ObjectData data = new ObjectData
        {
            id = Guid.NewGuid().ToString(),
            prefabName = currentPrefabName,
            position = obj.transform.position,
            rotation = obj.transform.rotation
        };

        saveData.objects.Add(data);
        SaveJson(saveData);
    }

    //void DeleteObject(GameObject target)
    //{
    //    ObjectSaveData saveData = LoadData();

    //    saveData.objects.RemoveAll(o =>
    //        Vector3.Distance(o.position, target.transform.position) < 0.01f &&
    //        Quaternion.Angle(o.rotation, target.transform.rotation) < 1f);

    //    Destroy(target);
    //    SaveJson(saveData);
    //}

    void DeleteObject(GameObject target)
    {
        ObjectSaveData saveData = LoadData();

        var identifier = target.GetComponent<ObjectIdentifier>();
        if (identifier == null)
        {
            Debug.LogWarning("削除対象にIDがありません（未保存か無効）");
            return;
        }

        saveData.objects.RemoveAll(o => o.id == identifier.id);

        Destroy(target);
        SaveJson(saveData);
        Debug.Log($"削除保存されました: {identifier.id}");
    }



    ObjectSaveData LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ObjectSaveData>(json);
        }
        return new ObjectSaveData();
    }

    void SaveJson(ObjectSaveData saveData)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}