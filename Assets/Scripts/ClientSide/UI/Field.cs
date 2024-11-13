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

    [Space(10)]
    [Header("Server")]
    [SerializeField] private Authorization _authorization;

    [SerializeField] private Sprite _activeField, _inActiveField;

    private string _login = "", _password = "";

    public string Login { get => _login; set => _login = value; }
    public string Password { get => _password; set => _password = value; }
    public TMP_Text PasswordIpnutField { get => _passwordIpnutField; set => _passwordIpnutField = value; }
    public TMP_Text LoginInputField { get => _loginInputField; set => _loginInputField = value; }
    private void Start()
    {
        _activeField = Resources.Load<Sprite>("Art/UI/ActiveInputField"); 
        _inActiveField = Resources.Load<Sprite>("Art/UI/InactiveInputField");
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
