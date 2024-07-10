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
using static UnityEditorInternal.ReorderableList;
using static ObjectPlacment;
using System.Linq;
using Unity.VisualScripting;

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
    [SerializeField] private GameObject _Canvas_pertanyaan;


    [SerializeField]

    GameObject player;

    private static GameObject static_player;
    private static GameObject currentCanvas;
    private static GameObject currentPertanyaan_canvas;
    private static float nilai1 ;
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
        Color,
        None,
        Default,
    };

    public void setController(Controller newController)
    {
        _currentController = newController;
    }
    public GameObject getObjectprefent()
    {
        return barang;
    }
    public Controller getCurrentController()
    {
        return _currentController;
    }

    public static Controller _currentController;

   
    void Start()
    {
       
       
        database1 = database;
        originalCanvasScale = _canvas.transform.localScale;
        originalCanvasRotation = _canvas.transform.rotation;
        _currentController = Controller.Default;


        player = GameObject.Find("XR Origin (XR Rig)");
        GameObject cameraobject = player.transform.GetChild(0).gameObject;
        
        if (rayLeft == null && rayRight == null)
        {
            rayLeft = cameraobject.transform.GetChild(1).GetComponent<XRRayInteractor>();
            rayRight = cameraobject.transform.GetChild(2).GetComponent<XRRayInteractor>();

        }

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
            case Controller.None:
                if (barang != null)
                {
                    SetRigidbodyKinematic(barang);
                    SetMeshColliderConvex(barang);
                }
                break;
            default:
                break;
        }

    }

    void SetRigidbodyKinematic(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
        {
            rb.isKinematic = true;
        }

        Rigidbody rbChild = obj.GetComponentInChildren<Rigidbody>();
        if (rbChild != null && !rbChild.isKinematic)
        {
            rbChild.isKinematic = true;
        }

        Rigidbody rbParent = obj.GetComponentInParent<Rigidbody>();
        if (rbParent != null && !rbParent.isKinematic)
        {
            rbParent.isKinematic = true;
        }
    }

    void SetMeshColliderConvex(GameObject obj)
    {
        MeshCollider mc = obj.GetComponent<MeshCollider>();
        if (mc != null && !mc.convex)
        {
            mc.convex = true;
        }

        MeshCollider mcChild = obj.GetComponentInChildren<MeshCollider>();
        if (mcChild != null && !mcChild.convex)
        {
            mcChild.convex = true;
        }

        MeshCollider mcParent = obj.GetComponentInParent<MeshCollider>();
        if (mcParent != null && !mcParent.convex)
        {
            mcParent.convex = true;
        }
    }

    void _actionrotate()
    {
        
        Vector2 leftValue = inputActionLeftrotate.action.ReadValue<Vector2>();
        Vector2 rightValue = inputActionRightrotate.action.ReadValue<Vector2>();

        float rotationSpeed = 100f;
        float rotationAmount = (leftValue.x - rightValue.x) * rotationSpeed * Time.deltaTime;

       
        if (barang.transform.parent != null)
        {
            barang.transform.parent.transform.Rotate(0, rotationAmount, 0, Space.Self);

        }
        else
        {
            barang.transform.transform.Rotate(0, rotationAmount, 0, Space.Self);

        }

    }
    private void getobject()
    {

        Ray ray = new Ray(GetRaycastOrigin(), GetRaycastDirection());
        RaycastHit hit;

        float leftValue = inputActionLeft.action.ReadValue<float>();
        float rightValue = inputActionRight.action.ReadValue<float>();
        bool inputActive = leftValue > 0.5f || rightValue > 0.5f;
        
        Debug.Log(inputActive);
        
            UpdateControllerActiveState();

            int layerMaskUI = 1 << LayerMask.NameToLayer("Door");
            int layerMaskPlayer = 1 << LayerMask.NameToLayer("Player"); // Example: Layer for player
            int combinedLayerMask = layerMaskUI | layerMaskPlayer;

            if (rayLeft.TryGetCurrent3DRaycastHit(out hit) && inputActionLeft.action.WasPressedThisFrame())
            {
                _objectRaycast( hit);               
            }

            if (rayRight.TryGetCurrent3DRaycastHit(out hit) && inputActionRight.action.WasPressedThisFrame())
            {
                _objectRaycast(hit);

            }

    }
    private void _objectRaycast(RaycastHit hit)
    {
        if (IsPointerOverUIElement()) return;

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            /*                    if (barang != null) barang.GetComponent<Rigidbody>().isKinematic = true;
            */
            if (hitObject != null && hitObject.tag != "Pertanyaan")
            {
                static_player = player;
                barang = hitObject;
                setController(Controller.Default);

                gameobject = barang;
                ShowCanvas(barang);
                Destroy(GameObject.Find("MenuItem_part1_fix(Clone)"));

            }
            

            if (hitObject.GetComponent<Canvas>() != null)
            {
                return;
            }
        }
    }

 
    public void Close()
    {
        if(barang.transform.tag != "Floor")
        {
            SetControllerType(Controller.None);

        }
        else
        {
            SetControllerType(Controller.Default);

        }

        if (barang.GetComponentInChildren<MeshCollider>() != null && barang.transform.tag != "Floor")
            barang.GetComponentInChildren<MeshCollider>().convex = true;
        if (barang.GetComponent<MeshCollider>() != null && barang.transform.tag != "Floor")
            barang.GetComponent<MeshCollider>().convex = true;
        if (barang.GetComponentInParent<MeshCollider>() != null && barang.transform.tag != "Floor")
            barang.GetComponentInParent<MeshCollider>().convex = true;
        Destroy(GameObject.Find("Canvas(Clone)"));
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);
        Destroy(currentCanvas);

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

    private void DeleteFeature(String name,GameObject canvas)
    {
        Transform feature = canvas.transform.GetChild(0).GetChild(0);

        if (name == "Wall Decoration")
        {
            feature.Find("Rotate").gameObject.SetActive(false);
            feature.Find("Texture").gameObject.SetActive(false);
            feature.Find("Scale").gameObject.SetActive(true);
            feature.Find("Hapus").gameObject.SetActive(true);
        }
        else if(name == "Surface" || name == "Furniture" || name == "Decoration" || name == "Ceiling" || name == "Partisi")
        {
            feature.Find("Rotate").gameObject.SetActive(true);
            feature.Find("Texture").gameObject.SetActive(false);
            feature.Find("Scale").gameObject.SetActive(true);
            feature.Find("Hapus").gameObject.SetActive(true);

        }else if (name == "Ceiling")
        {
            feature.Find("Rotate").gameObject.SetActive(false);
            feature.Find("Texture").gameObject.SetActive(false);
            feature.Find("Scale").gameObject.SetActive(false);
            feature.Find("Hapus").gameObject.SetActive(true);
        }
        else
        {
            feature.Find("Rotate").gameObject.SetActive(false);
            feature.Find("Texture").gameObject.SetActive(true);
            feature.Find("Scale").gameObject.SetActive(false);
            feature.Find("Hapus").gameObject.SetActive(false);
        }
    }
    private void ShowCanvas(GameObject hitObject)

    {
        Debug.Log("asdasd");
        GameObject player = GameObject.Find("XR Origin (XR Rig)");

        if (currentCanvas == null)
        {
            currentCanvas = Instantiate(_canvas);
            Debug.Log(currentCanvas.gameObject.name);
            Vector3 cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;
            Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 2.5f;
            currentCanvas.transform.DOMove(new Vector3(targetPosition.x, player.transform.position.y + 1 , targetPosition.z), 1f);

            currentCanvas.transform.DORotate(cameraRotation.eulerAngles, 0.5f);

            currentCanvas.transform.DORestart();

            DOTween.Play(currentCanvas);
            if(hitObject.transform.root.gameObject != null){
                DeleteFeature(hitObject.transform.root.gameObject.tag, currentCanvas);

            }
            else
            {
                DeleteFeature(hitObject.gameObject.tag, currentCanvas);

            }

        }
        else
        {

            _currentController = Controller.Default;
            Transform childTransform = currentCanvas.transform.GetChild(0);
            GameObject existingColorPicker = childTransform.transform.Find("Color picker(Clone)")?.gameObject;
            if (existingColorPicker != null)
            {
                Destroy(existingColorPicker);
            }
            Vector3 cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;

            Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 2.5f;
            currentCanvas.transform.DOMove(new Vector3(targetPosition.x, player.transform.position.y + 1, targetPosition.z),1f);

            currentCanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

            currentCanvas.transform.DORestart();

            DOTween.Play(currentCanvas);
            if (hitObject.transform.root.gameObject != null)
            {
                DeleteFeature(hitObject.transform.root.gameObject.tag, currentCanvas);

            }
            else
            {
                DeleteFeature(hitObject.gameObject.tag, currentCanvas);

            }

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
        // Check if the object has a Rigidbody component
       /* if (barang.GetComponent<Rigidbody>() != null)
        {
            barang.GetComponent<Rigidbody>().isKinematic = false;
        }*/

        Vector2 leftValue = inputActionLeftrotate.action.ReadValue<Vector2>();
        Vector2 rightValue = inputActionRightrotate.action.ReadValue<Vector2>();

        float scaleSpeed = 2f;

        float scaleFactor = (leftValue.x - rightValue.x) * scaleSpeed * Time.deltaTime;

        Vector3 scaleChange = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        Debug.Log(leftValue.x);

        if (barang.transform.parent != null)
        {



           
             barang.transform.parent.transform.localScale += scaleChange;

           

        }
        else
        {
            barang.transform.localScale += scaleChange;
        }
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

        if (barang.GetComponentInChildren<MeshCollider>() != null)
            barang.GetComponentInChildren<MeshCollider>().convex = true;
        if (barang.GetComponent<MeshCollider>() != null)
            barang.GetComponent<MeshCollider>().convex = true;
        if (barang.GetComponentInParent<MeshCollider>() != null)
            barang.GetComponentInParent<MeshCollider>().convex = true;

        if (barang.GetComponentInParent<Rigidbody>() != null)
        {
/*            barang.GetComponentInParent<Rigidbody>().isKinematic = false;
*/            barang.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        if (barang.GetComponent<Rigidbody>() != null)
        {
/*            barang.GetComponent<Rigidbody>().isKinematic = false;
*/            barang.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        }
        if (barang.GetComponentInChildren<Rigidbody>() != null)
        {
/*            barang.GetComponentInChildren<Rigidbody>().isKinematic = false;
*/            barang.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        }
        SetControllerType(Controller.Rotation);
        GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
        gameObject.transform.Find("Locomotion System").gameObject.SetActive(false);
    }
    public void Scake()
    {
        if (barang.GetComponentInChildren<MeshCollider>() != null)
            barang.GetComponentInChildren<MeshCollider>().convex = true;
        if (barang.GetComponent<MeshCollider>() != null)
            barang.GetComponent<MeshCollider>().convex = true;
        if (barang.GetComponentInParent<MeshCollider>() != null)
            barang.GetComponentInParent<MeshCollider>().convex = true;
        if (barang.GetComponentInParent<Rigidbody>() != null)
        {
            barang.GetComponentInParent<Rigidbody>().isKinematic = false;
            barang.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        if (barang.GetComponent<Rigidbody>() != null)
        {
            barang.GetComponent<Rigidbody>().isKinematic = false;
            barang.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        }
        if (barang.GetComponentInChildren<Rigidbody>() != null)
        {
            barang.GetComponentInChildren<Rigidbody>().isKinematic = false;
            barang.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        }
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




        if (barang.transform.root.gameObject != null)
        {
            Destroy(barang.transform.root.gameObject);

        }
        else
        {
            Destroy(barang);

        }
        Destroy(gameobject);
        Destroy(currentCanvas);
    }
    private void Texture()
    {
        Transform childTransform = currentCanvas.transform.GetChild(0); 
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

        if (gameobject != null && existingColorPicker != null)
        {
            
            existingColorPicker.GetComponent<ColorPicker>();
            if (existingColorPicker.TryGetComponent<ColorPicker>(out var cp))
            {
                if (gameobject.GetComponent<Renderer>() != null)
                {
                    gameobject.GetComponent<Renderer>().material.color = cp.color;
                }
                else
                {
                    if (gameobject.transform.childCount != 0)
                    {

                        for (int i = 0; i < gameobject.transform.childCount; i++)
                        {
                           
                            if(gameobject.transform.GetChild(i).GetComponent<Renderer>() != null)
                            {
                                gameobject.transform.GetChild(i).GetComponent<Renderer>().material.color = cp.color;

                            }

                        }
                    }
                }
                
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
