using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdminPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _adminName;


    private void Start()
    {
        Actions.OnStartLoad?.Invoke();
        SetUpBaseDatas();
    }

    private void SetUpBaseDatas()
    {
        _adminName.text = User.Player.login + " | " + User.Player.user_type;
        Actions.OnListCreated?.Invoke();
    }

}
