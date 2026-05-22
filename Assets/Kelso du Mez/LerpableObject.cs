using UnityEngine;
using System;

public class LerpableObject : MonoBehaviour
{
    private Transform _transformLerpTarget;
    private Vector3 _vectorLerpTarget;

    private Vector3 _startPos;
    private float _startTime;
    private float _lerpDist;
    [SerializeField] private bool _doVectorLerp = false;
    [SerializeField] private bool _doTransformLerp = false;



    private float _lerpSpeed;

    private bool _doResetLerp = true;

    public event Action OnLerpEnd;

    public event Action OnHitTarget;
    private bool _onHitTargetInvoked = false;

    void Awake()
    {
    }

    void Update()
    {
        if (_doTransformLerp)
        {
            DoLerp(_transformLerpTarget.position);
        }
        else if (_doVectorLerp)
        {
            DoLerp(_vectorLerpTarget);
        }
    }

    public void BeginLerpingToVector(Vector3 target, float lerpSpeed, bool endLerp = true)
    {
        _onHitTargetInvoked = false;
        _doTransformLerp = false;
        _doVectorLerp = true;
        _startTime = Time.time;
        _startPos = transform.position;
        _vectorLerpTarget = target;
        _lerpSpeed = lerpSpeed;

        _doResetLerp = endLerp;
    }

    public void BeginLerpingToTransform(Transform target, float lerpSpeed, bool endLerp = true)
    {
        _onHitTargetInvoked = false;
        _doVectorLerp = false;
        _doTransformLerp = true;
        _startTime = Time.time;
        _startPos = transform.position;
        _transformLerpTarget = target;
        _lerpSpeed = lerpSpeed;

        _doResetLerp = endLerp;
    }

    private void DoLerp(Vector3 lerpTarget)
    {
        _lerpDist = Vector3.Distance(_startPos, lerpTarget);

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - _startTime) * _lerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / _lerpDist;

        // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(_startPos, lerpTarget, fractionOfJourney);

        if ((transform.position - lerpTarget).sqrMagnitude <= 0.0002f)
        {
            if (!_onHitTargetInvoked)
            {
                _onHitTargetInvoked = true;
                OnHitTarget?.Invoke();
            }

            if (_doResetLerp)
            {
                EndLerp();
            }
        }
    }

    public bool IsLerping()
    {
        // returns if currently lerping
        return _doVectorLerp || _doTransformLerp;
    }

    public void EndLerp()
    {
        _doResetLerp = false;
        OnLerpEnd?.Invoke();
        ResetLerper();
    }

    private void ResetLerper()
    {   
        _doTransformLerp = false;
        _doVectorLerp = false;
    }
}
