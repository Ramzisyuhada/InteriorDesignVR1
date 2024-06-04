using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class Wallplacment : MonoBehaviour
{
    private bool isCreating;
    private bool isControllerActive;
    private bool isStartSet;
    private bool creating;
    private GameObject startInstance;
    private GameObject endInstance;
    private GameObject WallInstance;
 
    [Header("Prevab Game Object")]
    public GameObject start;
    public GameObject end;
    public GameObject Wall;
   

    [Header("Controller")]

    [SerializeField] private InputActionProperty inputActionLeft;
    [SerializeField] private InputActionProperty inputActionRight;
    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;



     private enum Controller
    {
        Rotation,
        Transform,
        Scale
    }
    void Start()
    {

        isCreating = false;
        isStartSet = false;
       
        startInstance  = Instantiate(start, Vector3.zero, Quaternion.identity);
        endInstance = Instantiate(end, Vector3.zero, Quaternion.identity);
        startInstance.SetActive(false);
        endInstance.SetActive(false);

    }

    void Update()
    {
        HandleInput();

        UpdateControllerActiveState();

    }

    private void UpdateControllerActiveState()
    {
        float leftValue = inputActionLeft.action.ReadValue<float>();
        float rightValue = inputActionRight.action.ReadValue<float>();

        if (leftValue != 0)
        {
            isControllerActive = true;
        }
        else if (rightValue != 0)
        {
            isControllerActive = false;

        }
    }

    Vector3 GridSnap(Vector3 originalPosisition) {

        int granularity = 1;
        Vector3 Snap = new Vector3(Mathf.Floor(originalPosisition.x / granularity) * granularity, originalPosisition.y, Mathf.Floor(originalPosisition.z / granularity) * granularity);
        return Snap;
    }

    private void HandleInput()
    {
        float leftValue = inputActionLeft.action.ReadValue<float>();
        float rightValue = inputActionRight.action.ReadValue<float>();
        bool inputActive = leftValue != 0 || rightValue != 0;

        if (inputActive)
        {
            if (!isStartSet)
            {
                SetStartPoint();
                isStartSet = true;
                isCreating = true;
            }
            else if (!isCreating)
            {
                SetEndPoint();
                isCreating = true;
                isStartSet = false;
            }
        }
        else 

        {

            if(isCreating)
            {
                Adjust();
                isCreating = false;

            }





        }
    }

    private void SetStartPoint()
    {
        startInstance.SetActive(true);
        endInstance.SetActive(true);

        startInstance.transform.position = GridSnap(GetWorldPoint());
        WallInstance  = Instantiate(Wall, startInstance.transform.position,Quaternion.identity);
      

    }

    private void SetEndPoint()
    {

        endInstance.transform.position = GridSnap( GetWorldPoint());

    }
    private void Adjust()
    {
        endInstance.transform.position = GridSnap(GetWorldPoint());
        endInstance.transform.position = new Vector3(startInstance.transform.position.x, endInstance.transform.position.y, endInstance.transform.position.z);

        
        AdjustWall();

    }

    private void AdjustWall()
    {

        startInstance.transform.LookAt(endInstance.transform.position);
        endInstance.transform.LookAt(startInstance.transform.position);
        float distance = Vector3.Distance(startInstance.transform.position, endInstance.transform.position);
        WallInstance.transform.position = startInstance.transform.position + distance/2 * startInstance.transform.forward;
        WallInstance.transform.rotation = startInstance.transform.rotation;
        WallInstance.transform.localScale = new Vector3(WallInstance.transform.localScale.x, WallInstance.transform.localScale.y, distance);



    }
   
    private Vector3 GetWorldPoint()
    {
        if (isControllerActive)
        {
            return rayLeft.rayEndPoint;
        }
        else
        {
            return rayRight.rayEndPoint;
        }
    }
}
