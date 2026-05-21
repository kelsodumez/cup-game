using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField] private InputAction _clickAction;
    [SerializeField] private InputAction _mouseMoveAction;

    void Awake()
    {
        _clickAction.performed += ctx => OnPointerClick();
        _clickAction.Enable();

        _mouseMoveAction.performed += ctx => MouseMoveEvent();
        _mouseMoveAction.Enable();
    }

    private Ray MouseToViewPortRelativeToScreen()
    {
        Vector2 clickLocation = Mouse.current.position.ReadValue();        
        clickLocation.x /= Screen.width;
        clickLocation.y /= Screen.height;
        Ray ray = Camera.main.ViewportPointToRay(clickLocation);
        return ray;
    }

    void OnPointerClick()
    {
        Ray ray = MouseToViewPortRelativeToScreen();
        // Debug.Log(clickLocation);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            try
            {
                hit.transform.GetComponent<ClickableObject>().OnMouseDown();
            }
            catch
            {
                Debug.LogWarning($"Object {transform.name} lacks class ClickableObject");
            }
        }
        else
        {
            Debug.Log("No hit object");
        }
    }

    // this is a really inefficient way to check if mouse hovering over an object, but it does work!
    void MouseMoveEvent()//InputAction.CallbackContext context)
    {
        Ray ray = MouseToViewPortRelativeToScreen();
        // Debug.Log(clickLocation);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            try
            {
                hit.transform.GetComponent<HoverableObject>().OnMouseOver();
            }
            catch
            {
                Debug.LogWarning($"Object {transform.name} lacks class HoverableObject");
            }
        }
        // else
        // {
        //     Debug.Log("No hit object");
        // }
    }
}
