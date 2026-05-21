using UnityEngine;
using System;
using Unity.VisualScripting;
static public class GameEventManager
    {
        static public event Action<GameEvent> OnEventBegin;
        static public event Action<GameEvent> OnScrambleBegin;
        static public event Action GuessingBegin;
        static public event Action<GameEvent, bool> OnEventEnd;        
        public static void StartGame(GameEvent inGameEvent)
        {
            OnEventBegin?.Invoke(inGameEvent);
            Debug.Log("Game Event: Initiatilised");
        }

        public static void ScramblePhase(GameEvent inGameEvent)
        {
            Debug.Log("Scramble!");
            OnScrambleBegin?.Invoke(inGameEvent);
            // EventTimerManager.CreateNewTimer(inGameEvent.scrambleDuration, () => GuessPhase(inGameEvent), true, $"{inGameEvent.eventName} TIMER", false);
        }

        public static void GuessPhase(GameEvent inGameEvent)
        {
            ArmController.ResetHand(ArmController.hand_target.both);
            Debug.Log("Game Event: Guessing Phase");
            GuessingBegin?.Invoke();
            EventTimerManager.CreateNewTimer(inGameEvent.guessDuration, () => EndEvent(inGameEvent, false), true, $"{inGameEvent.eventName} TIMER");
        }

        public static void EndEvent(GameEvent inGameEvent, bool roundWon)
        {
            Debug.Log("Game Event: Wrapup");
            EventTimerManager.ClearTimers();
            OnEventEnd?.Invoke(inGameEvent, roundWon);
        }
    }