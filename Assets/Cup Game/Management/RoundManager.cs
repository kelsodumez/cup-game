using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// The Round Manager is responsible for iterating through each round and calling its associated managers/scripts
/// </summary>
public class RoundManager : MonoBehaviour
{
    [Header("Parameters")]
    private static RoundManager _Instance;

    enum roundType
    {
        game_event,
        tutorial,
        modifier_wheel_spin //
    }

    private roundType currentRound = roundType.tutorial;

    void Awake()
    {
        _Instance = this;
        GameEventManager.OnEventEnd += ResolveRound;        
    }

    void Start()
    {
        EnactCurrentRound();
    }

    void OnDestroy()
    {
        GameEventManager.OnEventEnd -= GoNextRound;
    }

    private void ResolveRound(GameEvent previousGameEvent = null)
    {
        switch (currentRound)
        {
            case roundType.game_event:
                if (GameEventManager._roundWon)
                {
                     // handle win
                }  
                else
                {
                    // handle loss
                }
                break;
        }
        // Add loss condition here
        GoNextRound();


    }

    private void GoNextRound(GameEvent previousGameEvent = null)
    {
        // Random round type selection
        EnactCurrentRound();
    }
    private void EnactCurrentRound()
    {
        Debug.Log($"Current Round: {currentRound}");
        switch (currentRound)
        {
            case roundType.game_event:
                break;
            case roundType.tutorial:
                break;
            case roundType.modifier_wheel_spin:
                break;
    }
}
}
