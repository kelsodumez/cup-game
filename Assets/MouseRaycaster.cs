using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField] private InputAction _clickAction;
    void Awake()
    {
        _clickAction.performed += ctx => OnPointerClick();
        _clickAction.Enable();
    }
    void OnPointerClick()
    {
        Vector2 clickLocation = Mouse.current.position.ReadValue();        
        clickLocation.x /= Screen.width;
        clickLocation.y /= Screen.height;
        Ray ray = Camera.main.ViewportPointToRay(clickLocation);
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
}
