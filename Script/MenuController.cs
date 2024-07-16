using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using static UI_Interaction;

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
    private static GameObject _canvas;
    private static GameObject menucanvas;

    private static List<Button> btn = new List<Button>();
    private static List<GameObject> listgameobject = new List<GameObject>();
    private static List<Sprite> Sprite = new List<Sprite>();
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

    public void  ShowCanvas()
    {
       if (menucanvas == null)
            {
                GameObject currentCanvas = Instantiate(canvas);
                menucanvas = currentCanvas;
                UI_Interaction ui = new UI_Interaction();
                Controller currentController = UI_Interaction._currentController;

                currentController = Controller.Default;
                ui.setController(currentController);
                GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
                gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 4.5f;
                menucanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

                menucanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                menucanvas.transform.DORestart();

                DOTween.Play(menucanvas);

            }
            else
            {
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 4.5f;
                menucanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

                menucanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                menucanvas.transform.DORestart();

                DOTween.Play(menucanvas);
            }
       if(GameObject.Find("Menu(Clone)") != null ) Destroy(GameObject.Find("Menu(Clone)"));

    }
    void Update()                                                                 
    {
       /* buttonn();
        float leftValue = inputAction.action.ReadValue<float>();
        float rightValue = inputActionClose.action.ReadValue<float>();

        if (leftValue != 0 || rightValue != 0)
        {
            if (menucanvas == null)
            {
                GameObject currentCanvas = Instantiate(canvas);
                menucanvas = currentCanvas;
                UI_Interaction ui = new UI_Interaction();
                Controller currentController = UI_Interaction._currentController;
              
                currentController = Controller.Default;
                ui.setController(currentController);
                GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
                gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 4.5f;
                menucanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y , targetPosition.z), 1f);

                menucanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                menucanvas.transform.DORestart();

                DOTween.Play(menucanvas);

            }
            else
            {
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 4.5f;
                menucanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y , targetPosition.z), 1f);

                menucanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                menucanvas.transform.DORestart();

                DOTween.Play(menucanvas);
            }
          *//*  if (GameObject.Find("Canvas 1(Clone)") != null ) Destroy(GameObject.Find("Canvas 1(Clone)"));
            if (GameObject.Find("How To(Clone)") != null) Destroy(GameObject.Find("How To(Clone)"));*//*


        }




        if (Input.GetKeyDown(KeyCode.B))
        {
            if (menucanvas == null)
            {
                UI_Interaction ui = new UI_Interaction();
                Controller currentController = UI_Interaction._currentController;
                currentController = Controller.Default;
                GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
                gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);


                ui.setController(currentController);
                GameObject currentCanvas = Instantiate(canvas);
                menucanvas = currentCanvas;
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;
                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 2.5f;
                menucanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

                menucanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                menucanvas.transform.DORestart();

                DOTween.Play(menucanvas);

            }
            else
            {
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 2.5f;
                menucanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y , targetPosition.z), 1f);

                menucanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                menucanvas.transform.DORestart();

                DOTween.Play(menucanvas);
            }
            if (GameObject.Find("Canvas 1(Clone)") != null) Destroy(GameObject.Find("Canvas 1(Clone)"));
            if (GameObject.Find("How To(Clone)") != null) Destroy(GameObject.Find("How To(Clone)"));
            if (GameObject.Find("Pertanyaan(Clone)") != null) Destroy(GameObject.Find("Pertanyaan(Clone)"));

        }
*/







    }

    public void Close()
    {
        Destroy(menucanvas);

    }
    private void ResetUI()
    {
        Transform content = menucanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        Debug.Log(content.gameObject.name);
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Transform child = content.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        Transform listObjectTemplate = content.GetChild(0);
        listObjectTemplate.gameObject.SetActive(true);
    }
    private void buttonn()
    {
        float leftValue = inputAction.action.ReadValue<float>();
        float rightValue = inputActionClose.action.ReadValue<float>();

        Debug.Log(leftValue + " " + rightValue);
    }

    public void Tanaman()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();

        Inventory("Tanaman");

    }
    public void Dekorasi()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();

        Inventory("Dekorasi");

    }

    public void Lampu()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();

        Inventory("Lampu");
    }

    public void Jendela()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Jendela");
    }

    public void Karpet()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Karpet");
    }

    public void Kursi()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Kursi & Sofa");

    }

    public void Lemari()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Lemari & Rak");

    }

    public void Lukisan()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Lukisan");

    }

    public void Meja()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Meja");
    }

    public void partisi()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Partisi");

    }

    public void Pintu()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Pintu");

    }

    public void Plants()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Tanaman");

    }

    public void Sampah()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Tempat Sampah");

    }

    public void Toilet()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Toilet");

    }

    public void Elektronik()
    {
        ResetUI();

        listgameobject.Clear();
        btn.Clear();
        Sprite.Clear();
        Inventory("Elektronik");

    }
    private void Inventory(string jenis)
    {
        Transform Background = menucanvas.transform.GetChild(0);
        Transform ListoObjectItem = Background.GetChild(1);
        Transform Viweport = ListoObjectItem.GetChild(0);
        Transform Content = Viweport.GetChild(0);


        Transform ListObject = Content.GetChild(0);

        Transform ListObject1 = ListObject.GetChild(0);
        if (ListObject != null && ListObject.name == "List Object")
        {

            Transform btn = ListObject.GetChild(0);
            Item(btn.gameObject, ListObject.gameObject, jenis, Content.gameObject);
            Debug.Log(Content.gameObject.name);
        }

    }

    private void Item(GameObject item, GameObject Inventory1, string jenis, GameObject content)

    {

        btn.Clear();
        foreach (var data in database1.objectsData)
        {
            if (data.Jenis == jenis)
            {
                Debug.Log(jenis);
                listgameobject.Add(data.prefab);
                Sprite.Add(data.SourceImage);

            }
        }

        CreateButton(item, Inventory1, listgameobject, content, Sprite);
    }

    private void CreateButton(GameObject item, GameObject Inventory1, List<GameObject> objects, GameObject content, List<Sprite> img)
    {
        /*foreach (Transform data in content.transform)
        {
            Destroy(data.gameObject);
        }*/
        Inventory1.SetActive(true);
        GameObject currentParent = Instantiate(Inventory1, content.transform);

        for (int i = 0; i < objects.Count; i++)
        {
            if (currentParent.transform.childCount >= 4)
            {


                currentParent = Instantiate(Inventory1, content.transform);



            }

            GameObject inventory = Instantiate(item);
            inventory.GetComponent<Image>().sprite = img[i];
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

        Vector3 cameraPosition = Camera.main.transform.position ;
        Vector3 objectPosition = cameraPosition + Camera.main.transform.forward * 2;
/*        objectPosition.y = objectPosition.y  ;
*//*        objectPosition.z = objectPosition.z * 2;
*/
        float rotasiYSumbuKamera = Camera.main.transform.rotation.eulerAngles.y;

        float rotasiYSumbuTerdekat = Mathf.Round(rotasiYSumbuKamera / 90f) * 90f;

        Vector3 rotasiObjekTerpilih = selectedObject.transform.rotation.eulerAngles;

        // Membuat rotasi target
        Quaternion rotasiTarget = Quaternion.Euler(rotasiObjekTerpilih.x, rotasiYSumbuTerdekat, rotasiObjekTerpilih.z);

        GameObject player = GameObject.Find("XR Origin (XR Rig)");


        objectPosition.y = player.transform.position.y + 1;

        GameObject instantiatedItem = Instantiate(selectedObject, objectPosition, rotasiTarget);



        if (instantiatedItem.GetComponent<Rigidbody>() != null)
        {
            instantiatedItem.GetComponent<Rigidbody>().isKinematic = true;

        }
  

        GameObject menuBarang = GameObject.Find("MenuItem_part1_fix(Clone)");


        if (menuBarang != null)
        {
            Destroy(menuBarang.gameObject);
        }
    }
}


