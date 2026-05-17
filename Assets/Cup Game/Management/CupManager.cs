using System;
using System.Linq;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class CupManager : MonoBehaviour
{

    [SerializeField] private GameObject _cupObject;
    [SerializeField] private Transform _spawnAnchor;
    [SerializeField][Range(0, 10)] private float _distanceMultiplier = 2f;
    private GameObject _winningCup;
    static private  bool _winnerSelected = false;
    private GameEvent _currentGameEvent;
    private GameObject[] _roundCups;

    private bool _canGuess = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        GameEventManager.OnEventBegin += BeginRound;
        GameEventManager.GuessingBegin += AllowGuess;
        GameEventManager.OnEventEnd += ClearGame;
        _spawnAnchor.transform.position += new Vector3(-_distanceMultiplier,0, 0); // TODO, once round manager controls cup amount this will position correctly with more cup
        //Subscribe begin round to RoundManager begin Round event instaed
    }


    void OnDestroy()
    {
        GameEventManager.OnEventBegin -= BeginRound;
        GameEventManager.GuessingBegin -= AllowGuess;
        GameEventManager.OnEventEnd -= ClearGame;

    }

    public void BeginRound(GameEvent inGameEvent = null)
    {
        _canGuess = false;
        _winnerSelected = false;
        int stoneAmount = inGameEvent.stoneAmount;
        int cupAmount = inGameEvent.cupAmount;
        _roundCups = GenerateCupArray(cupAmount);
        _winningCup = _roundCups[UnityEngine.Random.Range(0, _roundCups.Length -1)];
        // Debug.Log($"Winning Cup will be: {_winningCup.name}");
    }
    private void EndRound()
    {
        GameEventManager.EndEvent(_currentGameEvent);
    }

    private void AllowGuess()
    {
        _canGuess = true;
    }

    static public bool retrieveGuess()
    {
        return _winnerSelected;
    }

    private GameObject[] GenerateCupArray(int cupAmount)
    {
        GameObject[] cupsArray = new GameObject[cupAmount];
        for (int count = 0; count < cupAmount; count++)
        {
            GameObject newCup = Instantiate(_cupObject, _spawnAnchor);
            newCup.name = $"CUP: {count}";
            newCup.transform.position += new Vector3(count * _distanceMultiplier, 0 , 0);;
            newCup.GetComponent<ClickableObject>().Initialize(() => CupSelected(newCup));

            cupsArray[count] = newCup;
            
        }
        return cupsArray;
    }


    public void CupSelected(GameObject selectedCup)
    {
        if (_canGuess)
        {
            Debug.Log($"Guessed: {selectedCup.name}");
            _winnerSelected = selectedCup == _winningCup;
            EndRound(); 
        }
        else
        {
            Debug.Log("Cup Selected Outside of Guess");
        }

    }

    private void ClearGame(GameEvent inGameEvent)
    {
        foreach (GameObject cup in _roundCups)
        {
            Destroy(cup);
        }
    }
}