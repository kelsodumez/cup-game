using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class CupManager : MonoBehaviour
{

    [SerializeField] private GameObject _cupObject;
    [SerializeField] private Transform _spawnAnchor;
    [SerializeField] private Transform _dealerPocket;

    [SerializeField][Range(0, 10)] private float _distanceMultiplier = 2f;
    private GameObject _winningCup;
    static private  bool _winnerSelected = false;
    private GameEvent _currentGameEvent;
    private List<GameObject> _roundCups;
    private bool _hasGuessed = false;
    private bool _canGuess = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        GameEventManager.OnEventBegin += BeginRound;
        GameEventManager.OnScrambleBegin += ScrambleCups;
        GameEventManager.GuessingBegin += AllowGuess;
        GameEventManager.OnEventEnd += ClearGame;
        // _spawnAnchor.transform.position += new Vector3(-_distanceMultiplier,0, 0); // TODO, once round manager controls cup amount this will position correctly with more cup
        //Subscribe begin round to RoundManager begin Round event instaed
    }


    void OnDestroy()
    {
        GameEventManager.OnEventBegin -= BeginRound;
        GameEventManager.OnScrambleBegin -= ScrambleCups;
        GameEventManager.GuessingBegin -= AllowGuess;
        GameEventManager.OnEventEnd -= ClearGame;
    }

    public void BeginRound(GameEvent inGameEvent = null)
    {
        _currentGameEvent = inGameEvent;
        _canGuess = false;
        _hasGuessed = false;
        _winnerSelected = false;
        GenerateCupList(inGameEvent.cupAmount);
    }
    private void EndRound(bool roundWon)
    {
        GameEventManager.EndEvent(_currentGameEvent, roundWon);
    }

    private void AllowGuess()
    {
        _canGuess = true;
    }

    static public bool retrieveGuess()
    {
        return _winnerSelected;
    }

    private void GenerateCupList(int cupAmount)
    {
        float deltaRadianPlacement = 360f/cupAmount;
        List<GameObject> cupsList = new List<GameObject>();
        Vector3 sourceVector = new Vector3(_spawnAnchor.position.x * _distanceMultiplier, _spawnAnchor.position.y, _spawnAnchor.position.z);
        for (int count = 0; count < cupAmount; count++)
        {
            GameObject newCup = Instantiate(_cupObject, _spawnAnchor);
            newCup.name = $"CUP: {count}";

            // place cups evenly across a N-sided Shape where n = {cupAmount}
            Quaternion newCupRotation = Quaternion.AngleAxis(deltaRadianPlacement * count, Vector3.up);
            Vector3 newCupPos = newCupRotation * sourceVector;
            newCup.transform.position = newCupPos;
            // newCup.transform.position += new Vector3(count * _distanceMultiplier, 0 , 0);;

            // Add ClickableObject Class to newCup  
            newCup.GetComponent<ClickableObject>().Initialize(() => CupSelected(newCup));
            cupsList.Add(newCup);
        }
        _roundCups = cupsList;
        PlaceCups();
    }

    private void PlaceCups()
    {
        Debug.Log("Placing Cups");
        StartCoroutine(PlaceCup(0));
    }

    System.Collections.IEnumerator PlaceCup(int cupIndex)
    {
        if (cupIndex > _roundCups.Count)
        {
            // won't display the winning cup until
            DisplayWinningCup();
            yield break;
        }
        else if (cupIndex > 0)
        {
            yield return new WaitUntil(() => !_roundCups[cupIndex - 1].GetComponent<LerpableObject>().IsLerping();
        }
        //cup anim code
        GameObject currentCup = _roundCups[cupIndex];
        Vector3 prevCupPosition = currentCup.transform.position;
        currentCup.transform.position = _dealerPocket.position;
        currentCup.GetComponent<MeshRenderer>().enabled = true;
        currentCup.GetComponent<LerpableObject>().BeginLerpingToPoint(prevCupPosition, _currentGameEvent.cupMoveSpeed);
        
        StartCoroutine(PlaceCup(cupIndex + 1));

    }

    private void DisplayWinningCup()
    {
        _winningCup = _roundCups[UnityEngine.Random.Range(0, _roundCups.Count)];
        DoShuffle();
        _winningCup.transform.GetComponent<Material>().color = Color.red; // temp
        Debug.Log("Displaying Winning Cup");
        EventTimerManager.CreateNewTimer(_currentGameEvent.displayDuration, () => GameEventManager.ScramblePhase(_currentGameEvent), true, $"TIMER");
    }

    

    private void ScrambleCups(GameEvent inGameEvent)
    {
        Debug.Log("Shuffling Cups");
        StartCoroutine(ShuffleOnceLerped(0, inGameEvent.noShuffles - 1));
    }

    System.Collections.IEnumerator ShuffleOnceLerped(int currentShuffle, int maxShuffles)
    {
        Debug.Log(currentShuffle);
        if (currentShuffle > maxShuffles)
        {
            Debug.Log("Reach");
            GameEventManager.GuessPhase(_currentGameEvent);
            yield break;
        }
        yield return new WaitUntil(() => CupsDoneLerping());
        DoShuffle();
        StartCoroutine(ShuffleOnceLerped(currentShuffle + 1, maxShuffles));
    }
    private void DoShuffle()
    {
        List<(GameObject,GameObject)> cupPairs = new List<(GameObject, GameObject)>();
        List<GameObject> availableCups = new List<GameObject>();
        availableCups.AddRange(_roundCups);
        float pairCount = _currentGameEvent.cupAmount/2;
        for (int count  = 0; count < pairCount; count++)
        {
            int availIndex = Mathf.Clamp(count, 0, availableCups.Count - 1);
            GameObject currentCup = availableCups[availIndex];
            availableCups.RemoveAt(availIndex);
            GameObject randCup = availableCups[UnityEngine.Random.Range(0, availableCups.Count)];
            cupPairs.Add((currentCup, randCup));
            availableCups.Remove(randCup);
        }

        foreach ((GameObject, GameObject) cupPair in cupPairs)
        {
            Vector3 cupOnePos = cupPair.Item1.transform.position;
            Vector3 cupTwoPos = cupPair.Item2.transform.position;

            cupPair.Item1.GetComponent<LerpableObject>().BeginLerpingToPoint(cupTwoPos, _currentGameEvent.cupMoveSpeed);
            cupPair.Item2.GetComponent<LerpableObject>().BeginLerpingToPoint(cupOnePos, _currentGameEvent.cupMoveSpeed);
        }
    }

    private bool CupsDoneLerping()
    {
        foreach (GameObject cup in _roundCups)
        {
            // if any cup is currently lerping, return false
            if (cup.GetComponent<LerpableObject>().IsLerping())
            {
                return false;
            }
        }
        return true;
    }


    public void CupSelected(GameObject selectedCup)
    {
        if (_canGuess && !_hasGuessed)
        {
            _hasGuessed = true;
            EndRound(selectedCup == _winningCup); 
        }
    }

    private void ClearGame(GameEvent inGameEvent, bool gameWon)
    {
        foreach (GameObject cup in _roundCups)
        {
            Destroy(cup);
        }
    }
}