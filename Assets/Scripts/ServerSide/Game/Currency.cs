using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class Currency : MonoBehaviour
{
    #region UI
    [SerializeField] private UserUI _currencyUI;
    #endregion
    private static int gold;

    public int Gold { get => gold; set => gold = value; }

    private void Start()
    {
        GetPlayerCurrency();
    }
    private void GetPlayerCurrency()
    {
        StartCoroutine(SendToServer());
    }

    private IEnumerator SendToServer()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        UnityWebRequest www = UnityWebRequest.Post(URLs.UserCurrency, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        Debug.Log(www.downloadHandler.text);
        User.Currency = JsonUtility.FromJson<PlayerCurrency>(www.downloadHandler.text);
        _currencyUI.SetUpCurrencyValue();
    }
}

