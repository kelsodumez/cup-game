using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverableObject : MonoBehaviour//, IPointerClickHandler
{
    private bool _allowHover = true;
    [SerializeField] private List<UnityEvent> _onHoverEvents;

    public void OnMouseOver()
    {
        if (!_allowHover)
        {
            return;
        }

        foreach (UnityEvent onHoverEvent in _onHoverEvents)
        {
            onHoverEvent?.Invoke();
        }
    }

    public void AllowHovering(bool inAllowHover)
    {
        _allowHover = inAllowHover;
    }
}