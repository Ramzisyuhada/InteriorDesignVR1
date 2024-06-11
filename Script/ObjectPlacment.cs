using LibCSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static ObjectPlacment;
using LibCSG;

public class ObjectPlacment : XRGrabInteractable
{

    public enum ObjectType
    {
        Furniture,
        WallDecor,
        CeilingLight,
        Decoration
    }


    public ObjectType objectType;
    private Renderer objectRenderer;

    [SerializeField]
    private Material _material,_currentmaterial;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Vector3 originalPosition;

    private Quaternion _quaternion;
    private float _transformY;



    static CSGBrush cube;
    static CSGBrush sphere;
    static CSGBrush cylinder;
    static CSGBrushOperation CSGOp = new CSGBrushOperation();
    static CSGBrush cube_inter_sphere;
    static CSGBrush finalres;
    bool Move = false;
    float value_add;

    private GameObject _hasil;
   
    protected override void Awake()
    {
        CSGOp = new CSGBrushOperation();

        base.Awake();
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null ) 
            _currentmaterial = objectRenderer.material;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _transformY = objectRenderer.transform.position.y;
        _quaternion = objectRenderer.transform.rotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {



        base.OnSelectEntered(args);
        if (_material != null)
        {
            objectRenderer.material = _material;
        }
        objectRenderer.GetComponent<Rigidbody>().constraints= RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        objectRenderer.GetComponent<Rigidbody>().isKinematic = false;

    }
    public void CreateBrush(GameObject obj,GameObject hasil)
    {
        cube = new CSGBrush(hasil);
        cube.build_from_mesh(hasil.GetComponent<MeshFilter>().mesh);


        cylinder = new CSGBrush(obj);
        cylinder.build_from_mesh(obj.GetComponent<MeshFilter>().mesh);
    }

    public void CreateObjet(GameObject obj, GameObject hasil)
    {
        Vector3 originalScale = hasil.transform.localScale;
        Bounds originalBounds = hasil.GetComponent<MeshFilter>().mesh.bounds;

        BoxCollider originalCubeCollider = hasil.GetComponent<BoxCollider>();
        Vector3 originalCubeColliderCenter = originalCubeCollider.center;
        Vector3 originalCubeColliderSize = originalCubeCollider.size;
        Debug.Log(originalScale.z);


        CSGOp.merge_brushes(Operation.OPERATION_SUBTRACTION, cube, cylinder, ref finalres);

        hasil.GetComponent<MeshFilter>().mesh.Clear();
        finalres.getMesh(hasil.GetComponent<MeshFilter>().mesh);
        Bounds newBounds = hasil.GetComponent<MeshFilter>().mesh.bounds;


        if (originalBounds.size != newBounds.size)
        {
            Debug.Log("Ukuran mesh berubah, menyesuaikan kembali ke ukuran asli.");

            Vector3 scaleFactor = new Vector3(
                originalBounds.size.x,
                originalBounds.size.y,
                originalBounds.size.z
            );

            hasil.transform.localScale = scaleFactor;


            originalCubeCollider.size = new Vector3(newBounds.size.x, newBounds.size.y, originalScale.z);

            originalCubeCollider.center = new Vector3(newBounds.center.x, newBounds.center.y, 0f);


        }
        else
        {
            Debug.Log("Ukuran mesh tetap sama.");
        }
 
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (_material != null)
        {
            objectRenderer.material = _currentmaterial;
        }
        if (objectRenderer.transform.position.y > _transformY && objectType == ObjectType.Furniture)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _transformY;
            transform.position = newPosition;

        }
        objectRenderer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        objectRenderer.GetComponent<Rigidbody>().isKinematic = true;


    }
    private bool SnapToPosition()
    {
        bool validPlacement = false;

        switch (objectType)
        {
            case ObjectType.Furniture:
                validPlacement = PlaceOnFloor();
                break;
            case ObjectType.WallDecor:
                validPlacement = PlaceOnWall();
                break;
            case ObjectType.CeilingLight:
                validPlacement = PlaceOnCeiling();
                break;
            case ObjectType.Decoration:
                validPlacement = PlaceOnSurface();
                break;
        }

        return validPlacement;
    }

    private bool PlaceOnFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                transform.position = new Vector3(transform.position.x, _transformY, transform.position.z);

                return true;
            }
        }
        return false;
    }

    private bool PlaceOnWall()
    {
        RaycastHit hit;
        RaycastHit[] hits;

        if (Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Wall"))
            {

                transform.position = new Vector3(transform.position.x, transform.position.y, hit.point.z);
                




                return true;
            }
        }
        return false;
    }

    private bool PlaceOnCeiling()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Ceiling"))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

                Debug.Log("Objects on wall:");
                
                return true;
            }
        }
        return false;
    }

    private bool PlaceOnSurface()
    {
        RaycastHit hit;
        HashSet<Collider> collider1 = new HashSet<Collider>();
        HashSet<float> Jarak = new HashSet<float>();
        float x;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Surface"))
            {
                transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                Bounds parentBounds = hit.transform.GetComponent<Renderer>().bounds;

                float objLengthZ = transform.GetComponent<Renderer>().bounds.size.z; 
                float newPositionZ = Mathf.Clamp(hit.point.z, parentBounds.min.z + (objLengthZ / 2f), parentBounds.max.z - (objLengthZ / 2f));
                transform.position = new Vector3(hit.point.x, hit.point.y, newPositionZ);

                float distanceToEdgeZ = Mathf.Abs(transform.position.z - parentBounds.min.z);
                float snappingThreshold = 0.005f;
                if (distanceToEdgeZ <= snappingThreshold)
                {
                    Debug.Log("test");
                    transform.position = new Vector3(hit.point.x, hit.point.y, newPositionZ);

                }
                return true;
            }


        }
        return false;
    }

    void jarak()
    {
      //  Bounds parentBounds = hit.transform.GetComponent<Renderer>().bounds;

        // Menghitung jarak antara posisi objek dan tepi parent pada sumbu z
      //  float distanceToEdgeZ = Mathf.Abs(transform.position.z - parentBounds.min.z);

        // Output atau gunakan nilai jarak yang telah dihitung
       // Debug.Log("Jarak ke tepi parent (sumbu z): " + distanceToEdgeZ);
    }

    private void Update()
    {
        SnapToPosition();


    }
    private void RestrictVerticalMovement()
    {
        Vector3 restrictedPosition = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
        transform.position = restrictedPosition;
    }

   
  
}
