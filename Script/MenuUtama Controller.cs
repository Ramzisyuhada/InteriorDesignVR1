using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UI_Interaction;

public class MenuUtamaController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private InputActionProperty InputActionLeft;
    [SerializeField] private InputActionProperty InputActionRight;


    [Header("Canvas")]
    [SerializeField] private GameObject Canvas_Menu;

    public void detroyCanvas()
    {
        if (GameObject.Find("Menu(Clone)") != null) Destroy(GameObject.Find("Menu(Clone)"));

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (InputActionLeft.action.WasPressedThisFrame()) {

            ShowCanvas();
        }

        if (InputActionRight.action.WasPressedThisFrame())
        {
            ShowCanvas();

        }
    }


    void ShowCanvas()
    {
           if (GameObject.Find("Menu(Clone)") == null)
            {
                GameObject currentCanvas = Instantiate(Canvas_Menu);
                UI_Interaction ui = new UI_Interaction();
                Controller currentController = UI_Interaction._currentController;

                currentController = Controller.Default;
                ui.setController(currentController);
                GameObject gameObject = GameObject.Find("XR Origin (XR Rig)");
                gameObject.transform.Find("Locomotion System").gameObject.SetActive(true);
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 4.5f;
                currentCanvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y , targetPosition.z), 1f);

                currentCanvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

                currentCanvas.transform.DORestart();

                DOTween.Play(currentCanvas);

            }
            else
            {
                Vector3 cameraPosition = Camera.main.transform.position;
                Quaternion cameraRotation = Camera.main.transform.rotation;

                Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 4.5f;
                GameObject.Find("Menu(Clone)").transform.DOMove(new Vector3(targetPosition.x, targetPosition.y , targetPosition.z), 1f);

                GameObject.Find("Menu(Clone)").transform.DORotate(cameraRotation.eulerAngles, 1f);

                GameObject.Find("Menu(Clone)").transform.DORestart();

                DOTween.Play(GameObject.Find("Menu(Clone)"));
            }

        if (GameObject.Find("MenuItem_part1_fix(Clone)") != null) Destroy(GameObject.Find("MenuItem_part1_fix(Clone)"));
        if (GameObject.Find("How To(Clone)") != null) Destroy(GameObject.Find("How To(Clone)"));
        if (GameObject.Find("Pertanyaan(Clone)") != null) Destroy(GameObject.Find("Pertanyaan(Clone)"));
        if (GameObject.Find("Canvas 1(Clone)") != null) Destroy(GameObject.Find("Canvas 1(Clone)"));

       
    }
}
