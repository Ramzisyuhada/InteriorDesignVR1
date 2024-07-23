using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        GameObject player1 = GameObject.Find("XR Origin (XR Rig)");
        GameObject cameraobject = player1.transform.GetChild(0).gameObject;

        if (rayLeft == null && rayRight == null)
        {
            rayLeft = cameraobject.transform.GetChild(1).GetComponent<XRRayInteractor>();
            rayRight = cameraobject.transform.GetChild(2).GetComponent<XRRayInteractor>();

        }
    }

    void Update()
    {
        Click();

    }
    void Click()
    {
        RaycastHit hit;
        if (rayLeft.TryGetCurrent3DRaycastHit(out hit) && inputActionLeft.action.WasPressedThisFrame())
        {
            if(hit.transform.GetComponentInChildren<VideoPlayer>() != null && hit.transform.CompareTag("Video"))
            _objectRaycast(hit);
        }

        if (rayRight.TryGetCurrent3DRaycastHit(out hit) && inputActionRight.action.WasPressedThisFrame())
        {
            if (hit.transform.GetComponentInChildren<VideoPlayer>()  != null && hit.transform.CompareTag("Video"))

                _objectRaycast(hit);
                
        }
    }


    private void _objectRaycast(RaycastHit hit)
    {
        
            Play(hit);
        
     
    }

    bool isPlayingVid = false;
    private void Play(RaycastHit hit)
    {
        isPlayingVid = !isPlayingVid;

        if (isPlayingVid  )
        {
            player = hit.transform.GetComponentInChildren<VideoPlayer>();

            player.Play();
        }
        else
        {
            player = hit.transform.GetComponentInChildren<VideoPlayer>();

            player.Stop();

        }
    }
}