using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibCSG;

public class Sample : MonoBehaviour
{
    CSGBrush cube;
    CSGBrush sphere;
    CSGBrush cylinder;
    CSGBrushOperation CSGOp = new CSGBrushOperation();
    CSGBrush cube_inter_sphere;
    CSGBrush finalres;
    bool Move = false;
    float value_add;






 
    void Start()
    {
        CSGOp = new CSGBrushOperation();
       
        finalres = new CSGBrush(GameObject.Find("Cube1"));
    }
    
    public void CreateBrush()
    {
        cube = new CSGBrush(GameObject.Find("Cube1"));
        cube.build_from_mesh(GameObject.Find("Cube1").GetComponent<MeshFilter>().mesh);


        cylinder = new CSGBrush(GameObject.Find("Cylinder"));
        cylinder.build_from_mesh(GameObject.Find("Cylinder").GetComponent<MeshFilter>().mesh);
    }
    
    public void CreateObjet()
    {
        Vector3 originalScale = GameObject.Find("Cube1").transform.localScale;
        Bounds originalBounds = GameObject.Find("Cube1").GetComponent<MeshFilter>().mesh.bounds;

        BoxCollider originalCubeCollider = GameObject.Find("Cube1").GetComponent<BoxCollider>();
        Vector3 originalCubeColliderCenter = originalCubeCollider.center;
        Vector3 originalCubeColliderSize = originalCubeCollider.size;
        Debug.Log(originalScale.z);


        CSGOp.merge_brushes(Operation.OPERATION_SUBTRACTION, cube, cylinder, ref finalres);

        GameObject.Find("Cube1").GetComponent<MeshFilter>().mesh.Clear();
        finalres.getMesh(GameObject.Find("Cube1").GetComponent<MeshFilter>().mesh);
        Bounds newBounds = GameObject.Find("Cube1").GetComponent<MeshFilter>().mesh.bounds;


        // Jika Anda ingin memeriksa apakah ukuran berubah
        if (originalBounds.size != newBounds.size)
        {
            Debug.Log("Ukuran mesh berubah, menyesuaikan kembali ke ukuran asli.");

            // Hitung faktor skala untuk mengembalikan ukuran asli
            Vector3 scaleFactor = new Vector3(
                originalBounds.size.x ,
                originalBounds.size.y,
                originalBounds.size.z 
            );

            GameObject.Find("Cube1").transform.localScale = scaleFactor;


            originalCubeCollider.size = new Vector3(newBounds.size.x, newBounds.size.y, originalScale.z);

            originalCubeCollider.center = new Vector3(newBounds.center.x, newBounds.center.y, 0f);


        }
        else
        {
            Debug.Log("Ukuran mesh tetap sama.");
        }
        //  originalCubeCollider.center = originalCubeColliderCenter;
        //  originalCubeCollider.size = originalCubeColliderSize;
    }
   
    
    // Update is called once per frame
    void Update()
    {
        
    }

}
