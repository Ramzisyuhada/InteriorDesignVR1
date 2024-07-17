using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class Controllervideo : MonoBehaviour
{

    private GameObject tv;


    [Header("Controller")]
    [SerializeField] private InputActionProperty inputActionLeft;
    [SerializeField] private InputActionProperty inputActionRight;
    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;

    private VideoPlayer player;
    void Start()
    {
        player = GetComponentInChildren<VideoPlayer>();
        GameObject player1 = GameObject.Find("XR Origin (XR Rig)");
        GameObject cameraobject = player1.transform.GetChild(0).gameObject;

        if (rayLeft == null && rayRight == null)
        {
            rayLeft = cameraobject.transform.GetChild(1).GetComponent<XRRayInteractor>();
            rayRight = cameraobject.transform.GetChild(2).GetComponent<XRRayInteractor>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        Click();

    }
    void Click()
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


    private void _objectRaycast(RaycastHit hit)
    {
        if (hit.transform.gameObject.name == "TV_Apt_01")
        {
            Play();
        }
     
    }

    bool isPlayingVid = false;
    private void Play()
    {
        isPlayingVid = !isPlayingVid;

        if (isPlayingVid)
        {

            player.Play();
        }
        else
        {
            player.Stop();

        }
    }
}