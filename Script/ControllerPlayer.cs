using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerPlayer : MonoBehaviour
{

    [SerializeField] private GameObject Left;
    [SerializeField] private GameObject Right;

    private XRRayInteractor Leftray;
    private XRRayInteractor Rightray;

    void Start()
    {
        Leftray = Left.GetComponent<XRRayInteractor>();
        Rightray = Right.GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Misalnya Anda ingin mengganti layer interaksi untuk sinar kiri
            Leftray.interactionLayerMask = Leftray.interactionLayerMask == 1 << 0
                ? 1 << 1 // Set ke layer lain, misalnya layer 1
                : 1 << 0; // Kembali ke layer 0
        }
    }
}
