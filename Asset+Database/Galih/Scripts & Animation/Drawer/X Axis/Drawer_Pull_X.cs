using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace SojaExiles

{

	public class Drawer_Pull_X : MonoBehaviour
	{
        private RaycastHit raycastHit;

        [SerializeField] private XRRayInteractor rayLeft;
        [SerializeField] private XRRayInteractor rayRight;
        [SerializeField] private InputActionProperty inputActionLeft;
        [SerializeField] private InputActionProperty inputActionRight;
        public Animator pull_01;
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
                
                    HandleDoorInteraction(raycastHit.transform.gameObject);
                
            }
        }

        private void HandleDoorInteraction(GameObject target)
        {
            if (Player != null)
            {
              
                    pull_01 = target.GetComponentInChildren<Animator>();
                
                float leftInputValue = inputActionLeft.action.ReadValue<float>();
                float rightInputValue = inputActionRight.action.ReadValue<float>();
                bool inputActive = leftInputValue > 0.5f || rightInputValue > 0.5f;
                float distance = Vector3.Distance(Player.position, target.transform.position);
                Debug.Log("Distance: " + distance);

                if (distance < 15)
                {
                    if (!open && inputActive && pull_01 != null)
                    {
                        StartCoroutine(opening());
                    }
                    else if (open && inputActive && pull_01 != null)
                    {
                        StartCoroutine(closing());
                    }
                }
            }
        }

        IEnumerator opening()
		{
			print("you are opening the door");
			pull_01.Play("openpull_01");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			pull_01.Play("closepush_01");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}