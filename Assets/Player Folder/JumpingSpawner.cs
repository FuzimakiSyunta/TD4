using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        //クリックしたらオブジェクト生成&保存
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                GameObject obj = Instantiate(prefab, hit.point, Quaternion.identity);

                //データ保存用リストを作成
                ObjectSaveData saveData = new ObjectSaveData();

                if(File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    saveData = JsonUtility.FromJson<ObjectSaveData>(json);
                }

                ObjectData newData = new ObjectData();
                newData.position = obj.transform.position;
                newData.rotation = obj.transform.rotation.eulerAngles;
                saveData.objects.Add(newData);

                //JSONに変換して保存
                Directory.CreateDirectory(Application.dataPath + "/SavedData"); // フォルダーがなければ作成
                string newJson = JsonUtility.ToJson(saveData);
                File.WriteAllText(path,newJson);
                
            }
        }
    }
}
