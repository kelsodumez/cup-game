using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class Hand : MonoBehaviour
{
    public ArmController.hand_target HandType
    {
        get => _hand;
    }

    [SerializeField] private ArmController.hand_target _hand;
    [SerializeField] private SpriteRenderer _handOpenSprite;
    [SerializeField] private SpriteRenderer _handClosedSprite;

    private Transform _target;
    private float _verticalOffset;
    [SerializeField] private Transform _root;
    private LerpableObject _handLerper;
    [SerializeField] private float _handLerpSpeed = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _handLerper = transform.GetComponent<LerpableObject>();
        // _root = transform.GetComponentInParent<Transform>();
        _verticalOffset = transform.position.y;

        ArmController.onHandTargetSet += FollowTarget;
        ArmController.onHandTargetCleared += ResetTarget;
        _handLerper.OnHitTarget += CloseHandSprite;
        _handLerper.OnLerpEnd += OpenHandSprite;
        ResetTarget(_hand);

        _handLerper.OnLerpEnd += HandStationary;
        _handLerper.OnHitTarget += ReachedTarget;
;
    }

    void OnDestroy()
    {
        ArmController.onHandTargetSet -= FollowTarget;
        ArmController.onHandTargetCleared -= ResetTarget;
        _handLerper.OnLerpEnd -= HandStationary;
        _handLerper.OnHitTarget -= CloseHandSprite;
        _handLerper.OnLerpEnd -= OpenHandSprite;
        _handLerper.OnHitTarget -= ReachedTarget;

    }

    // Update is called once per frame
    void Update()
    {
        if (_handLerper.IsLerping())
        {
            // Debug.Log($"{transform.name} lerping");
            return;
        }
        transform.position = new Vector3(_target.position.x, _target.position.y, _target.position.z);
    }

    private void FollowTarget(ArmController.hand_target inHand, Transform target)
    {
        if (inHand == ArmController.hand_target.both || _hand == inHand)
        {   
            // Debug.Log($"{transform.name} following {target.name}");
            ArmController.ToggleArmsStationary(false);
            _target = target;
            _handLerper.EndLerp();
            _handLerper.BeginLerpingToTransform(_target, _handLerpSpeed, false);
        }
    }

    private void ResetTarget(ArmController.hand_target inHand)
    {
        if (inHand == ArmController.hand_target.both || _hand == inHand)
        {
            _target = _root;
            _handLerper.EndLerp();
            _handLerper.BeginLerpingToTransform(_target, _handLerpSpeed * 2f);
        }
    }

    private void ReachedTarget()
    {
        ArmController.ToggleArmsAtTargets(true);
    }

    private void CloseHandSprite()
    {
        Debug.Log("Closing Hand!");
        _handOpenSprite.enabled = false;
        _handClosedSprite.enabled = true;
    }
    private void OpenHandSprite()
    {
        _handOpenSprite.enabled = true;
        _handClosedSprite.enabled = false;
    }

    public bool IsLerping()
    {
        return _handLerper.IsLerping();
    }

    private void HandStationary()
    {
        ArmController.ToggleArmsStationary(true);
    }

    
}
