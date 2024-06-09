using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private ObjectDataBaseSO database;


    private static ObjectDataBaseSO database1;
    [Header("Controller")]
    [SerializeField] private InputActionProperty inputAction;
    [SerializeField] private InputActionProperty inputActionClose;

    
    [SerializeField]
    private GameObject canvas;
    GameObject instantiatedObject;
    private static GameObject  _canvas;

    private static List<Button> btn = new List<Button>();
    private static List<GameObject> listgameobject = new List<GameObject>();

    private static GameObject selectedObject;


    private Vector3 originalCanvasScale;
    private Quaternion originalCanvasRotation;
    private void Start()
    {
        database1 = database;
        _canvas = canvas;

        originalCanvasScale = _canvas.transform.localScale;
        originalCanvasRotation = _canvas.transform.rotation;
    }
     void Update()
    {
        buttonn();
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("test00");
            GameObject currentCanvas = Instantiate(canvas);
            currentCanvas.transform.SetParent(Camera.main.transform);
            currentCanvas.transform.localPosition = new Vector3(0, 0f, 1.5f);
            currentCanvas.transform.localScale = originalCanvasScale;
            currentCanvas.transform.localRotation = Quaternion.identity; 
            Vector3 direction = currentCanvas.transform.position - Camera.main.transform.position;

            currentCanvas.transform.position = Camera.main.transform.position + direction.normalized * 4f;

            _canvas = currentCanvas;

            currentCanvas.SetActive(true);
        }

    }

    private void buttonn()
    {
        float leftValue = inputAction.action.ReadValue<float>();
        float rightValue = inputActionClose.action.ReadValue<float>();

       
    }

    public void Furniture()
    {
        listgameobject.Clear();
        btn.Clear();
        Inventory("Furniture");
    
    }
    public void Decorationt()
    {
         listgameobject.Clear();
        btn.Clear();
        Inventory("Decorationt");
     
    }

    private void Inventory(string jenis)
    {
        Transform Background = _canvas.transform.GetChild(1);
        Transform ListoObjectItem = Background.GetChild(1);
        Transform Viweport = ListoObjectItem.GetChild(0);
        Transform Content = Viweport.GetChild(0);

       /* Transform cloneTransform = Content.Find("List Object(Clone)");
        if (cloneTransform != null)
        {
            Destroy(cloneTransform.gameObject);
        }*/

        Transform ListObject = Content.GetChild(0);

        Transform ListObject1 = ListObject.GetChild(0);
        if (ListObject != null && ListObject.name == "List Object")
        {

            Transform btn = ListObject.GetChild(0);
            Item(btn.gameObject, ListObject.gameObject, jenis, Content.gameObject);
            Debug.Log(Content.gameObject.name);
        }

    }

    private void Item(GameObject item, GameObject Inventory1, string jenis,GameObject content)

    {
        
        btn.Clear();
        foreach (var data in database1.objectsData)
        {
            if (data.Jenis == jenis)
            {
                listgameobject.Add(data.prefab);
            }
        }

        CreateButton(item, Inventory1, listgameobject, content);
    }

    private void CreateButton(GameObject item, GameObject Inventory1, List<GameObject> objects, GameObject content)
    {
        GameObject currentParent = Instantiate(Inventory1, content.transform);
    
        for (int i = 0; i < objects.Count; i++)
        {
            if (currentParent.transform.childCount >= 4)
            {
               

                currentParent = Instantiate(Inventory1, content.transform);
            }

            GameObject inventory = Instantiate(item);
            inventory.transform.SetParent(currentParent.transform);

            inventory.transform.localPosition = Vector3.zero;
            inventory.transform.localScale = item.transform.localScale;
            inventory.transform.rotation = item.transform.rotation;

            Button button = inventory.GetComponent<Button>();
            if (button != null)
            {
                int index = i;
                button.onClick.AddListener(() => OnButtonClick(index));
                btn.Add(button);
            }

            inventory.SetActive(true);
        }
        Inventory1.SetActive(false);

    }

    private void OnButtonClick(int index)
    {
        if (index >= 0 && index < listgameobject.Count)
        {
            GameObject selectedObject = listgameobject[index];
            EquipObject(selectedObject);
        }
    }



    private void EquipObject(GameObject selectedObject)
    {


        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 objectPosition = cameraPosition + Camera.main.transform.forward * 4f;
        Quaternion rotation = Quaternion.LookRotation(cameraPosition - objectPosition);

        GameObject instantiatedItem = Instantiate(selectedObject, objectPosition, Quaternion.identity);


        Transform menuBarang = Camera.main.transform.Find("MenuItem(Clone)");
        listgameobject.Clear();
        btn.Clear();

        if (menuBarang != null)
        {
             Destroy(menuBarang.gameObject);
        }
    }
}
