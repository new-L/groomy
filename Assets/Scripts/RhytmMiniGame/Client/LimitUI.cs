using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimitUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameLimit _gameLimit;


    public void Set()
    {
        if (!_text.gameObject.activeSelf) return;
        _text.text = $"{_gameLimit.Limit.current}/{_gameLimit.Limit.maximum}";
    }

    public void ReduceAndSet()
    {
        _gameLimit.Reduce();
    }


}
