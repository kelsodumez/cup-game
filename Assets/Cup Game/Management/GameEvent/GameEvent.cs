using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    [Header("Game Parameters")]
    public string eventName;
    [Range(0,20f)] public float displayDuration = 5f;
    [Range(0,20f)] public float scrambleDuration = 5f;
    public int noShuffles = 2;
    [Range(0,20f)] public float guessDuration = 20f;

    [Range(2,10)] public int cupAmount = 3;
    [Range(0,200f)] public float dealSpeed = 40f;

    [Range(0,200f)] public float cupMoveSpeed = 20f;

}