using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using UnityEngine.EventSystems;

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
    [SerializeField]
    private GameObject colorpicker;

    [SerializeField]

    GameObject player;

    private static GameObject static_player;
    private static GameObject currentCanvas;
    private Vector3 originalCanvasScale;
    private Quaternion originalCanvasRotation;


    private static GameObject barang;
    private bool nilai = false;


    private static GameObject Inventory;
    private static GameObject gameobject;
    private static List<Button> btn = new List<Button>();



    public ColorPicker colorPicker;
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
        if (IsPointerOverUIElement()) return;

        Ray ray = new Ray(GetRaycastOrigin(), GetRaycastDirection());
        RaycastHit hit;

        float leftValue = inputActionLeft.action.ReadValue<float>();
        float rightValue = inputActionRight.action.ReadValue<float>();
        bool inputActive = leftValue != 0 || rightValue != 0;
        
        Debug.Log(inputActive);
        if (inputActive)
        {
            UpdateControllerActiveState();

            int layerMaskUI = 1 << LayerMask.NameToLayer("UI");
            int layerMaskPlayer = 1 << LayerMask.NameToLayer("Floor");
            int combinedLayerMask = layerMaskUI ;

            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                                    Debug.Log(hit.collider.name);

                if (hit.collider != null)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject != barang)
                    {
                        barang = hitObject;
                        if (hitObject != null && !HasChildCanvas(hitObject))
                        {
                            gameobject = hitObject.gameObject;
                            static_player = player;

                            ShowCanvas(hitObject);
                            Destroy(GameObject.Find("MenuItem(Clone)"));

                        }
                    }
                    if (hitObject.GetComponent<Canvas>() != null)
                    {
                        return;
                    }
                }
            }
        }
    }

    public void Close()
    {
        SetControllerType(Controller.None);

        Destroy(GameObject.Find("Canvas(Clone)"));
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);
    }
    private bool IsPointerOverUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = FindObjectOfType<GraphicRaycaster>();
        if (raycaster != null)
        {
            raycaster.Raycast(eventData, results);
            return results.Count > 0;
        }
        return false;
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
       
        
        if (currentCanvas == null)
        {

            currentCanvas = Instantiate(_canvas);
            Debug.Log(currentCanvas.gameObject.name);
            Vector3 cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;

            Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 3.5f;
            currentCanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y / 2, targetPosition.z), 1f);

            currentCanvas.transform.DORotate(cameraRotation.eulerAngles, 0.5f);

            currentCanvas.transform.DORestart();

            DOTween.Play(currentCanvas);
        }
        else
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;

            Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 3.5f;
            currentCanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y / 2, targetPosition.z),1f);

            currentCanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

            currentCanvas.transform.DORestart();

            DOTween.Play(currentCanvas);

        }



    }

    private Bounds GetBoundsOfSelectedObject(GameObject selectedObject)
    {
        Renderer renderer = selectedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on selectedObject.");
            return new Bounds(Vector3.zero, Vector3.zero);
        }
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
            Debug.Log("Kiri Aktif");
        }
        else if (rightValue != 0)
        {
            isControllerActive = false;
            Debug.Log("Kanan Aktif");

        }
    }

    
    public void Rotate()
    {

        SetControllerType(Controller.Rotation);
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(false);
    }
    public void Scake()
    {

        SetControllerType(Controller.Scale);
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(false);
    }
    public void Texture1()
    {

        SetControllerType(Controller.Texture);
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(false);
    }

    public void Destroy1()
    {
        SetControllerType(Controller.None);
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);

        Destroy(gameobject);
        Destroy(currentCanvas);
    }
    private void Texture()
    {
        Transform childTransform = currentCanvas.transform.GetChild(3); 
        GameObject existingColorPicker = childTransform.transform.Find("Color picker(Clone)")?.gameObject;

        colorPicker.onColorChanged += OnColorChanged;

        if (existingColorPicker == null)
        {
            GameObject newColorPicker = Instantiate(colorpicker);
            newColorPicker.transform.SetParent(childTransform, false);
        }
        else
        {
           
            colorPicker.onColorChanged -= OnColorChanged;
        }

        if (gameobject != null)
        {
            ColorPicker cp = existingColorPicker.GetComponent<ColorPicker>();
            if (cp != null)
            {
                gameobject.GetComponent<Renderer>().material.color = cp.color;
            }
            else
            {
                Debug.LogWarning("ColorPicker component not found on existingColorPicker.");
            }
        }
        else
        {
            Debug.LogWarning("gameobject not assigned.");
        }
    }


    public void OnColorChanged(Color c)
    {
    }

    private void OnDestroy()
    {
        if (colorPicker != null)
            colorPicker.onColorChanged -= OnColorChanged;
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
