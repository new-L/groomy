using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Currency : MonoBehaviour
{
    #region UI
    [SerializeField] private UserUI _currencyUI;
    #endregion
    private static int gold;

    public static int Gold { get => gold; set => gold = value; }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
            GetPlayerCurrency();
    }
    private void GetPlayerCurrency()
    {
        StartCoroutine(SendToServer("get"));
    }

    public void Add(int count)
    {
        StartCoroutine(AddCurrency("add", count));
    }

    private IEnumerator SendToServer(string request)
    {
        StopCoroutine(nameof(Waiter));
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        form.AddField("request_type", request);
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

    private IEnumerator AddCurrency(string request, int count)
    {
        StopCoroutine(nameof(Waiter));
        WWWForm form = new WWWForm();
        form.AddField("user_id", 406788);//User.Player.user_id);
        form.AddField("request_type", request);
        if (request.Equals("add")) form.AddField("count", count);
        UnityWebRequest www = UnityWebRequest.Post(URLs.UserCurrency, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null)
        {
            Debug.Log("Не удалось связаться с сервером!");
            StartCoroutine(nameof(Waiter));
            yield break;
        }
    }

    private IEnumerator Waiter()
    {
        Debug.Log("Waiter is working!");
        yield return new WaitForSecondsRealtime(ServerSettings.Cooldown);
        StartCoroutine(SendToServer("get"));
        Debug.Log("AFTER " + ServerSettings.Cooldown + " seconds waiter is working!");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

