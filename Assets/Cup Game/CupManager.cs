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
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _spawnAnchor.transform.position += new Vector3(-_distanceMultiplier,0, 0); // TODO, once round manager controls cup amount this will position correctly with more cup
        //Subscribe begin round to RoundManager begin Round event instaed
        BeginRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginRound()
    {
        int cupAmount = 3;
        GameObject[] roundCups = GenerateCupArray(cupAmount);
        _winningCup = roundCups[UnityEngine.Random.Range(0, roundCups.Length -1)];
        Debug.Log($"Winning Cup will be: {_winningCup.name}");
    }
    private void EndRound(bool guessCorrect)
    {
        
        // invoke next round event
    }



    private GameObject[] GenerateCupArray(int cupAmount)
    {
        GameObject[] cupsArray = new GameObject[cupAmount];
        for (int count = 0; count < cupAmount; count++)
        {
            GameObject newCup = Instantiate(_cupObject, _spawnAnchor);
            newCup.name = $"CUP: {count}";
            newCup.transform.position += new Vector3(count * _distanceMultiplier, 0 , 0);;
            newCup.GetComponent<ClickableObject>().Initialize(() => CupSelected(newCup)); //cannot pass through array value to gameobject value in this call

            cupsArray[count] = newCup;
            
        }
        return cupsArray;
    }


    public void CupSelected(GameObject selectedCup)
    {
        Debug.Log($"Guessed: {selectedCup.name}");
        EndRound(selectedCup == _winningCup);
    }
}