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
    private static bool menuIsActive = false;
        

    
        void OnTriggerEnter(Collider other)
        {
            if (!_menu.activeSelf && !menuIsActive 
            && other.CompareTag("Player"))
            {
            _menu.SetActive(true);
                _Interaction.SetActive(true);

                menuIsActive = true;

        }
       else  if (_menu.activeSelf && menuIsActive && other.CompareTag("Player"))
            {

            _menu.SetActive(false);
                _Interaction.SetActive(false);
                menuIsActive = false;
           /* if (GameObject.Find("MenuItem_part1_fix(Clone)") != null) Destroy(GameObject.Find("MenuItem_part1_fix(Clone)"));
            if (GameObject.Find("Canvas 1(Clone)") != null) Destroy(GameObject.Find("Canvas 1(Clone)"));*/

        }
    }

    IEnumerator Jeda()
    {
        yield return new WaitForSeconds(1.0f);
    }

  


    void OnTriggerExit(Collider other)
    {
      
         



    }
    private void Awake()
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
