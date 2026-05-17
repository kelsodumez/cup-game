using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
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
}
