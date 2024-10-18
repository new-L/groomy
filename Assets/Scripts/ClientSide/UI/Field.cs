using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _passwordIpnutField;
    [SerializeField] private TMP_Text _loginInputField;
    [SerializeField] private Notification _notification;
    [SerializeField] private DigitKeyboard _keyboard;

    [Space(10)]
    [Header("Server")]
    [SerializeField] private Authorization _authorization;

    private string _login = "", _password = "";

    public string Login { get => _login; set => _login = value; }
    public string Password { get => _password; set => _password = value; }
    public TMP_Text PasswordIpnutField { get => _passwordIpnutField; set => _passwordIpnutField = value; }
    public TMP_Text LoginInputField { get => _loginInputField; set => _loginInputField = value; }

    public void DataEntry(bool isHide)
    {
        if(isHide) _keyboard.FieldSetUp(isHide, this, _passwordIpnutField);
        else _keyboard.FieldSetUp(isHide, this, _loginInputField);
    }

    public void SendAuthorizationRequest()
    {
        if (!isFieldEmpty(LoginInputField) || !isFieldEmpty(PasswordIpnutField)) { _notification.NotificationIn("ќдин из полей пуст!"); return; }
        _authorization.GetAuthorizationStatus(Login, Password);
    }

    private bool isFieldEmpty(TMP_Text inputField)
    {
        if (string.IsNullOrEmpty(inputField.text)) return false;
        return true;
    }
}
