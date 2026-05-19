using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

/// <summary>
/// The Round Manager is responsible for iterating through each round and calling its associated managers/scripts
/// </summary>
public class RoundManager : MonoBehaviour
{
    [Header("Parameters")]
    private static RoundManager _Instance;
    [SerializeField] private GameEvent[] _gameEvents;
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;
    private int _roundCount = 0;
    private int _roundDifficulty = 0;
    public UnityEvent onHealthChange;

    enum roundType
    {
        game_event,
        modifier_wheel_spin,
        game_end
    }

    private roundType _currentRound = roundType.game_event;

    void Awake()
    {
        _Instance = this;
        GameEventManager.OnEventEnd += ResolveRound;  
    }

    void Start()
    {
        _currentHealth = _maxHealth;
        onHealthChange.Invoke();     
        EnactCurrentRound();
    }

    void OnDestroy()
    {
        GameEventManager.OnEventEnd -= ResolveRound;  

    }

    private void ResolveRound(GameEvent previousGameEvent = null, bool roundWon = false)
    {
        switch (_currentRound)
        {
            case roundType.game_event:
                if (roundWon)
                {
                    _roundCount++;
                    if (_roundCount % 3 == 0)
                    {
                        _currentRound = roundType.modifier_wheel_spin;
                    }
                    _roundDifficulty += 1/_roundCount; // UNTESTED
                }  
                else
                {
                    _currentHealth--;
                    onHealthChange.Invoke();
                    if (_currentHealth <= 0)
                    {
                        _currentRound = roundType.game_end;
                    }
                }
                break;
            case roundType.modifier_wheel_spin:
            {
                _currentRound = roundType.game_event;
                break;
            }
        }
        EnactCurrentRound();
    }

    private void EnactCurrentRound()
    {
        Debug.Log($"Current Round: {_currentRound}");
        switch (_currentRound)
        {
            case roundType.game_event:

                // GameEventManager.StartGame(_gameEvents[UnityEngine.Random.Range(0, _gameEvents.Length-1)]);
                // GameEventManager.StartGame(_gameEvents[_roundDifficulty]);
                GameEventManager.StartGame(_gameEvents[0]);
                break;
            case roundType.modifier_wheel_spin:
                break;
            case roundType.game_end:
                // exit to leaderboard ui
                break;
        }
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public int GetMaximumHealth()
    {
        return _maxHealth;
    }

    public int GetRoundCount()
    {
        return _roundCount;
    }
}

