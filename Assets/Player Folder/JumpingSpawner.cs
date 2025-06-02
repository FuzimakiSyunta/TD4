using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSpawner : MonoBehaviour
{
    public GameObject prefab; // 生成するオブジェクト
    public int spawnCount = 10; // 生成数
    private MeshCollider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>(); // MeshColliderを取得

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnInsideMesh();
        }
    }

    void Update()
    {

    }

    void SpawnInsideMesh()
    {
        // MeshColliderの範囲からランダムな位置を取得
        Vector3 randomPosition = new Vector3(
            Random.Range(meshCollider.bounds.min.x, meshCollider.bounds.max.x),
            Random.Range(meshCollider.bounds.min.y, meshCollider.bounds.max.y),
            Random.Range(meshCollider.bounds.min.z, meshCollider.bounds.max.z)
        );

        // 生成するオブジェクトを MeshCollider の子に設定
        GameObject newObj = Instantiate(prefab, randomPosition, Quaternion.identity);
        newObj.transform.parent = transform;
    }


}
