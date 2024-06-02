using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ObjectDataBaseSO : ScriptableObject
{
   public List<ObjectData> objectsData; 

}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string name {  get; set; }
    [field: SerializeField] 
    public int id { get; set; }
    [field: SerializeField]
    public GameObject prefab { get; set; }

    [field: SerializeField]
    public List<Material> materials { get; set; } 
}

