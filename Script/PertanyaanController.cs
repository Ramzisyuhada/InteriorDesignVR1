using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static UI_Interaction;
using DG.Tweening;

public class PertanyaanController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private InputActionProperty inputActionLeft;
    [SerializeField] private InputActionProperty inputActionRight;
    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;

    [Header("Canvas")]
    [SerializeField] private GameObject _Canvas_pertanyaan;



    private static GameObject currentPertanyaan_canvas;
    private static int Nilai;

    void Start()
    {
        GameObject player = GameObject.Find("XR Origin (XR Rig)");
        GameObject cameraobject = player.transform.GetChild(0).gameObject;
        if (rayLeft == null && rayRight == null)
        {
            rayLeft = cameraobject.transform.GetChild(1).GetComponent<XRRayInteractor>();
            rayRight = cameraobject.transform.GetChild(2).GetComponent<XRRayInteractor>();

        }
    }

    private void _InteractionHit()
    {
        RaycastHit hit;
        if (rayLeft.TryGetCurrent3DRaycastHit(out hit) && inputActionLeft.action.WasPressedThisFrame())
        {
            _objectRaycast(hit);
        }

        if (rayRight.TryGetCurrent3DRaycastHit(out hit) && inputActionRight.action.WasPressedThisFrame())
        {
            _objectRaycast(hit);

        }
    }
    void Update()
    {
        _InteractionHit();
    }

    private void _objectRaycast(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            /*                    if (barang != null) barang.GetComponent<Rigidbody>().isKinematic = true;
            */

            if (hitObject != null && hitObject.tag == "Pertanyaan")
            {
                ShowcanvasPertanyaan();
            }

            if (hitObject.GetComponent<Canvas>() != null)
            {
                return;
            }
        }
    }


    private void ShowcanvasPertanyaan()
    {
        if (currentPertanyaan_canvas == null)
        {
            currentPertanyaan_canvas = Instantiate(_Canvas_pertanyaan);
            Vector3 camera_posisition = Camera.main.transform.position;
            Quaternion camera_rotation = Camera.main.transform.rotation;
            Vector3 targetPosition = camera_posisition + camera_rotation * Vector3.forward * 2.5f;
            currentPertanyaan_canvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

            currentPertanyaan_canvas.transform.DORotate(camera_rotation.eulerAngles, 0.5f);

            currentPertanyaan_canvas.transform.DORestart();


        }
    }
}
