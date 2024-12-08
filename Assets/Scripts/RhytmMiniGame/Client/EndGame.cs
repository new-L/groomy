using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private float _endGameDelay = .2f;
    [SerializeField] private Currency _userCurrency;

    private int _ratingReward, _currencyReward;

    public void RewardCalculation(int totalNotes, Melody melody)
    {
        int totalProgress = (int)((double)ScoreManager.ComboScore / totalNotes * 100);
        Debug.Log(totalProgress);
        if (totalProgress < 50) //TODO: You lose;
        { }
        else
        {
            CurrencyCalculcation(melody);
            RatingCalculation(totalProgress);
            _userCurrency.Add(_currencyReward);
        }
    }


    private void CurrencyCalculcation(Melody melody)
    {
        switch (SongManager.DifficultLevel)
        {
            case DifficultLevel.Low:
                _currencyReward = melody.reward * (int)DifficultLevel.Low;
                break;
            case DifficultLevel.Medium:
                _currencyReward = melody.reward * (int)DifficultLevel.Medium;
                break;
            case DifficultLevel.High:
                _currencyReward = melody.reward * (int)DifficultLevel.High;
                break;
            default:
                _currencyReward = melody.reward * (int)DifficultLevel.Low;
                break;
        }
    }

    private void RatingCalculation(int totalProgress)
    {
        _ratingReward = totalProgress / 10 * (int)SongManager.DifficultLevel;
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(_endGameDelay);
        //TODO:
    }

}
