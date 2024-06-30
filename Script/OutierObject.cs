using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class OutierObject : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;
    [SerializeField] private InputActionProperty inputActionSelectLeft;
    [SerializeField] private InputActionProperty inputActionSelectRight;

    void Update()
    {
        if (highlight != null)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            highlight = null;
        }

        if (rayLeft.TryGetCurrent3DRaycastHit(out raycastHit))
        {
            HandleRaycastHit(raycastHit);
        }

        else if (rayRight.TryGetCurrent3DRaycastHit(out raycastHit))
        {
            HandleRaycastHit(raycastHit);
        }

        if (inputActionSelectLeft.action.WasPressedThisFrame())
        {
            HandleSelection(rayLeft);
        }

         if (inputActionSelectRight.action.WasPressedThisFrame())
        {
            HandleSelection(rayRight);
        }
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        if (IsPointerOverUIElement())
            return;

        highlight = hit.transform;
        if ((highlight.CompareTag("Surface") || highlight.CompareTag("Decoration") || highlight.CompareTag("Wall") || highlight.CompareTag("Wall Decoration") || highlight.CompareTag("Furniture") || highlight.CompareTag("Door")) && highlight  != selection)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline == null)
            {
                outline = highlight.gameObject.AddComponent<Outline>();
                outline.OutlineColor = Color.magenta;
                outline.OutlineWidth = 7.0f;
            }
            outline.enabled = true;
        }
        else
        {
            highlight = null;
        }
    }

    private bool IsPointerOverUIElement()
    {
        // Check using EventSystem
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        // Additional check using GraphicRaycaster
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Screen.width / 2, Screen.height / 2) // Assuming center of screen for XR
        };
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = FindObjectOfType<GraphicRaycaster>();
        if (raycaster != null)
        {
            raycaster.Raycast(eventData, results);
            return results.Count > 0;
        }
        return false;
    }

    private void HandleSelection(XRRayInteractor rayInteractor)
    {
        if (highlight != null)
        {
            if (selection != null)
            {
                Outline outline = selection.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
            selection = highlight;
            Outline selectionOutline = selection.gameObject.GetComponent<Outline>();
            if (selectionOutline != null)
            {
                selectionOutline.enabled = true;
            }
            highlight = null;
        }
        else
        {
            if (selection != null)
            {
                Outline outline = selection.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
                selection = null;
            }
        }
    }
}
