using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Threading;
using System;

public class EndGame : MonoBehaviour
{
    [Tooltip("In seconds")][SerializeField] private float _endGameDelay = 3f;
    [SerializeField] private Currency _userCurrency;
    [SerializeField] private LimitUI _limit;
    [SerializeField] private GameLimit _currentLimit;
    [SerializeField] private RatingServer _rating;
    [SerializeField] private SongManager _songManager;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private MultiplicatorList _multiplicatorList;

    private int _totalNotes, _totalProgress;
    private Melody _melody;

    [SerializeField] private UnityEvent _onGameEnded;
    #region SendedDatasFields
    [SerializeField] private UnityEvent _onDatasSended;
    public UnityEvent OnDatasSended { get => _onDatasSended; set => _onDatasSended = value; }
    #endregion

    private int _ratingReward, _currencyReward;

    [Header("UI")]
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _coinsRewardText;
    [SerializeField] private TMP_Text _ratingRewardText;
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _loader;

    public IEnumerator Waiter(int totalNotes, Melody melody)
    {
        yield return new WaitForSeconds(_endGameDelay);
        _totalNotes = totalNotes;
        _melody = melody;
        FinishTheGame();
    }

    public void CheckDatasSendedStatus()
    {
        if (!_userCurrency.IsSended || !_rating.IsSended) return;
        _menuButton.SetActive(true);
        _loader.SetActive(false);
    }

    public void Clear()
    {
        _ratingReward = 0;
        _currencyReward = 0;
        _totalNotes = 0;
    }

    private void FinishTheGame()
    {
        _onGameEnded?.Invoke();
        _totalProgress = (int)((double)ScoreManager.ComboScore / _totalNotes * 100);
        _multiplicatorList.ResetEvenIndex();
        if (_totalProgress < 50)
        {
            _statusText.text = "Поражение";
            _statusText.color = new Color32(255, 33, 46, 255);
            _coinsRewardText.text = "0";
            _userCurrency.IsSended = true;
            RatingRewardCalculation(RequestType.Subtract);
        }
        else
        {
            _statusText.text = "Победа";
            _statusText.color = new Color32(122, 254, 87, 255);
            StartCoroutine(SendCalculation());
            _limit.ReduceAndSet();
        }
    }

    private void CoinsAdd(int add)
    {
        _currencyReward += add;
        if (_currentLimit.Limit.current <= 0) _currencyReward = 0;
        _coinsRewardText.text = $"+{_currencyReward}"; 
    }

    private void RatingAdd(int add)
    {
        _ratingReward += add;
        _ratingRewardText.text = $"+{_ratingReward}";
    }
    private void RatingRewardCalculation(RequestType type)
    {
        RatingCalculation(_totalProgress, type);
        _ratingRewardText.text = $"+{_ratingReward}";
        if(type == RequestType.Subtract) _ratingRewardText.text = $"-{_ratingReward}";
        _rating.Counting(_ratingReward, type);        
    }

    private void RatingCalculation(int totalProgress, RequestType type)
    {
        _ratingReward = totalProgress / 10 * (int)SongManager.DifficultLevel;
        if (type == RequestType.Subtract) _ratingReward = 10 * (int)SongManager.DifficultLevel;
    }

    private IEnumerator SendCalculation()
    {
        CoinsAdd(_melody.reward);//Считаем базу
        RatingAdd(_totalProgress / 10);
        yield return new WaitForSecondsRealtime(.4f);

        CoinsAdd(CurrencyMultiplicatorCalc());//Считаем по сложности
        RatingAdd(RatingMultiplicatorCalc());//Считаем по сложности
        _multiplicatorList.SetUpListElement("Сложность",
            SongManager.DifficultLevel.ToString(),
            _currencyReward.ToString(),
            _ratingReward.ToString());
        

        yield return new WaitForSecondsRealtime(.4f);
        CoinsAdd(ComboXCalc(_scoreManager.ComboX5Count, 0.15f, true)); //Считаем по х5
        RatingAdd(ComboXCalc(_scoreManager.ComboX5Count, .3f, false));
        _multiplicatorList.SetUpListElement("Комбо x5",
            _scoreManager.ComboX5Count.ToString(),
            ComboXCalc(_scoreManager.ComboX5Count, 0.15f, true).ToString(),
            ComboXCalc(_scoreManager.ComboX5Count, .3f, false).ToString());

        yield return new WaitForSecondsRealtime(.4f);
        CoinsAdd(ComboXCalc(_scoreManager.ComboX10Count, 0.4f, true)); //Считаем по х10
        RatingAdd(ComboXCalc(_scoreManager.ComboX10Count, .5f, false));
        _multiplicatorList.SetUpListElement("Комбо x10",
            _scoreManager.ComboX10Count.ToString(),
            ComboXCalc(_scoreManager.ComboX10Count, 0.4f, true).ToString(),
            ComboXCalc(_scoreManager.ComboX10Count, .5f, false).ToString());

        if (_currentLimit.Limit.current <= 0) _currencyReward = 0;
        _userCurrency.Add(_currencyReward);
        _rating.Counting(_ratingReward, RequestType.Add);
    }

    private int ComboXCalc(int combo, float multiplicator, bool isCurrency)
    {
        if (_currentLimit.Limit.current <= 0 && isCurrency) { _currencyReward = 0; return 0; }
        return (int)(combo * multiplicator / 2);
    }
    private int CurrencyMultiplicatorCalc()
    {
        if (_currentLimit.Limit.current <= 0)
            return 0;
        else
            return _currencyReward * (int)SongManager.DifficultLevel - _melody.reward;
    }
    private int RatingMultiplicatorCalc()
    {
        return _ratingReward * (int)SongManager.DifficultLevel - _ratingReward;
    }
}
