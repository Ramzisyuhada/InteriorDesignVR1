using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSync : MonoBehaviour
{
    public Transform objectInScene;
    public Transform objectOnMap;
    public Vector3 mapScale;

    void Update()
    {
        Vector3 scenePos = objectInScene.position;
        objectOnMap.localPosition = new Vector3(scenePos.x * mapScale.x, scenePos.y * mapScale.y, scenePos.z * mapScale.z);
    }
}
