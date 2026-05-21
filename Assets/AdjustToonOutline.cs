using Unity.VisualScripting;
using UnityEngine;

public class AdjustToonOutline : MonoBehaviour
{
    [SerializeField] private Color _newOutline;
    private Color _originalOutline;
    private MeshRenderer _renderer;

    void Awake()
    {
        _renderer = transform.GetComponentInChildren<MeshRenderer>();
        _originalOutline = _renderer.material.GetColor("_Outline_Color");
    }

    public void SetOutlineToSaved()
    {
        Debug.Log("doing");
        _renderer.material.SetColor("_Outline_Color", _newOutline);
    }
    
}
