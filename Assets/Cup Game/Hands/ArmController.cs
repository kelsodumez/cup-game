using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.Linq;

public class ArmController : MonoBehaviour
{
    private static ArmController _instance;

    public static ArmController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ArmController();
            }
            return _instance;
        }
    }

    public enum hand_target
    {
        left,
        right,
        both
    }
    static public event Action<hand_target, Transform> onHandTargetSet;
    static public event Action<hand_target> onHandTargetCleared;

    private Hand[] _hands;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _hands = GetComponents<Hand>();
        Debug.Log($"{_hands.Count()} hands in scene");
    }


    public static void SetTarget(hand_target inHand, Transform inTarget)
    {
        onHandTargetSet?.Invoke(inHand, inTarget);
    }

    public static void ResetHand(hand_target inHand)
    {
        onHandTargetCleared?.Invoke(inHand);
    }

    public bool HandsStationary()
    {
        foreach (Hand hand in _hands)
        {
            if (hand.IsLerping())
            {
                return false;
            }
        }
        return true;
    }
}
