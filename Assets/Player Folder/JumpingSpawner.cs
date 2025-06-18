using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public Vector3 position;
    public Vector3 rotation;
}

[System.Serializable]
public class ObjectSaveData
{
    public List<ObjectData> objects = new List<ObjectData>();
}


public class JumpingSpawner : MonoBehaviour
{

    public GameObject prefab; //生成するプレハブ
    [SerializeField]
    private Camera mainCamera;
    private string path;
    void Start()
    {
        path = Application.dataPath + "/SavedData/objectSaveData.json"; // プロジェクトフォルダー内に保存

        ObjectSaveData saveData;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<ObjectSaveData>(json);

            foreach(ObjectData data in saveData.objects)
            {
                Instantiate(prefab, data.position, Quaternion.Euler(data.rotation));
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
                GameObject obj = hit.collider.gameObject;

                // 削除処理: "Savable" タグのオブジェクトなら削除
                if (obj.CompareTag("Savable"))
                {
                    Debug.Log("削除対象のオブジェクト: " + obj.name);
                    DeleteObject(obj);
                }
                else
                {
                    Debug.Log("クリックされたオブジェクトには 'Savable' タグがありません: " + obj.name);
                    CreateObject(hit.point);
                }
            }
        }


    }

    void CreateObject(Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        // 既存の保存データを読み込む
        ObjectSaveData saveData = new ObjectSaveData();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<ObjectSaveData>(json);
        }

        // 新しいオブジェクトのデータを追加
        ObjectData newData = new ObjectData();
        newData.position = obj.transform.position;
        newData.rotation = obj.transform.rotation.eulerAngles;
        saveData.objects.Add(newData);

        SaveJson(saveData);
    }

    void DeleteObject(GameObject obj)
    {
        // 親オブジェクトを取得
        GameObject parentObj = obj.transform.root.gameObject;

        // 既存の保存データを読み込む
        ObjectSaveData saveData = new ObjectSaveData();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<ObjectSaveData>(json);
        }

        // 削除対象のデータをリストから除外（誤差を許容）
        saveData.objects.RemoveAll(o =>
            Vector3.Distance(o.position, parentObj.transform.position) < 0.01f &&
            Vector3.Distance(o.rotation, parentObj.transform.rotation.eulerAngles) < 0.01f);

        // プレハブインスタンスなら適用（エディターのみ）
        if (PrefabUtility.IsPartOfPrefabInstance(parentObj))
        {
            PrefabUtility.ApplyPrefabInstance(parentObj, InteractionMode.UserAction);
        }

        // オブジェクト削除
        Destroy(parentObj);

        // JSONデータを更新
        SaveJson(saveData);


    }

    void SaveJson(ObjectSaveData saveData)
    {
        Directory.CreateDirectory(Application.dataPath + "/SavedData"); // フォルダーがなければ作成
        string newJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, newJson);
    }


}
