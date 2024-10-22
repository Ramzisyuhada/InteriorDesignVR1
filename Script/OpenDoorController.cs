using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenDoorController : MonoBehaviour
{
    [SerializeField] GameObject _door;
    [SerializeField] GameObject _door1;

    private Animator openandclose;
    private Animator openandclose1;

    private void Awake()
    {
        openandclose = _door.GetComponent<Animator>();
        openandclose1 = _door1.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(OpenDoor());

    }
   
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(CloseDoor());

    }

    private IEnumerator OpenDoor()
    {
        openandclose.Play("Opening 1");
        openandclose1.Play("Opening");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator CloseDoor()
    {
        Debug.Log("You are closing the door");
        openandclose.Play("Closing 1");
        openandclose1.Play("Closing");

        yield return new WaitForSeconds(0.5f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
