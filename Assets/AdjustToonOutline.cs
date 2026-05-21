using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;

public class AdjustToonOutline : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Color _newOutline;
    private Color _originalOutline;
    private bool _doWobble = false;
    [SerializeField] private float _wobbleAmplitude = 2f;
    [SerializeField] private float _wobbleFrequency = 2f;
    [SerializeField] private float _minimumOutlineWidth = 30f;
    private float _maximumOutlineWidth;


    void Awake()
    {
        _originalOutline = _renderer.material.GetColor("_Outline_Color");
        _maximumOutlineWidth = _renderer.material.GetFloat("_Outline_Width");
        CupMeshManager.OnOutlineAltered += ResetOutlineColour;
    }

    void Update()
    {
        if (_doWobble)
        {
            OutlineSinWobble();
        }
    }

    void OnDestroy()
    {
        CupMeshManager.OnOutlineAltered -= ResetOutlineColour;
    }

    public void ResetOutlineColour(MeshRenderer rendererIn)
    {
        if (_renderer != rendererIn)
        {
            _renderer.material.SetColor("_Outline_Color", _originalOutline);
            _doWobble = false;
        }
    }

    public void SetOutlineColour()
    {
        _renderer.material.SetColor("_Outline_Color", _newOutline);
        CupMeshManager.OutlineAltered(_renderer);
    }

    public void ToggleOutlineWobble(bool inWobbleState)
    {
        _doWobble = inWobbleState;
    }

    private void OutlineSinWobble()
    {
        float outlineWidth = Mathf.Sin(Time.time * _wobbleFrequency) * _wobbleAmplitude;
        outlineWidth += _minimumOutlineWidth;
        _renderer.material.SetFloat("_Outline_Width", outlineWidth);
    }
}
