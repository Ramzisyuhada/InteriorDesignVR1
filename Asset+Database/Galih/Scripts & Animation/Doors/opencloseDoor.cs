using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR;
using UnityEngine.XR;


    public class opencloseDoor : MonoBehaviour
	{
        [SerializeField] private InputActionProperty inputActionLeft;
        [SerializeField] private InputActionProperty inputActionRight;
        public Animator openandclose;
		public bool open;
		public Transform Player;

    private void Start()
    {
        open = false;

    }

    private void Update()
        {
			OnMouseOver1();
        }
        void OnMouseOver1()
		{
			{
                float leftValue = inputActionLeft.action.ReadValue<float>();
                float rightValue = inputActionRight.action.ReadValue<float>();
                bool inputActive = leftValue > 0.5f || rightValue > 0.5f;


                if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
				Debug.Log("Jarak : " + dist);
					if (dist < 5)
					{
						if (open == false)
						{
							if (inputActive)
							{
								StartCoroutine(opening());
							}
						}
						else
						{
							if (open == true)
							{
								if (inputActive)
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
