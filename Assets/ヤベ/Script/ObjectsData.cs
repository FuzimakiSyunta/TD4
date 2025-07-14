using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsData : MonoBehaviour
{
    [Serializable]
    public class ObjectData
    {
        public string id;
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
    }


}
