using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UI_Interaction : MonoBehaviour
{

    private bool isControllerActive;
    [SerializeField]
    private  ObjectDataBaseSO database;

    private static ObjectDataBaseSO database1;
    [Header("Controller")]
    [SerializeField] private InputActionProperty inputActionLeft;
    [SerializeField] private InputActionProperty inputActionRight;
    [SerializeField] private InputActionProperty inputActionLeftrotate;
    [SerializeField] private InputActionProperty inputActionRightrotate;
    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;

    [Header("Canvas")]
    [SerializeField] private GameObject _canvas;


   

    private static GameObject currentCanvas;
    private Vector3 originalCanvasScale;
    private Quaternion originalCanvasRotation;


    private static GameObject barang;
    private bool nilai = false;


    private static GameObject Inventory;

    private static List<Button> btn = new List<Button>();
    public  enum Controller 
    {
        
        Rotation,
        Scale,
        Texture,
        None
    };
    public static Controller _currentController;  
    void Start()
    {
        database1 = database;
        originalCanvasScale = _canvas.transform.localScale;
        originalCanvasRotation = _canvas.transform.rotation;
            _currentController = Controller.None;
   ;


    }

    void Update()
    {
        getobject();
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
            case Controller.Texture:
                Texture();
                CheckButtonPress();
                break;
            default:
                break;
        }

    }


    void _actionrotate()
    {
        Vector2 leftValue = inputActionLeftrotate.action.ReadValue<Vector2>();
        Vector2 rightValue = inputActionRightrotate.action.ReadValue<Vector2>();

        float rotationSpeed = 100f;
        float rotationAmount = (leftValue.x - rightValue.x) * rotationSpeed * Time.deltaTime;

        barang.transform.Rotate(Vector3.up, rotationAmount);

    }
    private void getobject()
    {
        Ray ray = new Ray(GetRaycastOrigin(), GetRaycastDirection());
        RaycastHit hit;

        float leftValue = inputActionLeft.action.ReadValue<float>();
        float rightValue = inputActionRight.action.ReadValue<float>();
        bool inputActive = leftValue != 0 || rightValue != 0;
        if (inputActive)
        {
            UpdateControllerActiveState();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject != barang)
                    {
                        CleanUpPreviousCanvas();
                        barang = hitObject;
                        if (hitObject != null && !HasChildCanvas(hitObject))
                        {
                            ShowCanvas(hitObject);
                        }
                    }
                }
            }
        }
    }

    private void CleanUpPreviousCanvas()
    {
        if (currentCanvas != null)
        {
            Destroy(currentCanvas);
        }
    }

    bool HasChildCanvas(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            if (child.CompareTag("Canvas")) 
            {
                return true;
            }
        }
        return false;
    }
    private void ShowCanvas(GameObject hitObject)
    {
        currentCanvas = Instantiate(_canvas);

        currentCanvas.transform.SetParent(hitObject.transform);
        currentCanvas.transform.localPosition = new Vector3(0, 1.0f, 0);
        currentCanvas.transform.localScale = originalCanvasScale;

        Vector3 directionToPlayer = Camera.main.transform.position - currentCanvas.transform.position;
        directionToPlayer.y = 0f; 

        currentCanvas.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        currentCanvas.SetActive(true);


    }


    void _actionscale()
    {
        Vector2 leftValue = inputActionLeftrotate.action.ReadValue<Vector2>();
        Vector2 rightValue = inputActionRightrotate.action.ReadValue<Vector2>();

        float scaleSpeed = 2f;
        float scaleFactor = (leftValue.x - rightValue.x) * scaleSpeed * Time.deltaTime;

        Vector3 scaleChange = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        Debug.Log(leftValue.x);

        barang.transform.localScale += scaleChange;
    }


    private Vector3 GetRaycastOrigin()
    {
        if (isControllerActive)
        {
            return rayLeft.transform.position;
        }
        else
        {
            return rayRight.transform.position;
        }
    }

    private Vector3 GetRaycastDirection()
    {
        if (isControllerActive)
        {
            return rayLeft.transform.forward;
        }
        else
        {
            return rayRight.transform.forward;
        }
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

    
    public void Rotate()
    {

        SetControllerType(Controller.Rotation);

    }
    public void Scake()
    {

        SetControllerType(Controller.Scale);
    }
    public void Texture1()
    {

        SetControllerType(Controller.Texture);
    }

    private void Texture()
    {
        Transform Background = currentCanvas.transform.GetChild(0);
        Transform inventoryChild = Background.GetChild(1);
        if (inventoryChild != null && inventoryChild.name == "Inventory")
        {
            inventoryChild.gameObject.SetActive(true);
            Transform InventorySlot = inventoryChild.GetChild(0);

            Item(InventorySlot.gameObject,
            inventoryChild.gameObject);
            Debug.Log(inventoryChild.gameObject.name);
        }

    }

    private void Item(GameObject item,GameObject  Inventory1)

    {
        btn.Clear();
        ObjectData objectData = null;
        foreach (var data in database1.objectsData)
        {
            if (data.prefab.name == barang.name)
            {
                objectData = data;
                break;
            }
        }
        Debug.Log(barang.name);
        for (int i = 0; i < objectData.materials.Count;  i++)
        {
            GameObject inventory = Instantiate(item);
            inventory.transform.SetParent(Inventory1.transform);
            inventory.transform.localPosition = Vector3.zero; 
            inventory.transform.localScale = item.transform.localScale;
            inventory.transform.rotation = item.transform.rotation;
            btn.Add(inventory.GetComponent<Button>());
            Button button = inventory.GetComponent<Button>();
            if (button != null)
            {
                int index = i;
                button.onClick.AddListener(() => OnButtonClick(index));
                btn.Add(button);
            }
            inventory.SetActive(true);
        }
        item.SetActive(false);
        HorizontalLayoutGroup layoutGroup = Inventory1.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutHorizontal(); 
        }

    }
    private void SetControllerType(Controller controllerType)
    {
        Debug.Log("Setting controller type to: " + controllerType);
        _currentController = controllerType;
        Debug.Log("_currentController is now: " + _currentController);
        
    }


    private void OnButtonClick(int index)
    {

        if (barang == null)
        {
            Debug.LogWarning("Barang belum diassign.");
            return;
        }

        ObjectData objectData = null;
        foreach (var data in database1.objectsData)
        {
            if (data.prefab.name == barang.name)
            {
                objectData = data;
                break;
            }
        }
        Renderer renderer = barang.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = objectData.materials[index];
        }
        else
        {
            Debug.LogWarning("Barang tidak memiliki komponen Renderer.");
        }
    }
    private void CheckButtonPress()
    {
        for (int i = 0; i < btn.Count; i++)
        {
            if (btn[i] != null && btn[i].IsInteractable())
            {
                if (rayLeft.TryGetCurrent3DRaycastHit(out RaycastHit hitLeft))
                {
                    if (hitLeft.collider.gameObject == btn[i].gameObject)
                    {
                        if (inputActionLeft.action.WasPressedThisFrame())
                        {
                            btn[i].onClick.Invoke();
                        }
                    }
                }

                if (rayRight.TryGetCurrent3DRaycastHit(out RaycastHit hitRight))
                {
                    if (hitRight.collider.gameObject == btn[i].gameObject)
                    {
                        if (inputActionRight.action.WasPressedThisFrame())
                        {
                            btn[i].onClick.Invoke();
                        }
                    }
                }
            }
        }
    }
}
