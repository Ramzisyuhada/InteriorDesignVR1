    using LibCSG;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;
    using static ObjectPlacment;
    using LibCSG;
    using static UnityEngine.XR.Interaction.Toolkit.Inputs.Interactions.SectorInteraction;
using static UI_Interaction;
using UnityEditor.AssetImporters;
using System;

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
        private static Vector3 originalPosition;

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



        private static Quaternion rotasi;
        protected override void Awake()
        {
            CSGOp = new CSGBrushOperation();
            base.Awake();
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null )
            {

                _transformY = objectRenderer.transform.position.y;
                _quaternion = objectRenderer.transform.rotation;

                _currentmaterial = objectRenderer.material;

            }
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        

       
        


        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {

       

      
        rotasi = transform.rotation;
        base.OnSelectEntered(args);
        UI_Interaction ui = new UI_Interaction();

        Controller currentController = UI_Interaction._currentController;
        if (GetComponent<MeshCollider>() != null)

            GetComponent<MeshCollider>().convex = true;
        if (GetComponentInParent<MeshCollider>() != null)

            GetComponentInParent<MeshCollider>().convex = true;
        if (GetComponentInChildren<MeshCollider>() != null)

            GetComponentInChildren<MeshCollider>().convex = true;
       

        Debug.Log(ui.getCurrentController());
        currentController = Controller.Default;
        GetComponent<Rigidbody>().isKinematic = false;

        if (ui.getCurrentController() != Controller.Default)
        {
            ui.setController(currentController);

        }

        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
                gameObject.transform.Find("Locomotion System").gameObject.SetActive(false);

                originalPosition = transform.position;
                /*   if (_material != null)
                   {
                       objectRenderer.material = _material;
                   }*/
              GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
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
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);
        if (GetComponent<MeshCollider>() != null)
            GetComponent<MeshCollider>().convex = false;
        if (GetComponentInParent<MeshCollider>() != null)
            GetComponentInParent<MeshCollider>().convex = false;
        if (GetComponentInChildren<MeshCollider>() != null)
            GetComponentInChildren<MeshCollider>().convex = false;

        /*if (transform.childCount > 0)
            {

                if (GetComponentInChildren<MeshCollider>() != null)
                    GetComponentInChildren<MeshCollider>().convex = false;
            }
            else
            { */

    

            
        /*   if (_material != null)
           {
               objectRenderer.material = _currentmaterial;
           }*/
        /* if (transform.position.y > _transformY && objectType == ObjectType.Furniture)
         {
             Vector3 newPosition = transform.position;
             newPosition.y = _transformY;
             transform.position = originalPosition;

         }*/
        /*    if (GetComponent<MeshCollider>() != null)
            {

                GetComponent<MeshCollider>().convex = false;
            }*/
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().isKinematic = true;


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
        public float snapThreshold = 1.0f; // Jarak maksimal untuk snap

        private bool PlaceOnFloor()
        {
                RaycastHit hit;
                int layerMask = LayerMask.GetMask("Floor"); 


                if (Physics.Raycast(transform.position, Vector3.down, out hit,Mathf.Infinity))
                {

                    if (hit.collider.CompareTag("Floor"))
                    {
                            Vector3 newPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                            transform.position = newPosition;

                            RaycastHit hit1;
                    /* Debug.Log("ehllo1");


                         Debug.Log("ehllo");
                     if (transform.tag == "Partisi")
                     {
                         RaycastHit hit1;

                         foreach (Vector3 direction in directions)
                         {
                             Debug.DrawRay(transform.position, direction , Color.red, 1f);
                             if (Physics.Raycast(transform.position, direction, out hit1, 0.2f))
                             {
                                 Vector3 newPosition1 = new Vector3(Mathf.Round(hit1.transform.position.x), transform.position.y, transform.position.z);

                                 hit1.transform.position = newPosition1;

                                 return true;
                             }
                         }
                     }*/
                    return true;
                    }
                        else {

                            Collider objectCollider = GetComponent<Collider>();
                             transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

                            if (objectCollider != null)
                            {
                                float objectHeight = objectCollider.bounds.size.y;
                                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                            }
                            return true;

                        }




                }
/*        Debug.DrawRay(transform.position, Vector3.right, Color.red);
*/

        return false;
        }

    private bool PlaceOnWall()
    {
        RaycastHit hit;
        Vector3[] raycastDirections = { transform.forward, transform.right, -transform.right };

        bool wallFound = false;
        float closestDistance = Mathf.Infinity;
        Vector3 closestNormal = Vector3.zero;
        Vector3 closestPoint = Vector3.zero;
        
        foreach (var direction in raycastDirections)
        {
            
            if (Physics.Raycast(transform.position, direction, out hit,2F))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    float distanceToHit = Vector3.Distance(transform.position, hit.point);

                    if (distanceToHit < closestDistance)
                    {
                        closestDistance = distanceToHit;
                        closestNormal = hit.normal;
                        closestPoint = hit.point;
                        wallFound = true;
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }
        }

        if (wallFound)
        {
            RaycastHit hitForward;
            RaycastHit hitBackward;

            bool forwardWall = Physics.Raycast(closestPoint + closestNormal * 0.01f, transform.forward, out hitForward, 1f) && hitForward.collider.CompareTag("Wall");
            bool backwardWall = Physics.Raycast(closestPoint - closestNormal * 0.01f, -transform.forward, out hitBackward, 1f) && hitBackward.collider.CompareTag("Wall");

            if (forwardWall && backwardWall)
            {
                Vector3 midpoint = (hitForward.point + hitBackward.point) / 2f;
                transform.position = midpoint;
            }
            else
            {
                Vector3 offset = closestNormal * 0.01f;
                Vector3 targetPosition = closestPoint + offset;
                transform.position = targetPosition;
            }

            // Rotate the object to face against the wall's normal.
            Quaternion targetRotation = Quaternion.LookRotation(-closestNormal, Vector3.up);
            transform.rotation = targetRotation;

            return true;
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
            HashSet<float> Jarak = new HashSet<float>();
            float x;
        
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Surface"))
                {
                    transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
     
                
                    return true;
                }


            }
            return false;
        }

 
        private void Update()
        {
            SnapToPosition();


        }
        /*private void CheckObject( ObjectType type)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position , Vector3.down, out hit, Mathf.Infinity))
            {
                if(hit.collider != null)
                {
                    Debug.Log("Ada Colider");
                }
                else
                {
                    Debug.Log("TIDAK ADA COLIDER");

                }
        }*/
            
         

    }

   
  
    
