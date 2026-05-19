using System;
using Unity.VisualScripting;
using UnityEngine;

public class EventTimer : MonoBehaviour
{
    [SerializeField] private System.Action onTimerEnd;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isActive = true;
    private bool _doDestroySelf = true;

    public void Initialize(string timerName, float time, System.Action methodName, bool doStartActive, bool doDestroySelf = true)
    {
        EventTimerManager.OnDestroyAllTimers += DestroySelf;

        transform.name = timerName;
        currentTime = time;
        onTimerEnd = methodName;
        isActive = doStartActive;
        _doDestroySelf = doDestroySelf;
    }

    void Update()
    {
        if (isActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                try
                {
                    onTimerEnd();
                }
                catch
                {
                    Debug.LogWarning($"{this.gameObject.name}: Unable to invoke method: {onTimerEnd.Method.Name}.");
                }

                isActive = false;

                if (_doDestroySelf)
                {
                    DestroySelf(); // TODO make this optional in function call
                }
            }
        }
    }

    public void StartTimer()
    {
        isActive = true;
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        EventTimerManager.OnDestroyAllTimers -= DestroySelf;
    }
}
