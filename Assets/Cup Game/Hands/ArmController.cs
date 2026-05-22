using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.Linq;

public class ArmController : MonoBehaviour
{

    public enum hand_target
    {
        left,
        right,
        both
    }
    static public event Action<hand_target, Transform> onHandTargetSet;
    static public event Action<hand_target> onHandTargetCleared;

    private static bool _armsStationary = true;
    private static bool _armsAtTargets = true;


    [SerializeField] private List<Hand> _hands;

    public static void ToggleArmsStationary(bool inState)
    {
        _armsStationary = inState;
    }

    public static bool ArmsStationary()
    {
        return _armsStationary;
    }

    public static void ToggleArmsAtTargets(bool inState)
    {
        _armsAtTargets = inState;
    }
    public static bool ArmsAtTargets()
    {
        return _armsAtTargets;
    }

    public static void SetTarget(hand_target inHand, Transform inTarget)
    {
        onHandTargetSet?.Invoke(inHand, inTarget);
    }

    public static void ResetHand(hand_target inHand)
    {
        onHandTargetCleared?.Invoke(inHand);
    }
}
