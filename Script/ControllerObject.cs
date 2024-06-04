using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerObject : MonoBehaviour
{
    [Header("Controller")]

    [SerializeField] private InputActionProperty inputActionLeft;
    [SerializeField] private InputActionProperty inputActionRight;


    private enum Controller
    {
        Rotation,
        Scale
    }
    private Controller _currentController;

    public GameObject _barang;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Action();
      
    }

    void Action()
    {
        
            switch (_currentController)
            {
                case Controller.Rotation:
                    _actionrotate();
                    break;        
                case Controller.Scale:
                    _actionscale();
                    break;
                default:
                    break;
            }
        
    }

    void _actionrotate()
    {
        Vector2 leftValue = inputActionLeft.action.ReadValue<Vector2>();
        Vector2 rightValue = inputActionRight.action.ReadValue<Vector2>();

        float rotationSpeed = 100f;
        float rotationAmount = (leftValue.x - rightValue.x) * rotationSpeed * Time.deltaTime;

        _barang.transform.Rotate(Vector3.up, rotationAmount); 
            
    }
    void _actionscale()
    {
        Vector2 leftValue = inputActionLeft.action.ReadValue<Vector2>();
        Vector2 rightValue = inputActionRight.action.ReadValue<Vector2>();

        float scaleSpeed = 0.1f; 

       
        float scaleFactor = (leftValue.y - rightValue.y) * scaleSpeed * Time.deltaTime;

        _barang.transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void Rotate()
    {

     
        SetControllerType(Controller.Rotation);

    }
    public void Scake()
    {


        SetControllerType(Controller.Scale);

    }
    private void SetControllerType(Controller controllerType)
    {
        _currentController = controllerType;
    }
}
