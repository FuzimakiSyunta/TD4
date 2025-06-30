using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

[System.Serializable]
public class ObjectData
{
    public string id; 
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class ObjectSaveData
{
    public List<ObjectData> objects = new List<ObjectData>();
}

public class JumpingSpawner : MonoBehaviour
{
    public GameObject prefab; // プレハブ
    [SerializeField] private Camera mainCamera;
    private string path;

    void Start()
    {
        path = Application.dataPath + "/SavedData/objectSaveData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ObjectSaveData saveData = JsonUtility.FromJson<ObjectSaveData>(json);

            foreach (ObjectData data in saveData.objects)
            {
                GameObject obj = Instantiate(prefab, data.position,data.rotation);
                obj.tag = "Savable"; // 忘れずにタグを設定
                obj.AddComponent<AlreadyLoadedFlag>(); 
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clicked = hit.collider.gameObject;

                if (clicked.CompareTag("Savable"))
                {
                    DeleteObject(clicked.transform.root.gameObject);
                }
                else
                {
                    CreateObject(hit.point);
                }
            }
        }
    }

    void CreateObject(Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.tag = "Savable";

        ObjectSaveData saveData = LoadData();
        ObjectData newData = new ObjectData
        {
            id = Guid.NewGuid().ToString(),
            position = obj.transform.position,
            rotation = obj.transform.rotation
        };
      

        // IDがまだない場合だけ追加
        if (!saveData.objects.Any(o => o.id == newData.id))
        {
            saveData.objects.Add(newData);
            SaveJson(saveData); // ← 追加したときだけ保存
        }


    }

    void DeleteObject(GameObject target)
    {
        ObjectSaveData saveData = LoadData();

        // 削除処理：位置と回転がほぼ一致するデータを除去
        saveData.objects.RemoveAll(o =>
            Vector3.Distance(o.position, target.transform.position) < 0.01f &&
            Quaternion.Angle(o.rotation, target.transform.rotation) < 1f);

        Destroy(target);

        // 削除済みデータを保存
        SaveJson(saveData);


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
        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(path, json);
    }
}

