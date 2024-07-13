using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class V_PPT : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] GameObject next;
    [SerializeField] GameObject prev;
    [SerializeField] M_PPT m_PPT;

    [Header("Controller")]
    [SerializeField] private InputActionProperty inputActionLeft;
    [SerializeField] private InputActionProperty inputActionRight;
    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;
    private VM_Ppt ppt;

    private static int index = 0;
    void Start()
    {
        ppt = new VM_Ppt(m_PPT);

        transform.GetChild(0).GetComponent<MeshRenderer>().material = m_PPT.Material[index].materials;
        GameObject player = GameObject.Find("XR Origin (XR Rig)");
        GameObject cameraobject = player.transform.GetChild(0).gameObject;

        if (rayLeft == null && rayRight == null)
        {
            rayLeft = cameraobject.transform.GetChild(1).GetComponent<XRRayInteractor>();
            rayRight = cameraobject.transform.GetChild(2).GetComponent<XRRayInteractor>();

        }
    }

    private void Update()
    {
        Click();
        
    }
    private void _objectRaycast(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Prev"))
        {
            _prev();
        }
        if (hit.transform.CompareTag("Next"))
        {
            _next();
        }
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
    public void _next()
    {
        index++;
        var values = ppt.Next(index);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = values.Value.Item1;
        index = values.Value.Item2;

    }


    public void _prev()
    {
        index--;
        var values = ppt.Prev(index);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = values.Value.Item1;
        index = values.Value.Item2;
    }

}
