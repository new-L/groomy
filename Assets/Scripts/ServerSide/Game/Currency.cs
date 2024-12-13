using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Currency : MonoBehaviour
{
    #region UI
    [SerializeField] private UserUI _currencyUI;
    #endregion
    [SerializeField] private EndGame _endGame;

    private static int _gold;
    private int _count;
    private bool _isSended;

    public static int Gold { get => _gold; set => _gold = value; }
    public bool IsSended { get => _isSended; }

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
        _isSended = false;
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
            if(User.Currency != null) _gold = User.Currency.gold_count;
            User.Currency = JsonUtility.FromJson<PlayerCurrency>(www.downloadHandler.text);
            _currencyUI.SetUpCurrencyValue();
            StartCoroutine(Waiter("get"));
        }
    }

    private IEnumerator AddCurrency(string request, int count)
    {
        StopCoroutine(nameof(Waiter));
        _count = count;
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        form.AddField("request_type", request);
        if (request.Equals("add")) form.AddField("count", count);
        UnityWebRequest www = UnityWebRequest.Post(URLs.UserCurrency, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null)
        {
            Debug.Log("Не удалось связаться с сервером!");
            StartCoroutine(Waiter("add"));
            yield break;
        }
        _isSended = true;
        _endGame?.OnDatasSended?.Invoke();
    }

    private IEnumerator Waiter(string request)
    {
        Debug.Log("Waiter is working!");
        yield return new WaitForSecondsRealtime(ServerSettings.Cooldown);
        if(request.Equals("get"))StartCoroutine(SendToServer(request));
        else if(request.Equals("add")) StartCoroutine(AddCurrency(request, _count));
        Debug.Log("AFTER " + ServerSettings.Cooldown + " seconds waiter is working!");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

