using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour//, IPointerClickHandler
{
    [SerializeField] private System.Action onObjectClick;

    public void Initialize(System.Action methodName)
    {
        onObjectClick = methodName;
    }
    
    public void OnMouseDown()
    {
        onObjectClick();   
    }

// for no render tex bu
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     onObjectClick();   
    // }
}
