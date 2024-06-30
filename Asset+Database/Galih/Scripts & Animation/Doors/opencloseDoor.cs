using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


    public class opencloseDoor : MonoBehaviour
	{
    private RaycastHit raycastHit;

    [SerializeField] private XRRayInteractor rayLeft;
		[SerializeField] private XRRayInteractor rayRight;
		[SerializeField] private InputActionProperty inputActionLeft;
        [SerializeField] private InputActionProperty inputActionRight;
    [SerializeField] private LayerMask doorLayer; 

    public Animator openandclose;
		public bool open;
		public Transform Player;

    private void Start()
    {
        open = false;

    }

    private void Update()
    {
        CheckForDoorInteraction();
    }

    private void CheckForDoorInteraction()
    {
        if (rayLeft.TryGetCurrent3DRaycastHit(out raycastHit) || rayRight.TryGetCurrent3DRaycastHit(out raycastHit))
        {
            if (raycastHit.transform.CompareTag("Door"))
            {
                HandleDoorInteraction(raycastHit.transform.gameObject);
            }
        }
    }

    private void HandleDoorInteraction(GameObject target)
    {
        if (Player != null)
        {
            if (openandclose != null)
            {
                openandclose = target.GetComponent<Animator>();
            }
            float leftInputValue = inputActionLeft.action.ReadValue<float>();
            float rightInputValue = inputActionRight.action.ReadValue<float>();
            bool inputActive = leftInputValue > 0.5f || rightInputValue > 0.5f;
            float distance = Vector3.Distance(Player.position, target.transform.position);
            Debug.Log("Distance: " + distance);

            if (distance < 15)
            {
                if (!open && inputActive && openandclose != null)
                {
                    StartCoroutine(OpenDoor());
                }
                else if (open && inputActive && openandclose != null)
                {
                    StartCoroutine(CloseDoor());
                }
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        openandclose.Play("Opening");
        open = true;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator CloseDoor()
    {
        Debug.Log("You are closing the door");
        openandclose.Play("Closing");
        open = false;
        yield return new WaitForSeconds(0.5f);
    }


}
