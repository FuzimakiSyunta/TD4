// Assets/Scripts/BikeMaterialList.cs
using UnityEngine;

[CreateAssetMenu(fileName = "BikeMaterialList", menuName = "Bike/BikeMaterialList")]
public class BikeMaterialList : ScriptableObject
{
    public Material[] materials;
}
