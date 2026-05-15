// using UnityEngine;
// using UnityEngine.UIElements;

// /// <summary>
// /// The Round Manager is responsible for iterating through the Gather, Rebuild, Scramble, and Survive phases.
// /// </summary>
// public class RoundManager : MonoBehaviour
// {
//     [Header("Gather Period Parameters")]
//     [SerializeField][Min(0)] private float _gatherTime = 20f;
//     [Header("Rebuild Period Parameters")]
//     [SerializeField][Min(0)] private float _rebuildTime = 20f;
//     [Header("Scramble Period Parameters")]
//     [SerializeField][Min(0)] private float _scrambleTime = 20f;
//     [Header("Survive Period Parameters")]
//     // timer for weather events is determined by the WeatherEvent object.
//     [SerializeField] private WeatherEvent[] _weatherEvents;
//     private WeatherEvent _currentWeatherEvent;

//     private static RoundManager _Instance;

//     enum roundType
//     {
//         gather, // period for players to gather materials
//         rebuild, // period for players to rebuild structure
//         scramble, // weather event is announced
//         survive // weather event occurs
//     }

//     private roundType currentRound = roundType.gather;

//     void Awake()
//     {
//         _Instance = this;
//         WeatherManager.OnEventEnd += GoNextRound;        
//     }

//     void Start()
//     {
//         EnactCurrentRound();
//     }

//     void OnDestroy()
//     {
//         WeatherManager.OnEventEnd -= GoNextRound;
//     }

//     private void GoNextRound(WeatherEvent previousWeatherEvent = null)
//     {
//         switch (currentRound)
//         {
//             case roundType.gather:
//                 currentRound = roundType.rebuild;
//                 break;
//             case roundType.rebuild:
//                 currentRound = roundType.scramble;
//                 break;
//             case roundType.scramble:
//                 currentRound = roundType.survive;
//                 break;
//             case roundType.survive:
//                 currentRound = roundType.gather;
//                 break;
//         }
//         EnactCurrentRound();
//     }
//     private void EnactCurrentRound()
//     {
//         Debug.Log($"Current Round: {currentRound}");
//         switch (currentRound)
//         {
//             case roundType.gather:
//                 try
//                 {
//                     DebrisSpawner.Instance.ScatterDebris();
//                 }
//                 catch
//                 {
//                     Debug.LogWarning("Failed to Scatter Debris");
//                 }

//                 EventTimerManager.CreateNewTimer(_gatherTime, () => GoNextRound(), true, "_GATHER ROUND TIMER");
//                 break;
//             case roundType.rebuild:
//                 EventTimerManager.CreateNewTimer(_rebuildTime, () => GoNextRound(), true, "_REBUILD ROUND TIMER");
//                 break;
//             case roundType.scramble:
//                 _currentWeatherEvent = _weatherEvents[Random.Range(0,_weatherEvents.Length)]; // TODO logic for difficulty curvees will need to be added, probably in its own class
//                 Debug.Log($"The Next Weather Event is {_currentWeatherEvent.eventName}");
//                 EventTimerManager.CreateNewTimer(_scrambleTime, () => GoNextRound(), true, "_SCRAMBLE ROUND TIMER");
//                 break;
//             case roundType.survive:
//                 // start weather manager behaviour
//                 WeatherManager.StartEvent(_currentWeatherEvent);
                
//                 break;
//         }
//     }
// }
