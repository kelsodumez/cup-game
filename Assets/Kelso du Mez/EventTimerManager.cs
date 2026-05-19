using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EventTimerManager : MonoBehaviour
{
    private static EventTimerManager _Instance;


    #pragma warning disable 0067 // unity is annoying about this event not being used
    public static event Action OnDestroyAllTimers;
    #pragma warning restore 0067    


    private void Awake()
    {
        _Instance = this;
    }

    public static EventTimer CreateNewTimer(float time, System.Action methodName, bool doStartTimer = false, string timerName = "_UNNAMED TIMER", bool doDestroySelf = true)
    {
        return _Instance.CreateTimerInternal(time, methodName, doStartTimer, timerName, doDestroySelf);
    }

    private EventTimer CreateTimerInternal(float time, System.Action methodName, bool doStartTimer = false, string timerName = "_UNNAMED TIMER", bool doDestroySelf=true)
    {
        GameObject newTimerObject = new GameObject(timerName);
        newTimerObject.transform.SetParent(transform);

        EventTimer newTimerComponent = newTimerObject.AddComponent<EventTimer>();
        newTimerComponent.Initialize(timerName, time, methodName, doStartTimer, doDestroySelf);

        if (doStartTimer)
        {
            newTimerComponent.StartTimer();
        }

        return newTimerComponent;
    }

    public static void ClearTimers()
    {
        OnDestroyAllTimers?.Invoke();
    }
}
