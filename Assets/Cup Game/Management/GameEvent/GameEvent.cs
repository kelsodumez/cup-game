using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    [Header("Game Parameters")]
    public string eventName;
    [Range(0,5)] public int cupAmount = 2;
    [Range(0,5)] public int stoneAmount = 1;
    [Range(0,20f)] public float scrambleDuration = 20f;

    [Range(0,20f)] public float guessDuration = 20f;
}