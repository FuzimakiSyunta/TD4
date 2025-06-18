using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]  //エディターでも動作させる
public class ObjectSaveManager : MonoBehaviour
{
    public GameObject prefab;
    private string path;
    private Vector3 lastPosition;
    private Vector3 lastRotation;



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
                if (obj.transform.position != lastPosition || obj.transform.rotation.eulerAngles != lastRotation)
                {
                    SaveData();
                    lastPosition = obj.transform.position;
                    lastRotation = obj.transform.rotation.eulerAngles;
                }
            }
        }



    }

    void SaveData()
    {
        ObjectSaveData saveData = new ObjectSaveData();

        //過去のデータを読み込む
        if(File.Exists(path))
        {
           string jsontext = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(jsontext))
            {
                saveData = JsonUtility.FromJson<ObjectSaveData>(jsontext);
            }
            else
            {
                saveData = new ObjectSaveData(); //空の場合は新規作成
            }
        }
        else
        {
            saveData = new ObjectSaveData(); // ファイルが存在しない場合は新規作成
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Savable"))  //"Saveable"  タグのオブジェクトをすべて保存
        {
            ObjectData newData = new ObjectData();
            newData.position = obj.transform.position;
            newData.rotation = obj.transform.rotation.eulerAngles;

            //データの重複を避けるため、新規オブジェクトとして追加
            if (!saveData.objects.Exists(o => o.position == newData.position && o.rotation == newData.rotation))
            {
                saveData.objects.Add(newData);
            }
            //プレハブインスタンスの変更を適用 (エディターのみ)
            if (PrefabUtility.IsPartOfPrefabInstance(obj))
            {
                PrefabUtility.ApplyPrefabInstance(obj, InteractionMode.UserAction);
            }

        }

        Directory.CreateDirectory(Application.dataPath + "/SavedData");
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
        Debug.Log("保存されたデータ: " + File.ReadAllText(path));


        Debug.Log("シーンビューの変更を保存しました。");
    }

   


}
