using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR;
using UnityEngine.XR;

namespace SojaExiles

{

    public class opencloseDoor : MonoBehaviour
	{
        InputDevice LeftController;

        public Animator openandclose;
		public bool open;
		public Transform Player;

		void Start()
		{
            List<InputDevice> deviceR = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, deviceR);

            if (deviceR.Count == 1)
            {
                LeftController = deviceR[0];
            }
            else if (deviceR.Count > 1)
            {
                Debug.Log("Left Controlelr Ada ");
            }
            else
            {
                Debug.Log("Controller Left Tidak ditemukan");
            }

            open = false;
		}

		void OnMouseOver()
		{
			{

                

                if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 15)
					{
						if (open == false)
						{
							if (Input.GetMouseButtonDown(0))
							{
								StartCoroutine(opening());
							}
						}
						else
						{
							if (open == true)
							{
								if (Input.GetMouseButtonDown(0))
								{
									StartCoroutine(closing());
								}
							}

						}

					}
				}
/*				if(devicesleft)
*/
			}

		}

		IEnumerator opening()
		{
			print("you are opening the door");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}