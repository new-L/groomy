using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Threading;

public class EndGame : MonoBehaviour
{
    [Tooltip("In seconds")][SerializeField] private float _endGameDelay = 3f;
    [SerializeField] private Currency _userCurrency;
    [SerializeField] private RatingServer _rating;

    private int _totalNotes, _totalProgress;
    private Melody _melody;

    [SerializeField] private UnityEvent _onGameEnded;
    #region SendedDatasFields
    [SerializeField] private UnityEvent _onDatasSended;
    public UnityEvent OnDatasSended { get => _onDatasSended; set => _onDatasSended = value; }
    #endregion


    private int _ratingReward, _currencyReward;

    [Header("UI")]
    [SerializeField] private Image _statusPanel;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _coinsRewardText;
    [SerializeField] private TMP_Text _ratingRewardText;
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _loader;
    private const string PATH = "Art/UI/RhytmGame/";

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
        if(_totalProgress < 50)
        {
            _statusPanel.sprite = Resources.Load<Sprite>($"{PATH}EndGameLosePanel");
            _statusText.text = "Поражение";
            _statusText.color = new Color32(255, 33, 46, 255);
            _coinsRewardText.text = "0";
            RatingRewardCalculation(RequestType.Subtract);
        }
        else
        {
            _statusPanel.sprite = Resources.Load<Sprite>($"{PATH}EndGameWinPanel");
            _statusText.text = "Победа";
            _statusText.color = new Color32(122, 254, 87, 255);
            CoinsRewardCalculation(_totalNotes, _melody);
            RatingRewardCalculation(RequestType.Add);

        }

    }

    private void CoinsRewardCalculation(int totalNotes, Melody melody)
    {
        CurrencyCalculcation(melody);
        _coinsRewardText.text = $"+{_currencyReward}";
        _userCurrency.Add(_currencyReward);        
    }
    private void RatingRewardCalculation(RequestType type)
    {
        RatingCalculation(_totalProgress, type);
        _ratingRewardText.text = $"+{_ratingReward}";
        if(type == RequestType.Subtract) _ratingRewardText.text = $"-{_ratingReward}";
        _rating.Counting(_ratingReward, type);        
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

    private void RatingCalculation(int totalProgress, RequestType type)
    {
        _ratingReward = totalProgress / 10 * (int)SongManager.DifficultLevel;
        if (type == RequestType.Subtract) _ratingReward = 10;
    }




}
