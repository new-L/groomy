using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _passwordIpnutField;
    [SerializeField] private TMP_Text _loginInputField;
    [SerializeField] private List<Image> _ipnutFieldsBackground;
    [SerializeField] private Notification _notification;
    [SerializeField] private DigitKeyboard _keyboard;
    [SerializeField] private Toggle _saveToggle;

    [Space(10)]
    [Header("Server")]
    [SerializeField] private Authorization _authorization;

    [SerializeField] private Sprite _activeField, _inActiveField;
    [SerializeField] private SavePlayerPrefs _playerPrefs;

    private string _login = "", _password = "";

    public string Login { get => _login; set => _login = value; }
    public string Password { get => _password; set => _password = value; }
    public TMP_Text PasswordIpnutField { get => _passwordIpnutField; set => _passwordIpnutField = value; }
    public TMP_Text LoginInputField { get => _loginInputField; set => _loginInputField = value; }
    private void Start()
    {
        _activeField = Resources.Load<Sprite>("Art/UI/ActiveInputField"); 
        _inActiveField = Resources.Load<Sprite>("Art/UI/InactiveInputField");
        if(_playerPrefs.isPlayerPrefsExist())
        {
            _saveToggle.isOn = true;
            _loginInputField.text = _playerPrefs.LoadLogin();
            _login = _playerPrefs.LoadLogin();
            _password = _playerPrefs.LoadPassword();
            foreach (var item in _password)
            {
                _passwordIpnutField.text += "*";
            }
        }
    }
    public void DataEntry(bool isHide)
    {
        if (isHide)
        {
            _keyboard.FieldSetUp(isHide, this, _passwordIpnutField);
            ActiveInputField(_passwordIpnutField.GetComponentInParent<Image>());
        }
        else
        {
            _keyboard.FieldSetUp(isHide, this, _loginInputField);
            ActiveInputField(_loginInputField.GetComponentInParent<Image>());
        }
    }

    public void SendAuthorizationRequest()
    {
        if (!isFieldEmpty(LoginInputField) || !isFieldEmpty(PasswordIpnutField)) { _notification.NotificationIn("ќдин из полей пуст!"); return; }
        if (_saveToggle.isOn) _playerPrefs.SaveAuthDatas(Login, Password);
        else _playerPrefs.DeleteAuthDatas();
        _authorization.GetAuthorizationStatus(Login, Password);
    }

    private bool isFieldEmpty(TMP_Text inputField)
    {
        if (string.IsNullOrEmpty(inputField.text)) return false;
        return true;
    }

    private void ActiveInputField(Image currentField)
    {
        foreach (var item in _ipnutFieldsBackground)
        {
            item.sprite = _inActiveField;
        }
        currentField.sprite = _activeField;
    }

    public void DeactivateAllFields()
    {
        foreach (var item in _ipnutFieldsBackground)
        {
            item.sprite = _inActiveField;
        }
    }
}
