using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TMPDisplayVar : MonoBehaviour
{
    [SerializeField] private RoundManager _roundManager; 
    private TextMeshProUGUI _textAsset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
        try
        {
            _textAsset = transform.GetComponent<TextMeshProUGUI>();

        }
        catch
        {
            Debug.LogWarning($"No TMP Asset exists on {gameObject.name}");
        }
    }

    // Update is called once per frame
    public void UpdateText()
    {
        // String displayText = .ToString();
        _textAsset.SetText(_roundManager.GetCurrentHealth().ToString());
    }
}
