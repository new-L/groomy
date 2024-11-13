using TMPro;
using UnityEngine;

public class UserUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _currency;
    [SerializeField] private TMP_Text _userNickname;
    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;

    private void Start()
    {
        SetUpUserNickname();
    }
    private void SetUpUserNickname()
    {
        _userNickname.text = User.Player.login;
    }

    public void SetUpCurrencyValue()
    {
        _inGameLoader.SetLoader(_loader);
        _currency.text = User.Currency.gold_count.ToString();
        InGameLoader.Tables[DBTablesName.UserCurrency] = true;
        InGameLoader.IsBorryActivate = true;
        Actions.OnUserDatasLoad?.Invoke();
    }
}
