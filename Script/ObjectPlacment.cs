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
using UnityEngine.UIElements;
using System.Xml.Linq;

public class ObjectPlacment : XRGrabInteractable
    {

        public enum ObjectType
        {
            Furniture,
            WallDecor,
            CeilingLight,
            Decoration,
            None
        }

        public ObjectType GetType()
        {

        return objectType; 
    
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

    private bool isBeingHeld = false;


    static CSGBrush cube;
        static CSGBrush sphere;
        static CSGBrush cylinder;
        static CSGBrushOperation CSGOp = new CSGBrushOperation();
        static CSGBrush cube_inter_sphere;
        static CSGBrush finalres;
        bool Move = false;
        float value_add;

/*        private GameObject _hasil;
*/

        private Vector3 _preposisiiton;
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


        isBeingHeld = true;


        rotasi = transform.rotation;
        base.OnSelectEntered(args);
        UI_Interaction ui = new UI_Interaction();
        _preposisiiton = transform.position;
        Controller currentController = UI_Interaction._currentController;
        if (GetComponent<MeshCollider>() )

            GetComponent<MeshCollider>().convex = true;
        if (GetComponentInParent<MeshCollider>() != null)

            GetComponentInParent<MeshCollider>().convex = true;
        if (GetComponentInChildren<MeshCollider>() != null)

            GetComponentInChildren<MeshCollider>().convex = true;
       

        Debug.Log(ui.getCurrentController());
        currentController = Controller.Default;
        GetComponent<Rigidbody>().isKinematic = false;

        if (ui.getObjectprefent() != null && ui.getObjectprefent().name != transform.gameObject.name && ui.getObjectprefent().GetComponent<Rigidbody>() != null)
        {
            ui.getObjectprefent().GetComponent<Rigidbody>().isKinematic = true;

        }
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

        Debug.Log(IsAboveOtherObject());

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (isBeingHeld && collision.transform.tag == "Floor")
        {
            floorcheck = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isBeingHeld && collision.transform.tag == "Floor")
        {
            floorcheck = false;
        }
    }
    private bool IsAboveOtherObject()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        float rayLength = 1.0f;

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red, 0.1f);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                
                    return true;
                
            }
        }

        // Jika tidak mengenai apapun atau tidak mengenai "Floor", objek tidak berada di atas objek lain
        return false;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        isBeingHeld = false;

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
    /*    if (!floorcheck && objectType == ObjectType.Furniture)
        {
            transform.position = _preposisiiton;

        }*/
        Debug.Log(IsAboveOtherObject());
      /*  if (!floorcheck && objectType == ObjectType.Furniture)
        {

            transform.position = _preposisiiton;

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
    private bool tembus = false;
        private void OnTriggerEnter(Collider other)
        {
            
                tembus = true;
                Debug.Log("Object entered trigger with " + other.gameObject.name);

        }
    
    private bool PlaceOnFloor()
        {
                RaycastHit hit;
                int layerMask = LayerMask.GetMask("Floor"); 
               

        if (Physics.Raycast(transform.position, Vector3.down , out hit,1f))
                {

                    if (hit.collider.CompareTag("Floor"))
                    {
                    Collider objectCollider = GetComponent<Collider>();

                    if (objectCollider == null)
                    {
                        objectCollider = GetComponentInChildren<Collider>();
                    }

                        

                            
                           
                                Vector3 newPosition = new Vector3(transform.position.x, hit.point.y + 0.01f, transform.position.z);
                                transform.position = newPosition;
                            

                            return true;
                        

                return true;
                    }
                       /* else {
                        
                            Collider objectCollider = GetComponent<Collider>();

                            if (objectCollider != null)
                            {
                                float objectHeight = objectCollider.bounds.size.y;
                                transform.position = new Vector3(transform.position.x, hit.point.y+ objectHeight, transform.position.z);
                            }
                            return true;

                        }*/




                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.down * 2, Color.red);

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
            RaycastHit[] hitsForward = Physics.RaycastAll(closestPoint + closestNormal * 0.01f, transform.forward, 1f);
            RaycastHit[] hitsBackward = Physics.RaycastAll(closestPoint - closestNormal * 0.01f, -transform.forward, 1f);

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
                Vector3 offset = closestNormal * 0.05f;
                Vector3 targetPosition = closestPoint + offset;
                transform.position = targetPosition;
            }

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
        public static bool floorcheck;
/*     public void OnTriggerEnter(Collider collider)
     {
        if (collider.gameObject.tag == "Floor")
        {
            Debug.Log("Sedang di floor");
           floorcheck = true;
        }
     }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Floor")
        {
            floorcheck = false;
        }
    }*/

    private bool PlaceOnSurface()
{
    RaycastHit hit;

    if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
    {
        if (hit.collider.CompareTag("Surface"))
        {
            float hitY = hit.point.y;
            float minY = hit.collider.bounds.min.y;
            float centerY = hit.collider.bounds.center.y;
            float maxY = hit.collider.bounds.max.y;

            // Define a small offset to prevent the object from sinking below the surface
            float offset = 0.01f;

            if (Mathf.Approximately(hitY, minY))
            {
                transform.position = new Vector3(hit.point.x, minY + offset, hit.point.z);
                Debug.Log("Hit at the bottom");
            }
            else if (Mathf.Approximately(hitY, centerY))
            {
                transform.position = new Vector3(hit.point.x, centerY + offset, hit.point.z);
                Debug.Log("Hit at the center");
            }
            else if (Mathf.Approximately(hitY, maxY))
            {
                transform.position = new Vector3(hit.point.x, maxY + offset, hit.point.z);
                Debug.Log("Hit at the top");
            }
            else
            {
                transform.position = new Vector3(hit.point.x, hitY + offset, hit.point.z);
                Debug.Log("Hit somewhere in between");
            }

            return true;
        }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);

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

   
  
    
