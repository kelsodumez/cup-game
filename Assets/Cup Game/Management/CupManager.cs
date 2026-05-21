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
    private bool _shuffleInProgress = false;
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
        foreach (GameObject cup in _roundCups)
        {
            cup.GetComponent<HoverableObject>().AllowHovering(true);
        }
        _canGuess = true;
    }

    static public bool retrieveGuess()
    {
        return _winnerSelected;
    }

    private void GenerateCupList(int cupAmount)
    {
        float deltaDegreePlacement = 360f/cupAmount;
        List<GameObject> cupsList = new List<GameObject>();
        Vector3 sourceVector = new Vector3(_spawnAnchor.position.x * _distanceMultiplier, _spawnAnchor.position.y, _spawnAnchor.position.z);
        Debug.Log(sourceVector);
        for (int count = 0; count < cupAmount; count++)
        {
            GameObject newCup = Instantiate(_cupObject, _spawnAnchor);
            newCup.name = $"CUP: {count}";

            // place cups evenly across a N-sided Shape where n = {cupAmount}
            Quaternion newCupRotation = Quaternion.AngleAxis(deltaDegreePlacement * count, Vector3.up);
            Vector3 newCupPos = newCupRotation * sourceVector;
            newCup.transform.position = newCupPos;
            // newCup.transform.position += new Vector3(count * _distanceMultiplier, 0 , 0);;

            // Add ClickableObject Class to newCup  
            newCup.GetComponent<ClickableObject>().Initialize(() => CupSelected(newCup));
            newCup.GetComponent<HoverableObject>().AllowHovering(false);

            cupsList.Add(newCup);
        }
        _roundCups = cupsList;
        DealCups();
    }

    private void DealCups()
    {
        Debug.Log("Placing Cups");
        StartCoroutine(PlaceCup(0, ArmController.hand_target.left));
    }

    System.Collections.IEnumerator PlaceCup(int cupIndex, ArmController.hand_target prevHand)
    {
        yield return new WaitUntil(() => ArmController.ArmsStationary());

        if (cupIndex >= _roundCups.Count)
        {
            // won't display the winning cup until all cups dealt
            DisplayWinningCup();
            yield break;
        }
        else if (cupIndex > 0)
        {
            yield return new WaitUntil(() => !_roundCups[cupIndex - 1].GetComponent<LerpableObject>().IsLerping());
        }
        //cup anim code
        GameObject currentCup = _roundCups[cupIndex];

        Vector3 prevCupPosition = currentCup.transform.position;
        currentCup.transform.position = _dealerPocket.position;
        currentCup.GetComponentInChildren<MeshRenderer>().enabled = true;

        currentCup.GetComponent<LerpableObject>().BeginLerpingToVector(prevCupPosition, _currentGameEvent.cupMoveSpeed);
        ArmController.hand_target currentHand = ArmController.hand_target.left;
        switch (prevHand)
        {
            case ArmController.hand_target.left:
                currentHand = ArmController.hand_target.right;
                ArmController.SetTarget(currentHand, currentCup.transform);
                break;
            case ArmController.hand_target.right:
                currentHand = ArmController.hand_target.left;
                ArmController.SetTarget(currentHand, currentCup.transform);
                break;
        }
        StartCoroutine(PlaceCup(cupIndex + 1, currentHand));
    }

    private void DisplayWinningCup()
    {
        // ArmController.ResetHand(ArmController.hand_target.both);

        _winningCup = _roundCups[UnityEngine.Random.Range(0, _roundCups.Count)];
        // ArmController.SetTarget(false, _winningCup.transform);
        _winningCup.transform.GetComponentInChildren<MeshRenderer>().material.color = Color.red; // temp
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
        yield return new WaitUntil(() => CupsDoneShuffling());

        if (currentShuffle > maxShuffles)
        {
            GameEventManager.GuessPhase(_currentGameEvent);
            yield break;
        }
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
        _shuffleInProgress = true;
        StartCoroutine(LerpPair(0, cupPairs));
    }

    System.Collections.IEnumerator LerpPair(int pairIndex, List<(GameObject, GameObject)> cupPairs)
    {
        if (pairIndex >= cupPairs.Count)
        {
            // all cups shuffled
            _shuffleInProgress = false;
            yield break;
        }
        else if (pairIndex > 0)
        {
            // bool prevPairLerping = cupPairs[pairIndex - 1].Item1.GetComponent<LerpableObject>().IsLerping(); //&& cupPairs[pairIndex - 1].Item2.GetComponent<LerpableObject>().IsLerping();
            yield return new WaitUntil(() => !cupPairs[pairIndex - 1].Item1.GetComponent<LerpableObject>().IsLerping());
        }
        //cup anim code
        GameObject cupOne = cupPairs[pairIndex].Item1;
        GameObject cupTwo = cupPairs[pairIndex].Item2;

        ArmController.SetTarget(ArmController.hand_target.left, cupOne.transform);
        ArmController.SetTarget(ArmController.hand_target.right, cupTwo.transform);


        cupOne.GetComponent<LerpableObject>().BeginLerpingToVector(cupTwo.transform.position, _currentGameEvent.cupMoveSpeed);
        cupTwo.GetComponent<LerpableObject>().BeginLerpingToVector(cupOne.transform.position, _currentGameEvent.cupMoveSpeed);
        StartCoroutine(LerpPair(pairIndex + 1, cupPairs));
    }


    private bool CupsDoneShuffling()
    {
        if (_shuffleInProgress)
        {
            return false;
        }
        foreach (GameObject cup in _roundCups)
        {
            // if any cup is currently lerping return false
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