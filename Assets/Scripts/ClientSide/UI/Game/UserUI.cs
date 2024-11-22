using TMPro;
using UnityEngine;

public class UserUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _userNickname;
    
    [Header("Currency")]
    [SerializeField] private TMP_Text _currency;
    [SerializeField] private TMP_Text _currencyNotification;
    [SerializeField] private Animator _currencyNotificationAnimator;
    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;

    private bool isSceneStart;

    private void Start()
    {
        isSceneStart = true;
        NotificationReset();
        SetUpUserNickname();        
    }
    private void SetUpUserNickname()
    {
        _userNickname.text = User.Player.login;
    }

    public void SetUpCurrencyValue()
    { 
        NotificationReset();
        _inGameLoader.SetLoader(_loader);
        if (Currency.Gold > User.Currency.gold_count && !isSceneStart) //TODO: анимация минуса
        {
            _currencyNotification.gameObject.SetActive(true);
            _currencyNotificationAnimator.SetBool("Substract", true);
            _currencyNotification.text = "-" + Mathf.Abs(Currency.Gold - User.Currency.gold_count).ToString();
        }
        else if (Currency.Gold < User.Currency.gold_count && !isSceneStart) //TODO: анимация плюса
        {
            _currencyNotification.gameObject.SetActive(true);
            _currencyNotificationAnimator.SetBool("Add", true);
            _currencyNotification.text = "+" + Mathf.Abs(Currency.Gold - User.Currency.gold_count).ToString();
        }
        else if(isSceneStart)
        {
            InGameLoader.Tables[DBTablesName.UserCurrency] = true;
            InGameLoader.IsBorryActivate = true;
        }
        isSceneStart = false;
        _currency.text = User.Currency.gold_count.ToString();
        Actions.OnUserDatasLoad?.Invoke();
    }

    public void NotificationReset()
    {
        _currencyNotificationAnimator.SetBool("Add", false);
        _currencyNotificationAnimator.SetBool("Substract", false);
        _currencyNotification.gameObject.SetActive(false);
    }
}
