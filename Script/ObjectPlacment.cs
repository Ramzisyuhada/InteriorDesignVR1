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


    /// <summary>
    /// CSG ALGORITMA
    /// </summary>
    /// 


    protected override void Awake()
    {
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

        Debug.Log("Object Selected: " + gameObject.name);
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
        Debug.Log("Object Deselected: " + gameObject.name);
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
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                transform.rotation = _quaternion;

                transform.position = new Vector3(transform.position.x, transform.position.y, hit.point.z);
                Debug.Log(hit.point.z);
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


                return true;
            }
        }
        return false;
    }

    private bool PlaceOnSurface()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Surface"))
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                return true;
            }


        }
        return false;
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
