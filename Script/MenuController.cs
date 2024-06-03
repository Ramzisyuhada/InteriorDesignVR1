using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuController : XRGrabInteractable
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

    private void Start()
    {
        database1 = database;

    }
    private void Update()
    {
        buttonn();
    }

    private void buttonn()
    {
        float leftValue = inputAction.action.ReadValue<float>();
        float rightValue = inputActionClose.action.ReadValue<float>();


        if (leftValue == 1 && instantiatedObject == null)
        {
            instantiatedObject = Instantiate(canvas, Camera.main.transform.position, Quaternion.identity);

            instantiatedObject.transform.LookAt(Camera.main.transform);

            instantiatedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            _canvas = instantiatedObject;
        }
        else if (rightValue == 1 && instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            _canvas = null;
        }
    }

    public void Furniture()
    {
        Inventory("Furniture");
    }


    private void Inventory(string jenis)
    {
        Transform Background = _canvas.transform.GetChild(0);
        Transform inventoryChild = Background.GetChild(1);
        if (inventoryChild != null && inventoryChild.name == "Inventory")
        {
            inventoryChild.gameObject.SetActive(true);
            Transform InventorySlot = inventoryChild.GetChild(0);

            Item(InventorySlot.gameObject,
            inventoryChild.gameObject, jenis);
            Debug.Log(inventoryChild.gameObject.name);
        }

    }

    private void Item(GameObject item, GameObject Inventory1, string jenis)

    {
        
        btn.Clear();
        foreach (var data in database1.objectsData)
        {
            if (data.Jenis == jenis)
            {
                listgameobject.Add(data.prefab);
            }
        }

        CreateButton(item, Inventory1, listgameobject);
    }

    private void CreateButton(GameObject item, GameObject Inventory1,List<GameObject> objects) 
    {
        for (int i = 0; i < objects.Count; i++)
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
    }

    private void OnButtonClick(int index)
    {
        if (index >= 0 && index < listgameobject.Count)
        {
            GameObject selectedObject = listgameobject[index];
            Debug.Log("Selected item: " + selectedObject.name);
            EquipObject(selectedObject);
        }
    }



    private void EquipObject(GameObject selectedObject)
    {

        Vector3 newPosition = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, Camera.main.transform.position.z * 2);
        GameObject instantiatedItem = Instantiate(selectedObject, newPosition, Quaternion.identity);

    }
}
