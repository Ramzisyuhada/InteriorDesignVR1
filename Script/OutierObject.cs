using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class OutierObject : MonoBehaviour
{
    private Transform highlightLeft;
    private Transform highlightRight;
    private Transform selectionLeft;
    private Transform selectionRight;
    private RaycastHit raycastHit;

    private Color originalOutlineColorLeft;
    private Color originalOutlineColorRight;

    [SerializeField] private XRRayInteractor rayLeft;
    [SerializeField] private XRRayInteractor rayRight;
    [SerializeField] private InputActionProperty inputActionSelectLeft;
    [SerializeField] private InputActionProperty inputActionSelectRight;
    private void Awake()
    {
        GameObject player = GameObject.Find("XR Origin (XR Rig)");
        if (player != null)
        {
            GameObject cameraObject = player.transform.GetChild(0).gameObject;
            if (rayLeft == null)
            {
                rayLeft = cameraObject.transform.GetChild(1).GetComponent<XRRayInteractor>();
            }
            if (rayRight == null)
            {
                rayRight = cameraObject.transform.GetChild(2).GetComponent<XRRayInteractor>();
            }
        }
    }

    private void Update()
    {
        ResetHighlight(ref highlightLeft, ref originalOutlineColorLeft, selectionLeft);
        ResetHighlight(ref highlightRight, ref originalOutlineColorRight, selectionRight);

        if (rayLeft.TryGetCurrent3DRaycastHit(out raycastHit))
        {
            HandleRaycastHit(raycastHit, ref highlightLeft, selectionLeft, ref originalOutlineColorLeft);
        }

        if (rayRight.TryGetCurrent3DRaycastHit(out raycastHit))
        {
            HandleRaycastHit(raycastHit, ref highlightRight, selectionRight, ref originalOutlineColorRight);
        }

        HandleInputActions();
    }

    private void ResetHighlight(ref Transform highlight, ref Color originalColor, Transform selection)
    {
        if (highlight != null && highlight != selection)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.OutlineColor = originalColor;
                outline.enabled = false;
            }
            highlight = null;
        }
    }

    private void HandleInputActions()
    {
        if (inputActionSelectLeft.action.WasPressedThisFrame())
        {
            HandleSelection(ref highlightLeft, ref selectionLeft, ref originalOutlineColorLeft);
        }

        if (inputActionSelectRight.action.WasPressedThisFrame())
        {
            HandleSelection(ref highlightRight, ref selectionRight, ref originalOutlineColorRight);
        }
    }

    private void HandleRaycastHit(RaycastHit hit, ref Transform highlight, Transform selection, ref Color originalColor)
    {
        if (IsPointerOverUIElement())
            return;

        highlight = hit.transform;

        if (IsValidHighlight(highlight) && highlight != selection)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline == null)
            {
                outline = highlight.gameObject.AddComponent<Outline>();
                originalColor = outline.OutlineColor;
            }
            else if (outline.enabled == false) // only update original color if the outline is disabled
            {
                originalColor = outline.OutlineColor;
            }
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 7.0f;
            outline.enabled = true;
        }
        else
        {
            highlight = null;
        }
    }

    private bool IsPointerOverUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

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

    private void HandleSelection(ref Transform highlight, ref Transform selection, ref Color originalColor)
    {
        if (highlight != null)
        {
            if (selection != null)
            {
                Outline outline = selection.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.OutlineColor = originalColor;
                    outline.enabled = false;
                }
            }
            selection = highlight;
            Outline selectionOutline = selection.gameObject.GetComponent<Outline>();
            if (selectionOutline != null)
            {
                originalColor = selectionOutline.OutlineColor;
                selectionOutline.OutlineColor = Color.green;
                selectionOutline.enabled = true;
            }
            highlight = null;
        }
        else if (selection != null)
        {
            Outline outline = selection.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.OutlineColor = originalColor;
                outline.enabled = false;
            }
            selection = null;
        }
    }

    private bool IsValidHighlight(Transform transform)
    {
        return transform.CompareTag("Surface") || transform.CompareTag("Decoration") ||
               transform.CompareTag("Wall") || transform.CompareTag("Wall Decoration") ||
               transform.CompareTag("Furniture") || transform.CompareTag("Door") ||
               transform.CompareTag("Ceiling") || transform.CompareTag("Pertanyaan");
    }
}
