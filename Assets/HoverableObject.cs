using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverableObject : MonoBehaviour//, IPointerClickHandler
{
    [SerializeField] private UnityEvent onHoverEvent;

    public void Initialize(System.Action methodName)
    {
        // onObjectClick = methodName;
    }

    // public void OnMouseDown()
    // {
    //     // onObjectClick();   
    // }

    // for no render tex bu
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     onObjectClick();   
    // }

    public void OnMouseOver()
    {
        onHoverEvent?.Invoke();
    }
}
