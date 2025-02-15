using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [SerializeField] private Image _hidePasswordImage;
    [SerializeField] private Sprite _hide;
    [SerializeField] private Sprite _eye;

    [Space(10)]
    [Header("Server")]
    [SerializeField] private Authorization _authorization;

    [SerializeField] private Sprite _activeField, _inActiveField;
    [SerializeField] private SavePlayerPrefs _playerPrefs;

    private string _login = "", _password = "";
    private bool _isNeedHide = true;

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
    public void DataEntry(bool isHide) { 
    //{
    //    if(isHide && !_isNeedHide)
    //    {
    //        print("Это сработало");
            
    //        return;
    //    }
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
        if (!isFieldEmpty(LoginInputField) || !isFieldEmpty(PasswordIpnutField)) { _notification.Set(NotificationType.Attention, "Один из полей пуст!"); _notification.Play(); return; }
        if (_saveToggle.isOn) _playerPrefs.SaveAuthDatas(Login, Password);
        else _playerPrefs.DeleteAuthDatas();
        _authorization.GetAuthorizationStatus(Login, Password);
    }

    public void HidePassword()
    {
        _isNeedHide = !_isNeedHide;
        _passwordIpnutField.text = "";
        if (_isNeedHide)
        {
            foreach (var item in _password)
            {
                _passwordIpnutField.text += "*";
            }
            _hidePasswordImage.sprite = _hide;
            _keyboard.FieldSetUp(true, false, this, _passwordIpnutField);
            ActiveInputField(_passwordIpnutField.GetComponentInParent<Image>());
        }
        else
        {
            foreach (var item in _password)
            {
                _passwordIpnutField.text += item;
            }
            _hidePasswordImage.sprite = _eye;
            _keyboard.FieldSetUp(true, true, this, _passwordIpnutField);
            ActiveInputField(_passwordIpnutField.GetComponentInParent<Image>());
        }
    }
    private void Update()
    {
        print(_isNeedHide);
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
