using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectsData;

public class ObjectsSaveData : MonoBehaviour
{
    [Serializable]
    public class ObjectSaveData
    {
        public List<ObjectData> objects = new List<ObjectData>();
    }


}
