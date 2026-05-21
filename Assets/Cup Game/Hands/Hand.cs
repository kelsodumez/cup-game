using UnityEngine;
using System;

public class Hand : MonoBehaviour
{
    [SerializeField] private ArmController.hand_target _hand;
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

        ResetTarget(_hand);

        _handLerper.OnLerpEnd += HandStationary;
    }

    void OnDestroy()
    {
        ArmController.onHandTargetSet -= FollowTarget;
        ArmController.onHandTargetCleared -= ResetTarget;
        _handLerper.OnLerpEnd -= HandStationary;

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
            ArmController.ToggleArmsStationary(false);
            // Debug.Log($"{transform.name} following {target.name}");
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

    public bool IsLerping()
    {
        return _handLerper.IsLerping();
    }

    private void HandStationary()
    {
        ArmController.ToggleArmsStationary(true);
    }
}
