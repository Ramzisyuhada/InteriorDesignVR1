using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

public class CheckColliderOpendor : MonoBehaviour
{
    private GameObject _controller;

    private GameObject _menu;
    private GameObject _Interaction;
    
     void OnTriggerEnter(Collider other)
    {
        if (_menu.active == false)
        {


           _menu.SetActive(true);
           _Interaction.SetActive(true);


        }
       /* if (_menu.active == true)
        {
            _menu.SetActive(false);
            _Interaction.SetActive(false);

        }*/


    }

     void OnTriggerExit(Collider other)
    {
      




    }
    void Start()
    {
        _controller = GameObject.Find("Controller");
        _menu = _controller.transform.GetChild(0).gameObject;
        _Interaction = _controller.transform.GetChild(1).gameObject;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
