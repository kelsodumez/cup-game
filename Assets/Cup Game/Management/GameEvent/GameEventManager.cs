    using UnityEngine;
    using System;
    static public class GameEventManager
    {
        static public event Action<GameEvent> OnEventBegin;
        static public event Action<GameEvent> OnEventEnd;
        static public bool _roundWon;
        
        public static void StartGame(GameEvent inGameEvent)
        {
            OnEventBegin?.Invoke(inGameEvent);
            EventTimerManager.CreateNewTimer(inGameEvent.scrambleDuration, () => GuessPhase(inGameEvent), true, $"{inGameEvent.eventName} TIMER");
        }

        static void GuessPhase(GameEvent inGameEvent)
        {
            EventTimerManager.CreateNewTimer(inGameEvent.guessDuration, () => EndEvent(inGameEvent), true, $"{inGameEvent.eventName} TIMER");
        }


        public static void EndEvent(GameEvent inGameEvent)
        {
            _roundWon = CupManager.retrieveGuess();

            OnEventEnd?.Invoke(inGameEvent);
        }
    }