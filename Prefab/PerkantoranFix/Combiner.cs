using UnityEngine;

public class MergeObjects : MonoBehaviour
{
    public GameObject[] objectsToMerge;

    void Start()
    {
        Merge();
    }

    void Merge()
    {
        // Buat objek induk baru
        GameObject mergedObject = new GameObject("MergedObject");

        // Loop melalui objek-objek yang akan digabungkan
        foreach (GameObject obj in objectsToMerge)
        {
            // Jadikan objek anak dari objek induk baru
            obj.transform.parent = mergedObject.transform;
        }
    }
}
