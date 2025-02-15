using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.ComponentModel;
using UnityEngine.UIElements;
using System;

public class DigitKeyboard : MonoBehaviour
{

    [SerializeField] private TMP_Text _digit, _activeField;
    [SerializeField] private Field _field;
    [SerializeField] private bool _isHide;
    [SerializeField] private bool _isPassword = false;

    public void FieldSetUp(bool isHide, bool isPassword, Field field, TMP_Text activeField)
    {
        Set(isHide, field, activeField);
        _isPassword = isPassword;
    }

    public void FieldSetUp(bool isHide, Field field, TMP_Text activeField)
    {
        Set(isHide, field, activeField);
        if (isHide)
            _field.Password = "";
        else
            _field.Login = "";
        _activeField.text = "";
    }


    public void SymboilSetUp(TMP_Text button)
    {
        _digit = button;
        if(_isHide && _isPassword)
        {
            _field.Password += _digit.text;
            _activeField.text += _digit.text;
            return;
        }
        if (_isHide)
        {
            _field.Password += _digit.text;
            _activeField.text += "*";
        }
        else
        {
            _field.Login += _digit.text;
            _activeField.text += _digit.text;
        }
    }

    public void DeleteLastSymbol()
    {
        if (string.IsNullOrEmpty(_activeField.text)) return;
        if (_isHide)
            _field.Password = _field.Password.Remove(_field.Password.Length - 1);
        else
            _field.Login = _field.Login.Remove(_field.Login.Length - 1);
        _activeField.text = _activeField.text.Remove(_activeField.text.Length - 1);
    }


    private void Set(bool isHide, Field field, TMP_Text activeField)
    {
        _isHide = isHide;
        _field = field;
        _activeField = activeField;
    }

}
