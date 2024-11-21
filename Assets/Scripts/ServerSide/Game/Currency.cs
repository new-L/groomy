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

    public static int Gold { get => gold; set => gold = value; }

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
        StopCoroutine(nameof(Waiter));
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        UnityWebRequest www = UnityWebRequest.Post(URLs.UserCurrency, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) 
        { 
            Debug.Log("Не удалось связаться с сервером!");
            StartCoroutine(nameof(Waiter));
            yield break; 
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if(User.Currency != null) gold = User.Currency.gold_count;
            User.Currency = JsonUtility.FromJson<PlayerCurrency>(www.downloadHandler.text);
            _currencyUI.SetUpCurrencyValue();
            StartCoroutine(nameof(Waiter));
        }
    }

    private IEnumerator Waiter()
    {
        Debug.Log("Waiter is working!");
        yield return new WaitForSecondsRealtime(ServerSettings.Cooldown);
        StartCoroutine(nameof(SendToServer));
        Debug.Log("AFTER " + ServerSettings.Cooldown + " seconds waiter is working!");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

